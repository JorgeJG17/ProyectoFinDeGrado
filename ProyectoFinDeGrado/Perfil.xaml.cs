using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Configuration;

namespace ProyectoFinDeGrado
{
    public partial class Perfil : Window
    {
        private SQLiteConnection miConexionSql;
        private int usuario;

        public Perfil(string nombreUsuario)
        {
            InitializeComponent();

            string cadena = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;
            miConexionSql = new SQLiteConnection(cadena); // Cambiar a tu ruta y nombre de la base de datos SQLite.

            cuadroNombreUsuario.Text = nombreUsuario;

            string consulta = "SELECT Id FROM CUENTAS WHERE usuario = @Usuario";
            SQLiteCommand miSqlCommand = new SQLiteCommand(consulta, miConexionSql);
            miConexionSql.Open();

            miSqlCommand.Parameters.AddWithValue("@Usuario", nombreUsuario);

            DataTable IdUsuario = new DataTable();
            SQLiteDataAdapter miAdaptadorSql = new SQLiteDataAdapter(miSqlCommand);
            miAdaptadorSql.Fill(IdUsuario);

            usuario = Convert.ToInt32(IdUsuario.Rows[0]["Id"]);

            miConexionSql.Close();

            VerListas();
        }

        // Añadir libro
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string consulta = "SELECT * FROM LIBROS WHERE titulo = @titulo AND autor = @autor AND uUsuario = @Uusuario";

            SQLiteCommand miSqlCommand = new SQLiteCommand(consulta, miConexionSql);
            miConexionSql.Open();

            miSqlCommand.Parameters.AddWithValue("@titulo", cuadroTitulo.Text);
            miSqlCommand.Parameters.AddWithValue("@autor", cuadroAutor.Text);
            miSqlCommand.Parameters.AddWithValue("@Uusuario", usuario);

            DataTable numeroCoincidencia = new DataTable();
            SQLiteDataAdapter miAdaptadorSql = new SQLiteDataAdapter(miSqlCommand);
            miAdaptadorSql.Fill(numeroCoincidencia);

            if (numeroCoincidencia.Rows.Count == 0)
            {
                string consulta2 = "INSERT INTO LIBROS (titulo, autor, numeroPag, estado, uUsuario) VALUES (@Titulo, @Autor, @Numeropag, @Estado, @Uusuario)";

                SQLiteCommand miSqlCommand2 = new SQLiteCommand(consulta2, miConexionSql);

                miSqlCommand2.Parameters.AddWithValue("@Titulo", cuadroTitulo.Text);
                miSqlCommand2.Parameters.AddWithValue("@Autor", cuadroAutor.Text);
                miSqlCommand2.Parameters.AddWithValue("@Numeropag", cuadroPaginas.Text);
                miSqlCommand2.Parameters.AddWithValue("@Estado", estado);
                miSqlCommand2.Parameters.AddWithValue("@Uusuario", usuario);

                miSqlCommand2.ExecuteNonQuery();

                miConexionSql.Close();

                VerListas();
            }
            else
            {
                MessageBox.Show("Ya existe un libro igual en su cuenta");
                miConexionSql.Close();
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            estado = "Leido";
        }

        private void RadioButton_Click_1(object sender, RoutedEventArgs e)
        {
            estado = "Leyendo";
        }

        private void RadioButton_Click_2(object sender, RoutedEventArgs e)
        {
            estado = "Sin leer";
        }

        private void VerListas() // Ver la lista de libros que tiene el usuario agregado
        {
            string consulta = "SELECT *, titulo || ' ' || autor || ' ' || estado AS InformacionLibro FROM LIBROS WHERE uUsuario = @Uusuario";

            SQLiteCommand miSqlCommand = new SQLiteCommand(consulta, miConexionSql);

            miConexionSql.Open();
            miSqlCommand.Parameters.AddWithValue("@Uusuario", usuario);

            DataTable resultados = new DataTable();
            SQLiteDataAdapter miAdaptadorSql = new SQLiteDataAdapter(miSqlCommand);
            miAdaptadorSql.Fill(resultados);

            listaLibros.DisplayMemberPath = "InformacionLibro";
            listaLibros.SelectedValuePath = "Id";
            listaLibros.ItemsSource = resultados.DefaultView;

            miConexionSql.Close();
        }

        // Borrar Libro
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string consulta = "DELETE FROM LIBROS WHERE Id = @LIBROID";

            SQLiteCommand miSqlCommand = new SQLiteCommand(consulta, miConexionSql);

            miConexionSql.Open();
            miSqlCommand.Parameters.AddWithValue("@LIBROID", listaLibros.SelectedValue);

            miSqlCommand.ExecuteNonQuery();

            miConexionSql.Close();

            VerListas();
        }

        // Boton Buscar
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            BuscarLibros ventanaBuscar = new BuscarLibros(cuadroTitulo2.Text, cuadroAutor2.Text, usuario);
            ventanaBuscar.ShowDialog();
        }

        // Solicitudes
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Solicitudes ventanaSolicitar = new Solicitudes(usuario);
            ventanaSolicitar.ShowDialog();
        }

        private string estado;
    }
}

