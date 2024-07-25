using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Configuration;

namespace ProyectoFinDeGrado
{
    public partial class Iniciar : Window
    {
        private SQLiteConnection miConexionSql;

        public Iniciar()
        {
            InitializeComponent();
            
            string cadena = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

            miConexionSql = new SQLiteConnection(cadena); // Cambiar a tu ruta y nombre de la base de datos SQLite.
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Registro ventanaRegistro = new Registro();
            ventanaRegistro.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string consulta = "SELECT Id FROM CUENTAS WHERE usuario = @Usuario";
            SQLiteCommand miSqlCommand = new SQLiteCommand(consulta, miConexionSql);

            miConexionSql.Open();
            miSqlCommand.Parameters.AddWithValue("@Usuario", cuadroUsuario.Text);

            DataTable numeroUsuario = new DataTable();
            SQLiteDataAdapter miAdaptadorSql = new SQLiteDataAdapter(miSqlCommand);
            miAdaptadorSql.Fill(numeroUsuario);

            if (numeroUsuario.Rows.Count == 1)
            {
                int idUsuario = Convert.ToInt32(numeroUsuario.Rows[0]["Id"]);

                string consulta2 = "SELECT Id FROM CUENTAS WHERE contrasena = @Contrasena";
                SQLiteCommand miSqlCommand2 = new SQLiteCommand(consulta2, miConexionSql);

                miSqlCommand2.Parameters.AddWithValue("@Contrasena", cuadroContrasena.Text);

                DataTable numeroContrasena = new DataTable();
                SQLiteDataAdapter miAdaptadorSql2 = new SQLiteDataAdapter(miSqlCommand2);
                miAdaptadorSql2.Fill(numeroContrasena);

                if (numeroContrasena.Rows.Count > 0 && idUsuario == Convert.ToInt32(numeroContrasena.Rows[0]["Id"]))
                {
                    MessageBox.Show("Sesión iniciada");

                    Perfil ventanaPerfil = new Perfil(cuadroUsuario.Text);
                    ventanaPerfil.ShowDialog();
                }
                else
                {
                    MessageBox.Show("El usuario o la contraseña no coinciden");
                }

            }
            else
            {
                MessageBox.Show("El usuario o la contraseña no coinciden");
            }

            miConexionSql.Close();
        }
    }
}

