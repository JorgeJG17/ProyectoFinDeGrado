using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Configuration;

namespace ProyectoFinDeGrado
{
    public partial class LibrosElegir : Window
    {
        private SQLiteConnection miConexionSql;
        private int idLibroSeleccionado;

        public LibrosElegir(string usuario)
        {
            InitializeComponent();
            string cadena = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;
            miConexionSql = new SQLiteConnection(cadena); // Cambiar a tu ruta y nombre de la base de datos SQLite.

            string consulta = "SELECT l.*, l.titulo || ' ' || l.autor || ' ' || l.estado || ' ' || l.numeroPag || ' ' || c.usuario AS InfoCompleta FROM LIBROS l INNER JOIN CUENTAS c ON l.uUsuario = c.Id WHERE c.usuario = @Usuario";

            SQLiteCommand miSqlCommand = new SQLiteCommand(consulta, miConexionSql);
            miConexionSql.Open();

            miSqlCommand.Parameters.AddWithValue("@Usuario", usuario);

            DataTable resultado = new DataTable();
            SQLiteDataAdapter miAdaptadorSql = new SQLiteDataAdapter(miSqlCommand);
            miAdaptadorSql.Fill(resultado);

            listaLibros.DisplayMemberPath = "InfoCompleta";
            listaLibros.SelectedValuePath = "Id";
            listaLibros.ItemsSource = resultado.DefaultView;

            miConexionSql.Close();
        }

        // Quiere ese libro
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string consulta = "SELECT Id FROM LIBROS WHERE Id = @Libro";
            SQLiteCommand miSqlCommand = new SQLiteCommand(consulta, miConexionSql);
            miConexionSql.Open();

            miSqlCommand.Parameters.AddWithValue("@Libro", listaLibros.SelectedValue);

            DataTable resultado = new DataTable();
            SQLiteDataAdapter miAdaptadorSql = new SQLiteDataAdapter(miSqlCommand);
            miAdaptadorSql.Fill(resultado);

            idLibroSeleccionado = Convert.ToInt32(resultado.Rows[0]["Id"]);

            miConexionSql.Close();

            MessageBox.Show("Se ha seleccionado correctamente.");

            Close();
        }

        public int ObtenerIdLibroSeleccionado()
        {
            return idLibroSeleccionado;
        }
    }
}

