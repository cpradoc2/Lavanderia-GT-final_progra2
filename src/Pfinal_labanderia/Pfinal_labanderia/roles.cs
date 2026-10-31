using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Entidad;
using FontAwesome.Sharp;
using Negocio;

namespace Pfinal_labanderia
{
    /// <summary>
    /// CRUD sencillo para los roles y la configuración de accesos del menú.
    /// Este formulario se arma por código (no tiene Designer.cs). Por eso aquí se aplican a mano
    /// los mismos colores, fuentes, iconos y estilo de tabla que tienen los demás módulos del proyecto.
    /// </summary>
    public class roles : Form
    {
        // Paleta del proyecto: los mismos colores que usan los formularios CRUD y el menú.
        private static readonly Color Fondo = Color.FromArgb(240, 245, 252);
        private static readonly Color Titulo = Color.FromArgb(16, 42, 110);
        private static readonly Color Texto = Color.FromArgb(45, 60, 85);
        private static readonly Color Borde = Color.FromArgb(200, 214, 235);
        private static readonly Color Azul = Color.FromArgb(13, 110, 253);
        private static readonly Color AzulHover = Color.FromArgb(11, 94, 215);
        private static readonly Color Rojo = Color.FromArgb(220, 53, 69);
        private static readonly Color RojoHover = Color.FromArgb(187, 45, 59);

        private readonly NRol negocio = new NRol();
        private readonly DataGridView tabla = new DataGridView();
        private readonly TextBox txtNombre = new TextBox();
        private readonly TextBox txtDescripcion = new TextBox();
        private readonly CheckBox chkActivo = new CheckBox();
        private readonly CheckedListBox listaAccesos = new CheckedListBox();
        private int idRol;

        public roles()
        {
            // SuspendLayout/ResumeLayout: se arma todo el formulario y se dibuja una sola vez al final.
            SuspendLayout();
            Text = "Administración de roles";
            ClientSize = new Size(800, 500);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Fondo;
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            // Título centrado arriba, igual que en Clientes, Tarifas, etc.
            Controls.Add(new Label
            {
                Text = "Roles y accesos",
                AutoSize = false,
                Location = new Point(12, 0),
                Size = new Size(776, 42),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Titulo
            });

            // "Tarjeta" blanca con borde celeste: cumple el papel de las pestañas blancas de los otros módulos.
            Panel tarjeta = new Panel { Location = new Point(12, 46), Size = new Size(776, 440), BackColor = Color.White };
            tarjeta.Paint += (s, e) =>
            {
                using (Pen pluma = new Pen(Borde))
                    e.Graphics.DrawRectangle(pluma, 0, 0, tarjeta.Width - 1, tarjeta.Height - 1);
            };
            Controls.Add(tarjeta);

            // Tabla de roles (izquierda).
            tabla.Location = new Point(20, 20); tabla.Size = new Size(400, 340);
            tabla.ReadOnly = true; tabla.MultiSelect = false;
            tabla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tabla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tabla.CellClick += Tabla_CellClick;
            tabla.DataBindingComplete += (s, e) => PonerEncabezados();
            AplicarEstiloTabla();
            tarjeta.Controls.Add(tabla);

            // Datos del rol (derecha).
            tarjeta.Controls.Add(CrearEtiqueta("Nombre", 445, 18));
            EstiloCaja(txtNombre, 445, 44); tarjeta.Controls.Add(txtNombre);
            tarjeta.Controls.Add(CrearEtiqueta("Descripción", 445, 78));
            EstiloCaja(txtDescripcion, 445, 104); tarjeta.Controls.Add(txtDescripcion);
            chkActivo.Text = "Activo"; chkActivo.Location = new Point(445, 140); chkActivo.AutoSize = true;
            chkActivo.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            chkActivo.ForeColor = Texto; chkActivo.Checked = true;
            tarjeta.Controls.Add(chkActivo);
            tarjeta.Controls.Add(CrearEtiqueta("Accesos que puede abrir", 445, 170));
            listaAccesos.Location = new Point(445, 196); listaAccesos.Size = new Size(310, 164);
            listaAccesos.BorderStyle = BorderStyle.FixedSingle;
            listaAccesos.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            listaAccesos.ForeColor = Texto;
            listaAccesos.CheckOnClick = true;   // Un solo clic marca o desmarca el acceso.
            tarjeta.Controls.Add(listaAccesos);

            // Botones con el estilo del proyecto: azul = acción principal, rojo = acción peligrosa.
            tarjeta.Controls.Add(CrearBoton("Nuevo", IconChar.Plus, Azul, AzulHover, 20, 380, (s, e) => Limpiar()));
            tarjeta.Controls.Add(CrearBoton("Guardar", IconChar.FloppyDisk, Azul, AzulHover, 150, 380, Guardar_Click));
            tarjeta.Controls.Add(CrearBoton("Eliminar", IconChar.TrashCan, Rojo, RojoHover, 280, 380, Eliminar_Click));
            Load += (s, e) => Consultar();

            // Las medidas de arriba están pensadas para 96 DPI (pantalla al 100 %).
            // Con AutoScaleMode.Dpi, Windows las agranda solo si la pantalla usa otra escala (125 %, 150 %...).
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ResumeLayout(false);
            PerformLayout();
        }

