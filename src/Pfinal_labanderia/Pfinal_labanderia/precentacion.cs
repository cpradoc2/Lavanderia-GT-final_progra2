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

namespace Pfinal_labanderia
{
    public partial class precentacion : Form
    {
        // Guarda el formulario que se muestra en este momento dentro de panelPrincipal.
        // Sirve para cerrarlo antes de abrir otro y no acumular formularios en memoria.
        private Form formularioActivo = null;

        // Botón "Roles y accesos": se crea por código para no modificar precentacion.Designer.cs.
        private readonly IconButton botonRoles = new IconButton();

        // Botones del menú que el rol puede usar, en el orden en que aparecen (de arriba abajo).
        private readonly List<Control> botonesPermitidos = new List<Control>();
        private int inicioMenu;   // Posición del primer botón en el diseño.
        private int pasoMenu;     // Separación entre botones en el diseño.

        // Tamaño de panelPrincipal con la ventana normal (el "tamaño original" de los formularios).
        private Size tamanoNormal = Size.Empty;

        public precentacion()
        {
            InitializeComponent();
            MostrarBarraSesion();
            CrearBotonRoles();
            ConfigurarAccesos();

            // Al abrir el menú principal (después del login) se muestra la pantalla de Inicio.
            this.Load += MenuPrincipal_Load;

            // Al maximizar o restaurar la ventana se centra el formulario abierto (ver AjustarAreaDeTrabajo).
            this.Resize += Precentacion_Resize;

            // Módulos integrados del proyecto LavanderiasMultinegocio.
            // Se conectan aquí para no modificar precentacion.Designer.cs.
            iconButton3.Click += btnEntregas_Click;      // "Entregas"
            iconButton5.Click += btnProcesos_Click;      // "Procesos"
            iconButton9.Click += btnTiposPrenda_Click;   // "TiposPrenda"
            iconButton12.Click += btnServicios_Click;    // "Servicios"
            iconButton2.Click += btnDetalleRecepciones_Click; // "Detalle recepciones"
            iconButton11.Click += btnSucursales_Click;        // "Sucursales"
        }

        /// <summary>
        /// Barra "Conectado | Rol" arriba del área de trabajo, a lo ancho y una sola vez.
        /// Se agrega al formulario (no a panelPrincipal) para que el margen que se pone al maximizar
        /// no la mueva. El orden en Controls decide cómo se acomodan los controles con Dock:
        /// primero el menú (izquierda), luego la barra (arriba) y al final panelPrincipal (el resto).
        /// </summary>
        private void MostrarBarraSesion()
        {
            SesionActual.MostrarEn(this);
            // Orden final en Controls: panelPrincipal (adelante), barra, panelBotones (atrás).
            // Windows acomoda primero el de atrás: menú a la izquierda, luego la barra arriba y el panel ocupa el resto.
            panelPrincipal.BringToFront();
            panelBotones.SendToBack();
        }

        /// <summary>
        /// Crea el botón "Roles y accesos" con el MISMO estilo que los demás botones del menú:
        /// los colores, la fuente y el tamaño se copian de bnt_usuarios para que sean idénticos.
        /// </summary>
        private void CrearBotonRoles()
        {
            botonRoles.Name = "btnRoles";
            botonRoles.Text = "Roles y accesos";
            botonRoles.IconChar = IconChar.UserShield;
            botonRoles.IconFont = bnt_usuarios.IconFont;
            botonRoles.IconSize = bnt_usuarios.IconSize;
            botonRoles.IconColor = bnt_usuarios.IconColor;
            botonRoles.BackColor = bnt_usuarios.BackColor;
            botonRoles.ForeColor = bnt_usuarios.ForeColor;
            botonRoles.Font = bnt_usuarios.Font;
            botonRoles.FlatStyle = bnt_usuarios.FlatStyle;
            botonRoles.FlatAppearance.BorderSize = bnt_usuarios.FlatAppearance.BorderSize;
            botonRoles.FlatAppearance.MouseOverBackColor = bnt_usuarios.FlatAppearance.MouseOverBackColor;
            botonRoles.FlatAppearance.MouseDownBackColor = bnt_usuarios.FlatAppearance.MouseDownBackColor;
            botonRoles.ImageAlign = bnt_usuarios.ImageAlign;
            botonRoles.TextAlign = bnt_usuarios.TextAlign;
            botonRoles.TextImageRelation = bnt_usuarios.TextImageRelation;
            botonRoles.Padding = bnt_usuarios.Padding;
            botonRoles.Cursor = bnt_usuarios.Cursor;
            botonRoles.UseVisualStyleBackColor = false;
            botonRoles.Click += (s, e) => AbrirFormularioEnPanel(new roles());
            panelBotones.Controls.Add(botonRoles);
            // Si hay muchos botones y la ventana es baja, el menú se puede desplazar.
            panelBotones.AutoScroll = true;
        }

