using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FontAwesome.Sharp;
using Entidad;
using Negocio;

namespace Pfinal_labanderia
{
    public partial class usuarios : Form
    {
        // Capa Negocio: la Presentación no usa la capa Datos ni SQL Server directamente.
        private readonly NUsuario negocioUsuarios = new NUsuario();
        private readonly NRol negocioRoles = new NRol();
        private readonly ComboBox comboRol = new ComboBox();
        private readonly Label etiquetaRol = new Label();

        // ---------------------------------------------------------------
        // PAGINACIÓN (solo en la capa Presentación)
        // ---------------------------------------------------------------
        private const int RegistrosPorPagina = 10;
        private List<Usuario> listaUsuarios = new List<Usuario>();   // TODOS los usuarios consultados
        private int paginaActual = 1;

        // Controles de paginación: se crean por código para no modificar usuarios.Designer.cs.
        private IconButton btnPaginaAnterior;
        private IconButton btnPaginaSiguiente;
        private Label labelPagina;

        // ---------------------------------------------------------------
        // SELECCIÓN (chkSeleccionar): columna de casillas en la tabla.
        // Al marcar la casilla de una fila, ese usuario se carga en "Gestionar".
        // ---------------------------------------------------------------
        private const string ColumnaSeleccion = "colSeleccionar";
        private int idSeleccionado = 0;   // Código del usuario cargado (0 = ninguno)

        // Ayuda que aparece al pasar el mouse sobre el campo Clave.
        private readonly ToolTip ayuda = new ToolTip();

        // Correspondencia entre los controles de "Gestionar" y las columnas de dbo.Tbl_Usuarios
        // (los TextBox conservan el nombre que traían del diseño original):
        //   textCodigoCliente   -> IdUsuario       (solo lectura)
        //   textServicio        -> IdSucursal
        //   textBox1            -> NombreUsuario
        //   textRecargoServicio -> Clave           (se escribe con *, se guarda como antes y no se consulta)
        //   textTotalServios    -> NombreCompleto
        //   textDescripcion     -> Correo
        //   radioActivo / radioInactivo -> Estado

        public usuarios()
        {
            InitializeComponent();
            // Campo "Rol" creado por código (el Designer.cs no se modifica).
            // Su posición y estilo se copian de los controles del diseñador en Load (UbicarCampoRol).
            comboRol.Name = "comboRolUsuario";
            comboRol.DropDownStyle = ComboBoxStyle.DropDownList;
            etiquetaRol.Name = "labelRol";
            etiquetaRol.Text = "Rol";
            etiquetaRol.AutoSize = true;
            tabPage2.Controls.Add(etiquetaRol);
            tabPage2.Controls.Add(comboRol);

            // Los eventos se conectan aquí (y no en el Designer.cs, que no se modifica).
            this.Load += FormularioUsuarios_Load;
            btnNuevo_usuarios.Click += BotonNuevo_Click;
            btnCancelar_usuarios.Click += BotonCancelar_Click;
            btnGuardar_usuarios.Click += BotonGuardar_Click;
            btnEditar_usuarios.Click += BotonEditar_Click;
            btnEliminar_usuarios.Click += BotonEliminar_Click;
            chkSeleccionar.CheckedChanged += Seleccionar_CheckedChanged;
            btnBuscar_usuarios.Click += BotonBuscar_Click;
            btnLimpiar_usuarios.Click += BotonLimpiarBusqueda_Click;
            textBuascar_usuarios.KeyDown += TextoBuscar_KeyDown;
            dataGridViewUsuarios.CellContentClick += Tabla_CellContentClick;
            CrearControlesPaginacion();
        }

        // ===============================================================
        // CARGA DEL FORMULARIO
        // ===============================================================

        private void FormularioUsuarios_Load(object sender, EventArgs e)
        {
            UbicarControlesPaginacion();
            UbicarCampoRol();
            ConfigurarTabla();
            ConfigurarCampos();
            CargarRoles();
            ConsultarUsuarios();
            LimpiarCampos();
        }

