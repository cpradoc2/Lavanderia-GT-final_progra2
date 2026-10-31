using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Negocio;

namespace Pfinal_labanderia
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // PRUEBA INICIAL DE CONEXIÓN (documento 02).
            // La Presentación NO se conecta a SQL Server: le pide la prueba a Negocio,
            // Negocio se la pide a Datos y solo Datos abre la conexión.
            string mensaje;
            bool conectado = NConexion.ProbarConexion(out mensaje);
            MessageBox.Show(mensaje,
                conectado ? "Prueba de conexión" : "Error de conexión",
                MessageBoxButtons.OK,
                conectado ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            Application.Run(new logoing());
        }
    }
}
