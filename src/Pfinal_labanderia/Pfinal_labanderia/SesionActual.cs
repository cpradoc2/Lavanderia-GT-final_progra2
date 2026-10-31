using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Entidad;

namespace Pfinal_labanderia
{
    /// <summary>Conserva los datos básicos de la sesión mientras están abiertos los formularios.</summary>
    internal static class SesionActual
    {
        public static Usuario Usuario { get; set; }
        public static List<string> Accesos { get; set; } = new List<string>();

        public static void MostrarEn(Control formulario)
        {
            if (Usuario == null) return;
            if (formulario.Controls.ContainsKey("lblSesionActual")) return;
            Label etiqueta = new Label
            {
                Name = "lblSesionActual",
                Text = "Conectado: " + Usuario.NombreCompleto + "  |  Rol: " + Usuario.NombreRol,
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleRight,
                Padding = new Padding(8, 0, 16, 0),
                // Colores y fuente de la paleta del proyecto (celeste con texto azul marino, Segoe UI).
                Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point),
                BackColor = Color.FromArgb(232, 240, 253),
                ForeColor = Color.FromArgb(16, 42, 110)
            };
            formulario.Controls.Add(etiqueta);
            etiqueta.BringToFront();
        }
    }
}