        /// <summary>
        /// Coloca "Rol" como una fila más, debajo de "Estado", con la misma separación entre filas
        /// (la que hay entre "Correo" y "Estado"). Se usan los controles del diseñador como referencia
        /// porque Windows ya los ajustó a la escala de la pantalla; así el campo nunca queda descuadrado.
        /// </summary>
        private void UbicarCampoRol()
        {
            int separacionFilas = label5.Top - label9.Top;           // Correo -> Estado
            etiquetaRol.Font = label5.Font;                           // Misma fuente y color que las demás etiquetas.
            etiquetaRol.ForeColor = label5.ForeColor;
            etiquetaRol.Location = new Point(label5.Left, label5.Top + separacionFilas);

            comboRol.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            comboRol.Width = textDescripcion.Width;                   // Mismo ancho que las cajas de texto.
            comboRol.Location = new Point(textDescripcion.Left, etiquetaRol.Top + (textDescripcion.Top - label9.Top));
        }

        private void CargarRoles()
        {
            try
            {
                comboRol.DataSource = negocioRoles.Consultar().FindAll(r => r.Estado);
                comboRol.DisplayMember = "NombreRol";
                comboRol.ValueMember = "IdRol";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Roles", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        /// <summary>La tabla solo muestra datos: el usuario no escribe en ella.</summary>
        private void ConfigurarTabla()
        {
            dataGridViewUsuarios.ReadOnly = true;
            dataGridViewUsuarios.AllowUserToAddRows = false;      // Sin la fila vacía del final.
            dataGridViewUsuarios.AllowUserToDeleteRows = false;
            dataGridViewUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUsuarios.MultiSelect = false;
            dataGridViewUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>Campos de solo lectura, largos máximos y la clave oculta con asteriscos.</summary>
        private void ConfigurarCampos()
        {
            textCodigoCliente.ReadOnly = true;
            textServicio.MaxLength = 9;
            textBox1.MaxLength = 100;
            textRecargoServicio.MaxLength = 50;
            textRecargoServicio.PasswordChar = '*';   // La clave no se ve mientras se escribe.
            textTotalServios.MaxLength = 150;
            textDescripcion.MaxLength = 100;
            textBuascar_usuarios.MaxLength = 100;
            ayuda.SetToolTip(textRecargoServicio,
                "Usuario nuevo: obligatoria (mínimo 6 caracteres).\nAl editar: déjela vacía para conservar la clave actual.");
        }

        // ===============================================================
        // CONSULTAR
        // ===============================================================

        /// <summary>CONSULTAR: pide los usuarios a Negocio, muestra el total y la primera página.</summary>
        private void ConsultarUsuarios()
        {
            try
            {
                listaUsuarios = negocioUsuarios.Consultar();
            }
            catch (Exception ex)
            {
                listaUsuarios = new List<Usuario>();
                MessageBox.Show(ex.Message, "Error al consultar usuarios",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            MostrarTotales();
            paginaActual = 1;
            MostrarPagina();
        }

        /// <summary>Total de TODOS los usuarios; cambiar de página NO lo modifica.</summary>
        private void MostrarTotales()
        {
            labelRegistros.Text = "Registros: " + listaUsuarios.Count;
        }

        // ===============================================================
        // PAGINACIÓN
        // ===============================================================

        private int TotalPaginas()
        {
            int total = (listaUsuarios.Count + RegistrosPorPagina - 1) / RegistrosPorPagina;
            if (total < 1)
            {
                total = 1;
            }
            return total;
        }

        /// <summary>Muestra en la tabla solo los registros de la página actual.</summary>
        private void MostrarPagina()
        {
            int inicio = (paginaActual - 1) * RegistrosPorPagina;
            int cantidad = Math.Min(RegistrosPorPagina, listaUsuarios.Count - inicio);

            List<Usuario> pagina = new List<Usuario>();
            if (cantidad > 0)
            {
                pagina = listaUsuarios.GetRange(inicio, cantidad);
            }

            // DataSource crea una columna por cada propiedad de la clase Usuario (no existe Clave).
            dataGridViewUsuarios.DataSource = pagina;
            ConfigurarColumnas();
            MarcarFilaSeleccionada();

            labelPagina.Text = "Página " + paginaActual + " de " + TotalPaginas();
            btnPaginaAnterior.Enabled = paginaActual > 1;
            btnPaginaSiguiente.Enabled = paginaActual < TotalPaginas();
        }

        /// <summary>Encabezados claros; el código de sucursal se oculta (se ve su nombre).</summary>
        private void ConfigurarColumnas()
        {
            DataGridViewColumnCollection columnas = dataGridViewUsuarios.Columns;
            if (!columnas.Contains("IdUsuario"))
            {
                return;
            }

            columnas["IdUsuario"].HeaderText = "Código";
            columnas["NombreUsuario"].HeaderText = "Usuario";
            columnas["NombreCompleto"].HeaderText = "Nombre completo";
            columnas["Correo"].HeaderText = "Correo";
            columnas["IdSucursal"].Visible = false;
            columnas["NombreSucursal"].HeaderText = "Sucursal";
            if (columnas.Contains("NombreRol")) columnas["NombreRol"].HeaderText = "Rol";
            if (columnas.Contains("IdRol")) columnas["IdRol"].Visible = false;
            columnas["Estado"].HeaderText = "Activo";
        }

        private void btnPaginaAnterior_Click(object sender, EventArgs e)
        {
            if (paginaActual > 1)
            {
                paginaActual--;
                MostrarPagina();
            }
        }

        private void btnPaginaSiguiente_Click(object sender, EventArgs e)
        {
            if (paginaActual < TotalPaginas())
            {
                paginaActual++;
                MostrarPagina();
            }
        }

        // ===============================================================
        // BUSCAR (caja "Nombre" de la pestaña de consulta)
        // ===============================================================

        /// <summary>
        /// BUSCAR: pide a Negocio los usuarios cuyo nombre de usuario o nombre completo contienen el texto.
        /// La búsqueda se hace en la base de datos. El resultado usa la misma paginación
        /// y la etiqueta de totales cuenta los registros encontrados.
        /// </summary>
        private void BuscarUsuarios(bool avisarSiNoHay)
        {
            try
            {
                listaUsuarios = negocioUsuarios.Buscar(textBuascar_usuarios.Text);
            }
            catch (Exception ex)
            {
                listaUsuarios = new List<Usuario>();
                MessageBox.Show(ex.Message, "Error al buscar usuarios",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            MostrarTotales();
            paginaActual = 1;
            MostrarPagina();

            if (avisarSiNoHay && listaUsuarios.Count == 0)
            {
                MessageBox.Show("No se encontraron usuarios con \"" + textBuascar_usuarios.Text.Trim() + "\".",
                    "Buscar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BotonBuscar_Click(object sender, EventArgs e)
        {
            BuscarUsuarios(true);
        }

        /// <summary>Pulsar Enter dentro de la caja de búsqueda equivale a pulsar Buscar.</summary>
        private void TextoBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;   // Evita el sonido de Windows al pulsar Enter.
                BuscarUsuarios(true);
            }
        }

        /// <summary>Limpiar: borra el texto de búsqueda y vuelve a mostrar todos los usuarios.</summary>
        private void BotonLimpiarBusqueda_Click(object sender, EventArgs e)
        {
            textBuascar_usuarios.Clear();
            ConsultarUsuarios();
        }

        /// <summary>
        /// Después de guardar, editar o eliminar: si hay una búsqueda escrita se repite;
        /// si no, se vuelven a consultar todos los usuarios.
        /// </summary>
        private void RefrescarTabla()
        {
            if (textBuascar_usuarios.Text.Trim() != "")
            {
                BuscarUsuarios(false);
            }
            else
            {
                ConsultarUsuarios();
            }
        }

        // ===============================================================
        // SELECCIÓN CON CASILLAS (chkSeleccionar)
        // ===============================================================

        /// <summary>Marcar "Seleccionar" agrega la columna de casillas; desmarcarlo la quita y limpia los campos.</summary>
        private void Seleccionar_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSeleccionar.Checked)
            {
                if (!dataGridViewUsuarios.Columns.Contains(ColumnaSeleccion))
                {
                    DataGridViewCheckBoxColumn columna = new DataGridViewCheckBoxColumn();
                    columna.Name = ColumnaSeleccion;
                    columna.HeaderText = "Sel.";
                    columna.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    columna.Width = 50;
                    dataGridViewUsuarios.Columns.Insert(0, columna);
                }
                MarcarFilaSeleccionada();
            }
            else
            {
                if (dataGridViewUsuarios.Columns.Contains(ColumnaSeleccion))
                {
                    dataGridViewUsuarios.Columns.Remove(ColumnaSeleccion);
                }
                LimpiarCampos();
            }
        }

        /// <summary>Clic en la casilla de una fila: ese usuario se carga en "Gestionar".</summary>
        private void Tabla_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!chkSeleccionar.Checked || e.RowIndex < 0)
            {
                return;
            }
            if (dataGridViewUsuarios.Columns[e.ColumnIndex].Name != ColumnaSeleccion)
            {
                return;
            }

            Usuario usuario = (Usuario)dataGridViewUsuarios.Rows[e.RowIndex].DataBoundItem;
            MostrarEnCampos(usuario);
            MarcarFilaSeleccionada();
            tabControl1.SelectedTab = tabPage2;   // Pestaña "Gestionar".
        }

        /// <summary>Deja marcada solo la casilla del usuario seleccionado.</summary>
        private void MarcarFilaSeleccionada()
        {
            if (!dataGridViewUsuarios.Columns.Contains(ColumnaSeleccion))
            {
                return;
            }
            foreach (DataGridViewRow fila in dataGridViewUsuarios.Rows)
            {
                Usuario usuario = (Usuario)fila.DataBoundItem;
                fila.Cells[ColumnaSeleccion].Value = (usuario.IdUsuario == idSeleccionado);
            }
        }

        /// <summary>Editar y Eliminar necesitan un usuario seleccionado con la casilla.</summary>
        private bool HaySeleccion()
        {
            if (chkSeleccionar.Checked && idSeleccionado > 0)
            {
                return true;
            }
            MessageBox.Show("Para editar o eliminar: en la pestaña de consulta marque 'Seleccionar' " +
                            "y luego la casilla del usuario.", "Usuarios",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }

        // ===============================================================
        // CAMPOS DE "GESTIONAR"
        // ===============================================================

        /// <summary>Deja los campos en blanco y quita la selección.</summary>
        private void LimpiarCampos()
        {
            idSeleccionado = 0;
            textCodigoCliente.Clear();
            textServicio.Clear();
            textBox1.Clear();
            textRecargoServicio.Clear();
            textTotalServios.Clear();
            textDescripcion.Clear();
            radioActivo.Checked = true;   // Un usuario nuevo empieza Activo.
            if (comboRol.Items.Count > 0) comboRol.SelectedIndex = 0;
            MarcarFilaSeleccionada();
        }

        /// <summary>
        /// Convierte lo escrito en los campos a un objeto Usuario (la clave se lee aparte).
        /// Si el ID de sucursal está mal escrito, avisa y devuelve false.
        /// </summary>
        private bool LeerCampos(out Usuario usuario)
        {
            usuario = new Usuario();
            usuario.IdUsuario = idSeleccionado;

            int idSucursal;
            if (!Formato.LeerEntero(textServicio.Text, out idSucursal))
            {
                return Avisar("El ID de la sucursal debe ser un número entero.", textServicio);
            }
            usuario.IdSucursal = idSucursal;

            usuario.NombreUsuario = textBox1.Text;
            usuario.NombreCompleto = textTotalServios.Text;
            usuario.Correo = textDescripcion.Text;
            usuario.Estado = radioActivo.Checked;
            usuario.IdRol = comboRol.SelectedValue == null ? (int?)null : Convert.ToInt32(comboRol.SelectedValue);
            return true;
        }

        /// <summary>
        /// Muestra en los campos los datos de un usuario y lo deja seleccionado.
        /// La clave queda vacía: nunca se lee ni se muestra.
        /// </summary>
        private void MostrarEnCampos(Usuario usuario)
        {
            idSeleccionado = usuario.IdUsuario;
            textCodigoCliente.Text = usuario.IdUsuario.ToString();
            textServicio.Text = usuario.IdSucursal.ToString();
            textBox1.Text = usuario.NombreUsuario;
            textRecargoServicio.Clear();
            textTotalServios.Text = usuario.NombreCompleto;
            textDescripcion.Text = usuario.Correo;
            radioActivo.Checked = usuario.Estado;
            radioInactivo.Checked = !usuario.Estado;
            // Si el usuario todavía no tiene rol, la lista queda sin selección (al guardar se pedirá uno).
            // A una lista conectada a datos NO se le puede asignar SelectedValue = null: lanza ArgumentNullException.
            if (usuario.IdRol.HasValue)
            {
                comboRol.SelectedValue = usuario.IdRol.Value;
            }
            else
            {
                // Se asigna DOS veces a propósito: en una lista conectada a datos, Windows Forms ignora el
                // primer -1 y deja seleccionado el primer rol (Administrador). Así no se asigna un rol por error.
                comboRol.SelectedIndex = -1;
                comboRol.SelectedIndex = -1;
            }
        }

        /// <summary>Después de guardar, editar o eliminar: avisa, vuelve a consultar y limpia los campos.</summary>
        private void TerminarOperacion(string mensaje)
        {
            MessageBox.Show(mensaje, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefrescarTabla();
            LimpiarCampos();
        }

        /// <summary>Muestra un aviso y pone el cursor en el campo indicado. Devuelve false para cortar la lectura.</summary>
        private bool Avisar(string mensaje, Control campo)
        {
            MessageBox.Show(mensaje, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (campo != null)
            {
                campo.Focus();
            }
            return false;
        }

        // ===============================================================
        // BOTONES DE "GESTIONAR"
        // ===============================================================

        private void BotonNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            textServicio.Focus();   // El cursor queda en "ID sucursal".
        }

        private void BotonCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        /// <summary>Guardar: inserta un usuario con el formato de clave que ya utiliza el CRUD.</summary>
        private void BotonGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (idSeleccionado > 0)
                {
                    Avisar("Hay un usuario seleccionado. Use Editar para guardar sus cambios " +
                           "o Nuevo para registrar otro.", null);
                    return;
                }

                Usuario usuario;
                if (!LeerCampos(out usuario))
                {
                    return;
                }

                string resultado = negocioUsuarios.Insertar(usuario, textRecargoServicio.Text);
                if (resultado == NUsuario.Correcto)
                {
                    TerminarOperacion("Usuario guardado con el código " + usuario.IdUsuario + ".");
                }
                else
                {
                    Avisar(resultado, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Editar: guarda los cambios del usuario seleccionado (clave vacía = no se cambia).</summary>
        private void BotonEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!HaySeleccion())
                {
                    return;
                }

                Usuario usuario;
                if (!LeerCampos(out usuario))
                {
                    return;
                }

                string resultado = negocioUsuarios.Actualizar(usuario, textRecargoServicio.Text);
                if (resultado == NUsuario.Correcto)
                {
                    TerminarOperacion("Usuario actualizado correctamente.");
                }
                else
                {
                    Avisar(resultado, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al editar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Eliminar: borra el usuario seleccionado, después de confirmar.</summary>
        private void BotonEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!HaySeleccion())
                {
                    return;
                }

                DialogResult respuesta = MessageBox.Show(
                    "¿Desea eliminar al usuario \"" + textBox1.Text + "\"?\nEsta acción no se puede deshacer.",
                    "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (respuesta != DialogResult.Yes)
                {
                    return;
                }

                string resultado = negocioUsuarios.Eliminar(idSeleccionado);
                if (resultado == NUsuario.Correcto)
                {
                    TerminarOperacion("Usuario eliminado correctamente.");
                }
                else
                {
                    Avisar(resultado, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al eliminar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===============================================================
        // CONTROLES DE PAGINACIÓN (creados por código)
        // ===============================================================

        private void CrearControlesPaginacion()
        {
            Font fuente = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

            btnPaginaAnterior = CrearBotonPagina("btnPaginaAnterior", "Anterior", IconChar.AngleLeft,
                TextImageRelation.ImageBeforeText, new Point(565, 431), fuente);
            btnPaginaAnterior.Click += btnPaginaAnterior_Click;

            btnPaginaSiguiente = CrearBotonPagina("btnPaginaSiguiente", "Siguiente", IconChar.AngleRight,
                TextImageRelation.TextBeforeImage, new Point(837, 431), fuente);
            btnPaginaSiguiente.Click += btnPaginaSiguiente_Click;

            labelPagina = new Label();
            labelPagina.Name = "labelPagina";
            labelPagina.AutoSize = false;
            // Un punto más pequeña que los botones para que "Página 10 de 10" quepa en una sola línea.
            labelPagina.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            labelPagina.TextAlign = ContentAlignment.MiddleCenter;
            labelPagina.ForeColor = Color.FromArgb(95, 108, 128);
            labelPagina.Location = new Point(697, 431);
            labelPagina.Size = new Size(138, 38);

            tabPage1.Controls.Add(btnPaginaAnterior);
            tabPage1.Controls.Add(labelPagina);
            tabPage1.Controls.Add(btnPaginaSiguiente);
        }

        /// <summary>
        /// Ubica los controles de paginación tomando como referencia controles del diseñador,
        /// que Windows ya ajustó a la escala de pantalla (100 %, 125 %, ...): misma fila y alto que
        /// el botón Exportar y alineados al borde derecho de la tabla. Así siempre quedan visibles.
        /// </summary>
        private void UbicarControlesPaginacion()
        {
            float escala = btnExportar_usuarios.Width / 142f;   // 142 = ancho del botón Exportar en el diseño
            int anchoBoton = (int)(130 * escala);
            int anchoEtiqueta = (int)(138 * escala);
            int separacion = (int)(2 * escala);
            int y = btnExportar_usuarios.Top;
            int alto = btnExportar_usuarios.Height;

            btnPaginaSiguiente.SetBounds(dataGridViewUsuarios.Right - anchoBoton, y, anchoBoton, alto);
            labelPagina.SetBounds(btnPaginaSiguiente.Left - separacion - anchoEtiqueta, y, anchoEtiqueta, alto);
            btnPaginaAnterior.SetBounds(labelPagina.Left - separacion - anchoBoton, y, anchoBoton, alto);
        }

        private IconButton CrearBotonPagina(string nombre, string texto, IconChar icono,
            TextImageRelation relacion, Point ubicacion, Font fuente)
        {
            IconButton boton = new IconButton();
            boton.Name = nombre;
            boton.Text = texto;
            boton.IconChar = icono;
            // Estilo "secundario" del proyecto: fondo blanco, borde celeste, texto azul marino e icono azul.
            boton.IconColor = Color.FromArgb(13, 110, 253);
            boton.IconFont = IconFont.Auto;
            boton.IconSize = 18;
            boton.TextImageRelation = relacion;
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderColor = Color.FromArgb(200, 214, 235);
            boton.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 240, 253);
            boton.BackColor = Color.White;
            boton.ForeColor = Color.FromArgb(16, 42, 110);
            boton.Font = fuente;
            boton.Cursor = Cursors.Hand;
            boton.UseVisualStyleBackColor = false;
            boton.Location = ubicacion;
            boton.Size = new Size(130, 38);
            return boton;
        }
    }
}