        /// <summary>Muestra solo los botones del menú que el rol del usuario tiene permitidos.</summary>
        private void ConfigurarAccesos()
        {
            string[,] mapa = { { "inicio", "bnt_inicio" }, { "clientes", "btn_clientes" }, { "recepciones", "bnt_recepcion" },
                { "tarifas", "btn_tarifas" }, { "usuarios", "bnt_usuarios" }, { "entregas", "iconButton3" },
                { "procesos", "iconButton5" }, { "tiposprenda", "iconButton9" }, { "servicios", "iconButton12" },
                { "detalleRecepciones", "iconButton2" }, { "estadosProceso", "iconButton4" }, { "sucursales", "iconButton11" },
                { "roles", "btnRoles" } };
            for (int i = 0; i < mapa.GetLength(0); i++)
            {
                bool permitido = SesionActual.Accesos.Contains(mapa[i, 0]);
                foreach (Control control in Controls.Find(mapa[i, 1], true))
                {
                    control.Visible = permitido;
                    if (permitido) botonesPermitidos.Add(control);
                }
            }
        }

        /// <summary>
        /// Se ejecuta en Load, cuando Windows ya ajustó los botones del diseñador a la escala de la pantalla.
        /// "Roles y accesos" va debajo de "Usuarios" y los botones permitidos se ordenan como en el diseño.
        /// </summary>
        private void PrepararMenu()
        {
            pasoMenu = btn_clientes.Top - bnt_inicio.Top;
            inicioMenu = bnt_inicio.Top;
            botonRoles.Size = bnt_usuarios.Size;
            botonRoles.Location = new Point(bnt_usuarios.Left, bnt_usuarios.Top + pasoMenu);
            botonesPermitidos.Sort((a, b) => a.Top.CompareTo(b.Top));
            ReacomodarMenu();
        }

        /// <summary>
        /// Coloca los botones permitidos uno debajo del otro, SIN HUECOS (los ocultos no ocupan lugar).
        /// Si no caben con la separación del diseño, se juntan un poco (mínimo 2 px entre botones);
        /// si aun así no caben, el menú muestra una barra de desplazamiento.
        /// </summary>
        private void ReacomodarMenu()
        {
            if (botonesPermitidos.Count == 0 || pasoMenu == 0) return;
            panelBotones.AutoScrollPosition = new Point(0, 0);   // Se acomoda desde arriba.
            int alto = botonesPermitidos[0].Height;
            int disponible = panelBotones.ClientSize.Height - inicioMenu - 4;
            int paso = Math.Max(alto + 2, Math.Min(pasoMenu, disponible / botonesPermitidos.Count));
            for (int i = 0; i < botonesPermitidos.Count; i++)
            {
                botonesPermitidos[i].Top = inicioMenu + i * paso;
            }
        }

        /// <summary>
        /// MAXIMIZAR / RESTAURAR.
        /// - Ventana normal: el formulario abierto ocupa todo panelPrincipal, igual que siempre,
        ///   y se recuerda ese tamaño como el "tamaño original".
        /// - Ventana maximizada: panelPrincipal recibe un margen (Padding) igual por los cuatro lados.
        ///   Como el formulario usa Dock = Fill, respeta ese margen: conserva su tamaño original y queda
        ///   centrado, sin que su diseño cambie.
        /// - Al restaurar, el margen vuelve a 0 y todo queda exactamente como antes.
        /// </summary>
        private void AjustarAreaDeTrabajo()
        {
            if (WindowState == FormWindowState.Minimized)
            {
                return;   // Minimizada: no se ve nada, así que no hay que acomodar.
            }
            if (WindowState == FormWindowState.Normal)
            {
                panelPrincipal.Padding = Padding.Empty;
                tamanoNormal = panelPrincipal.ClientSize;
                return;
            }
            if (tamanoNormal.IsEmpty)
            {
                return;   // Todavía no se conoce el tamaño original.
            }
            int margenX = Math.Max(0, (panelPrincipal.ClientSize.Width - tamanoNormal.Width) / 2);
            int margenY = Math.Max(0, (panelPrincipal.ClientSize.Height - tamanoNormal.Height) / 2);
            panelPrincipal.Padding = new Padding(margenX, margenY, margenX, margenY);
        }

        private void Precentacion_Resize(object sender, EventArgs e)
        {
            AjustarAreaDeTrabajo();
            ReacomodarMenu();
        }