        // ---------------------------------------------------------------
        // ESTILO (solo apariencia; no cambia lo que hace el formulario)
        // ---------------------------------------------------------------

        private Label CrearEtiqueta(string texto, int x, int y)
        {
            return new Label
            {
                Text = texto, Location = new Point(x, y), AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point), ForeColor = Texto
            };
        }

        private void EstiloCaja(TextBox caja, int x, int y)
        {
            caja.Location = new Point(x, y); caja.Width = 310;
            caja.BorderStyle = BorderStyle.FixedSingle;
            caja.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
        }

        /// <summary>Botón con icono, igual a Nuevo/Guardar/Eliminar de los demás módulos.</summary>
        private IconButton CrearBoton(string texto, IconChar icono, Color fondo, Color hover, int x, int y, EventHandler evento)
        {
            IconButton boton = new IconButton
            {
                Text = texto, IconChar = icono, IconColor = Color.White, IconFont = IconFont.Auto, IconSize = 18,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Location = new Point(x, y), Size = new Size(120, 38),
                FlatStyle = FlatStyle.Flat, BackColor = fondo, ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point),
                Cursor = Cursors.Hand, UseVisualStyleBackColor = false
            };
            boton.FlatAppearance.BorderSize = 0;
            boton.FlatAppearance.MouseOverBackColor = hover;
            boton.FlatAppearance.MouseDownBackColor = hover;
            boton.Click += evento;
            return boton;
        }

        /// <summary>Misma tabla que los CRUD: blanca, sin bordes, encabezado azul marino y filas alternas.</summary>
        private void AplicarEstiloTabla()
        {
            tabla.BackgroundColor = Color.White;
            tabla.BorderStyle = BorderStyle.None;
            tabla.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            tabla.GridColor = Color.FromArgb(230, 236, 245);
            tabla.RowHeadersVisible = false;
            tabla.AllowUserToAddRows = false;
            tabla.AllowUserToDeleteRows = false;
            tabla.AllowUserToResizeRows = false;
            tabla.EnableHeadersVisualStyles = false;
            tabla.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            tabla.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(243, 246, 251);
            tabla.ColumnHeadersDefaultCellStyle.ForeColor = Titulo;
            tabla.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(243, 246, 251);
            tabla.ColumnHeadersDefaultCellStyle.SelectionForeColor = Titulo;
            tabla.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            tabla.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            tabla.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            tabla.DefaultCellStyle.ForeColor = Texto;
            tabla.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            tabla.DefaultCellStyle.SelectionForeColor = Titulo;
            tabla.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 253);
        }

        /// <summary>Encabezados en español con tildes, como en los demás módulos.</summary>
        private void PonerEncabezados()
        {
            DataGridViewColumnCollection columnas = tabla.Columns;
            if (columnas.Contains("IdRol")) { columnas["IdRol"].HeaderText = "Código"; columnas["IdRol"].FillWeight = 45; }
            if (columnas.Contains("NombreRol")) columnas["NombreRol"].HeaderText = "Rol";
            if (columnas.Contains("Descripcion")) columnas["Descripcion"].HeaderText = "Descripción";
            if (columnas.Contains("Estado")) { columnas["Estado"].HeaderText = "Activo"; columnas["Estado"].FillWeight = 45; }
        }

        // ---------------------------------------------------------------
        // LÓGICA DEL CRUD (sin cambios)
        // ---------------------------------------------------------------

        private void Consultar()
        {
            try { tabla.DataSource = negocio.Consultar(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Roles", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void Tabla_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            try
            {
                Rol rol = (Rol)tabla.Rows[e.RowIndex].DataBoundItem;
                idRol = rol.IdRol; txtNombre.Text = rol.NombreRol; txtDescripcion.Text = rol.Descripcion;
                chkActivo.Checked = rol.Estado;
                listaAccesos.Items.Clear();
                foreach (Acceso acceso in negocio.ConsultarAccesos(idRol))
                    listaAccesos.Items.Add(acceso, acceso.Permitido);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Roles", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void Guardar_Click(object sender, EventArgs e)
        {
            try
            {
                List<Acceso> accesos = listaAccesos.Items.Cast<Acceso>().ToList();
                for (int i = 0; i < accesos.Count; i++) accesos[i].Permitido = listaAccesos.GetItemChecked(i);
                negocio.Guardar(new Rol { IdRol = idRol, NombreRol = txtNombre.Text, Descripcion = txtDescripcion.Text, Estado = chkActivo.Checked }, accesos);
                Consultar(); Limpiar();
                MessageBox.Show("Rol y accesos guardados.", "Roles", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Roles", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void Eliminar_Click(object sender, EventArgs e)
        {
            if (idRol == 0) { MessageBox.Show("Seleccione un rol."); return; }
            if (MessageBox.Show("¿Eliminar el rol seleccionado?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try { negocio.Eliminar(idRol); Consultar(); Limpiar(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Roles", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void Limpiar()
        {
            idRol = 0; txtNombre.Clear(); txtDescripcion.Clear(); chkActivo.Checked = true;
            listaAccesos.Items.Clear();
            try { foreach (Acceso acceso in negocio.ConsultarAccesos(0)) listaAccesos.Items.Add(acceso, false); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Roles", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}
