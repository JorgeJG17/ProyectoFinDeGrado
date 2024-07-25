using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;

namespace ProyectoFinDeGrado
{
    public partial class Registro : Window
    {
        private SQLiteConnection miConexionSql; // Cambio de SqlConnection a SQLiteConnection

        public Registro()
        {
            InitializeComponent();

            string cadena = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

            // Cadena de conexión con la base de datos SQLite.
            miConexionSql = new SQLiteConnection(cadena); // Cambiar a tu ruta y nombre de la base de datos SQLite.
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string consulta = "SELECT * FROM CUENTAS WHERE correo = @Correo";
            SQLiteCommand miSqlCommand = new SQLiteCommand(consulta, miConexionSql);

            miConexionSql.Open();

            miSqlCommand.Parameters.AddWithValue("@Correo", cuadroCorreo.Text);

            DataTable numeroCorreos = new DataTable();
            SQLiteDataAdapter miAdaptadorSql = new SQLiteDataAdapter(miSqlCommand);
            miAdaptadorSql.Fill(numeroCorreos);

            if (numeroCorreos.Rows.Count > 0)
            {
                MessageBox.Show("Ya existe una cuenta con ese correo. Por favor, utiliza otro correo o inicia sesión.");
                miConexionSql.Close();
            }
            else
            {
                string consulta2 = "SELECT * FROM CUENTAS WHERE usuario = @Usuario";
                SQLiteCommand miSqlCommand2 = new SQLiteCommand(consulta2, miConexionSql);

                miSqlCommand2.Parameters.AddWithValue("@Usuario", cuadroUsuario.Text);

                DataTable numeroUsuarios = new DataTable();
                SQLiteDataAdapter miAdaptadorSql2 = new SQLiteDataAdapter(miSqlCommand2);
                miAdaptadorSql2.Fill(numeroUsuarios);

                if (numeroUsuarios.Rows.Count > 0)
                {
                    MessageBox.Show("Ya existe una cuenta con ese nombre de usuario. Por favor, elige otro nombre de usuario o inicia sesión.");
                    miConexionSql.Close();
                }
                else
                {
                    if (cuadroContrasena.Text == cuadroContrasenaRep.Text)
                    {
                        string consulta3 = "INSERT INTO CUENTAS (correo, usuario, contrasena) VALUES (@Correo, @Usuario, @Contrasena)";
                        SQLiteCommand miSqlCommand3 = new SQLiteCommand(consulta3, miConexionSql);

                        miSqlCommand3.Parameters.AddWithValue("@Correo", cuadroCorreo.Text);
                        miSqlCommand3.Parameters.AddWithValue("@Usuario", cuadroUsuario.Text);
                        miSqlCommand3.Parameters.AddWithValue("@Contrasena", cuadroContrasena.Text);

                        miSqlCommand3.ExecuteNonQuery();

                        miConexionSql.Close();

                        cuadroCorreo.Text = "";
                        cuadroUsuario.Text = "";
                        cuadroContrasena.Text = "";
                        cuadroContrasenaRep.Text = "";

                        MessageBox.Show("¡Registro completado correctamente!");
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Las contraseñas no coinciden.");
                        miConexionSql.Close();
                    }
                }
            }
        }
    }
}