        private void MenuPrincipal_Load(object sender, EventArgs e)
        {
            PrepararMenu();
            AjustarAreaDeTrabajo();

            if (SesionActual.Accesos.Contains("inicio"))
            {
                AbrirFormularioEnPanel(new inicio());
                return;
            }

            // Si el rol no tiene Inicio, se abre la primera opción permitida para esa cuenta.
            string[] orden = { "clientes", "recepciones", "tarifas", "usuarios", "entregas", "procesos", "tiposprenda", "servicios", "detalleRecepciones", "estadosProceso", "sucursales", "roles" };
            foreach (string acceso in orden)
            {
                if (!SesionActual.Accesos.Contains(acceso)) continue;
                if (acceso == "clientes") AbrirFormularioEnPanel(new clientes());
                else if (acceso == "recepciones" || acceso == "detalleRecepciones") AbrirFormularioEnPanel(new Recepciones());
                else if (acceso == "tarifas") AbrirFormularioEnPanel(new Tarifas());
                else if (acceso == "usuarios") AbrirFormularioEnPanel(new usuarios());
                else if (acceso == "entregas") AbrirFormularioEnPanel(new Entregas());
                else if (acceso == "procesos" || acceso == "estadosProceso") AbrirFormularioEnPanel(new Procesos());
                else if (acceso == "tiposprenda") AbrirFormularioEnPanel(new TiposPrenda());
                else if (acceso == "servicios") AbrirFormularioEnPanel(new Servicios());
                else if (acceso == "sucursales") AbrirFormularioEnPanel(new inicio());
                else if (acceso == "roles") AbrirFormularioEnPanel(new roles());
                return;
            }
            MessageBox.Show("Su rol no tiene accesos configurados. Solicite ayuda a un administrador.", "Sin accesos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Muestra un formulario DENTRO de panelPrincipal, como si fuera un control más.
        /// Se puede reutilizar para todos los botones del menú.
        /// </summary>
        private void AbrirFormularioEnPanel(Form formularioHijo)
        {
            try
            {
                // Si ya está abierto este formulario, conserva la instancia actual.
                // Se libera la instancia recién creada y se evita mostrar una copia.
                if (formularioActivo != null && formularioActivo.GetType() == formularioHijo.GetType())
                {
                    formularioHijo.Dispose();
                    formularioActivo.BringToFront();
                    return;
                }

                // 1. El formulario que se mostraba antes desaparece del panel.
                CerrarFormularioActivo();
                formularioActivo = formularioHijo;

                // 2. TopLevel = false permite colocar un Form dentro de otro control.
                formularioHijo.TopLevel = false;

                // 3. Sin bordes ni barra de título, para que se vea integrado al panel.
                formularioHijo.FormBorderStyle = FormBorderStyle.None;

                // 4. Ocupa todo el espacio disponible del panel.
                //    (Al maximizar, ese espacio es el tamaño original centrado: ver AjustarAreaDeTrabajo.)
                formularioHijo.Dock = DockStyle.Fill;

                // 5. Se agrega al panel, se coloca al frente y se muestra.
                //    La barra "Conectado | Rol" ya está una sola vez arriba del área de trabajo;
                //    no se agrega otra dentro del formulario porque taparía su título.
                panelPrincipal.Controls.Add(formularioHijo);
                formularioHijo.BringToFront();
                formularioHijo.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo abrir el formulario: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Quita del panel el formulario que se estaba mostrando y lo libera de memoria.
        /// Así, al pulsar otro botón, el anterior desaparece antes de que aparezca el nuevo.
        /// </summary>
        private void CerrarFormularioActivo()
        {
            if (formularioActivo == null)
            {
                return; // Todavía no se ha abierto ningún formulario.
            }

            panelPrincipal.Controls.Remove(formularioActivo); // a) deja de verse en el panel
            formularioActivo.Close();                          // b) se cierra (FormClosing / FormClosed)
            formularioActivo.Dispose();                        // c) libera la memoria que usaba
            formularioActivo = null;
        }

        // ---------------------------------------------------------------
        // Botones del menú lateral.
        // Cada evento Click indica qué formulario existente se abre en panelPrincipal.
        // ---------------------------------------------------------------

        /// <summary>Botón "Inicio" (bnt_inicio): vuelve a mostrar la pantalla de Inicio con datos actualizados.</summary>
        private void bnt_inicio_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new inicio());
        }

        /// <summary>Botón "Clientes" (btn_clientes): abre el formulario clientes.</summary>
        private void btn_clientes_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new clientes());
        }

        /// <summary>Botón "Recepciones" (bnt_recepcion): abre el formulario Recepciones.</summary>
        private void bnt_recepcion_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new Recepciones());
        }

        /// <summary>Botón "Tarifas" (btn_tarifas): abre el formulario Tarifas.</summary>
        private void btn_tarifas_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new Tarifas());
        }

        /// <summary>Botón "Usuarios" (bnt_usuarios): abre el formulario usuarios.</summary>
        private void bnt_usuarios_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new usuarios());
        }

        /// <summary>Botón "Entregas" (iconButton3): abre el formulario Entregas.</summary>
        private void btnEntregas_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new Entregas());
        }

        /// <summary>Botón "Procesos" (iconButton5): abre el formulario Procesos.</summary>
        private void btnProcesos_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new Procesos());
        }

        /// <summary>Botón "TiposPrenda" (iconButton9): abre el formulario TiposPrenda.</summary>
        private void btnTiposPrenda_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new TiposPrenda());
        }

        /// <summary>Botón "Servicios" (iconButton12): abre el formulario Servicios.</summary>
        private void btnServicios_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new Servicios());
        }

        /// <summary>Abre el listado existente de Recepciones para consultar sus registros.</summary>
        private void btnDetalleRecepciones_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new Recepciones());
        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new Procesos());
        }

        /// <summary>Abre Inicio, donde se muestran indicadores y una gráfica por sucursal.</summary>
        private void btnSucursales_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new inicio());
        }

        private void panelBotones_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
