using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;
using System.Configuration;

namespace ProyectoFinDeGrado
{
    public partial class BuscarLibros : Window
    {
        private int idUsuario;

        public BuscarLibros(string titulo, string autor, int idUsuario)
        {
            InitializeComponent();

            string cadena = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;
            string cadenaConexion = cadena; // Reemplazar con la ruta y nombre del archivo SQLite

            using (SQLiteConnection miConexionSqLite = new SQLiteConnection(cadenaConexion))
            {
                miConexionSqLite.Open();

                string consulta = "SELECT l.*, l.titulo || ' ' || l.autor || ' ' || l.estado || ' ' || l.numeroPag || ' ' || c.usuario AS InfoCompleta FROM LIBROS l INNER JOIN CUENTAS c ON l.uUsuario = c.Id WHERE l.titulo = @Titulo AND l.autor = @Autor";

                using (SQLiteCommand miSqlCommand = new SQLiteCommand(consulta, miConexionSqLite))
                {
                    miSqlCommand.Parameters.AddWithValue("@Titulo", titulo);
                    miSqlCommand.Parameters.AddWithValue("@Autor", autor);

                    using (SQLiteDataReader resultado = miSqlCommand.ExecuteReader())
                    {
                        if (resultado.HasRows)
                        {
                            listaResultados.DisplayMemberPath = "InfoCompleta";
                            listaResultados.SelectedValuePath = "Id";

                            List<Dictionary<string, object>> listaLibros = new List<Dictionary<string, object>>();
                            while (resultado.Read())
                            {
                                Dictionary<string, object> libro = new Dictionary<string, object>();
                                for (int i = 0; i < resultado.FieldCount; i++)
                                {
                                    libro.Add(resultado.GetName(i), resultado.GetValue(i));
                                }
                                listaLibros.Add(libro);
                            }
                            listaResultados.ItemsSource = listaLibros;
                        }
                        else
                        {
                            MessageBox.Show("No se han encontrado resultados.");
                        }
                    }
                }
            }

            this.idUsuario = idUsuario;
        }

        // Volver a Buscar
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Solicitar Intercambio
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string cadenaConexion = "Data Source=miBaseDeDatos.db"; // Reemplazar con la ruta y nombre del archivo SQLite

            using (SQLiteConnection miConexionSqLite = new SQLiteConnection(cadenaConexion))
            {
                miConexionSqLite.Open();

                string consulta = "INSERT INTO INTERCAMBIOS (lLibro, uUsuarioS) VALUES (@IdLibro, @IdUsuarioSolicita)";

                using (SQLiteCommand miSqlCommand = new SQLiteCommand(consulta, miConexionSqLite))
                {
                    miSqlCommand.Parameters.AddWithValue("@IdLibro", listaResultados.SelectedValue);
                    miSqlCommand.Parameters.AddWithValue("@IdUsuarioSolicita", idUsuario);

                    miSqlCommand.ExecuteNonQuery();
                }

                MessageBox.Show("Solicitud enviada correctamente");
            }

            Close();
        }
    }
}
