using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Configuration;

namespace ProyectoFinDeGrado
{
    public partial class Solicitudes : Window
    {
        private SQLiteConnection miConexionSql;
        private int idUsuario;
        private int idLibro = -1;
        private string telefono;

        public Solicitudes(int usuario)
        {
            InitializeComponent();

            idUsuario = usuario;

            string cadena = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;
            miConexionSql = new SQLiteConnection(cadena); // Cambiar a tu ruta y nombre de la base de datos SQLite.

            string consulta = "SELECT *, l.titulo || ' ' || l.autor || ' ' || c.usuario AS InfoCom FROM INTERCAMBIOS i INNER JOIN LIBROS l ON i.lLibro = l.Id INNER JOIN CUENTAS c ON l.uUsuario = c.Id WHERE i.uUsuarioS = @Idusuario";
            string consulta2 = "SELECT *, l.titulo || ' ' || l.autor || ' ' || c.usuario AS InfoCom FROM INTERCAMBIOS i INNER JOIN LIBROS l ON i.lLibro = l.Id INNER JOIN CUENTAS c ON i.uUsuarioS = c.Id WHERE l.uUsuario = @Idusuario AND l.Id = i.lLibro";

            SQLiteCommand miSqlCommand = new SQLiteCommand(consulta, miConexionSql);
            SQLiteCommand miSqlCommand2 = new SQLiteCommand(consulta2, miConexionSql);

            miConexionSql.Open();

            miSqlCommand.Parameters.AddWithValue("@Idusuario", usuario);
            miSqlCommand2.Parameters.AddWithValue("@Idusuario", usuario);

            SQLiteDataAdapter miAdaptadorSql = new SQLiteDataAdapter(miSqlCommand);
            SQLiteDataAdapter miAdaptadorSql2 = new SQLiteDataAdapter(miSqlCommand2);

            DataTable enviado = new DataTable();
            miAdaptadorSql.Fill(enviado);

            DataTable recibido = new DataTable();
            miAdaptadorSql2.Fill(recibido);

            if (enviado.Rows.Count > 0 || recibido.Rows.Count > 0)
            {
                if (enviado.Rows.Count > 0)
                {
                    listaEnviados.DisplayMemberPath = "InfoCom";
                    listaEnviados.SelectedValuePath = "Id";
                    listaEnviados.ItemsSource = enviado.DefaultView;
                }

                if (recibido.Rows.Count > 0)
                {
                    listaRecibido.DisplayMemberPath = "InfoCom";
                    listaRecibido.SelectedValuePath = "Id";
                    listaRecibido.ItemsSource = recibido.DefaultView;
                }

                miConexionSql.Close();
            }
            else
            {
                MessageBox.Show("No se han encontrado resultados.");
                miConexionSql.Close();
            }
        }

        // Rechazar
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string consulta = "UPDATE INTERCAMBIOS SET estado = @Estado, libroCambiar = @IdLibro WHERE Id = @IdElemento";

            SQLiteCommand miSqlCommand = new SQLiteCommand(consulta, miConexionSql);

            miConexionSql.Open();

            miSqlCommand.Parameters.AddWithValue("@IdElemento", listaRecibido.SelectedValue);
            miSqlCommand.Parameters.AddWithValue("@Estado", "rechazado");
            miSqlCommand.Parameters.AddWithValue("@IdLibro", idLibro);

            miSqlCommand.ExecuteNonQuery();

            miConexionSql.Close();

            MessageBox.Show("Se ha rechazado correctamente");
        }

        // Aceptar
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (idLibro >= 0)
            {
                string consulta = "UPDATE INTERCAMBIOS SET estado = @Estado, libroCambiar = @IdLibro WHERE Id = @IdElemento";

                SQLiteCommand miSqlCommand = new SQLiteCommand(consulta, miConexionSql);

                miConexionSql.Open();

                miSqlCommand.Parameters.AddWithValue("@IdElemento", listaRecibido.SelectedValue);
                miSqlCommand.Parameters.AddWithValue("@Estado", "aceptado");
                miSqlCommand.Parameters.AddWithValue("@IdLibro", idLibro);

                miSqlCommand.ExecuteNonQuery();

                string consulta2 = "UPDATE CUENTAS SET telefono = @Telefono WHERE Id = @Usuario";

                SQLiteCommand miSqlCommand2 = new SQLiteCommand(consulta2, miConexionSql);

                miSqlCommand2.Parameters.AddWithValue("@Usuario", idUsuario);

                Telefono ventanaTelefono = new Telefono();
                ventanaTelefono.ShowDialog();

                telefono = ventanaTelefono.devolverTelefono();

                miSqlCommand2.Parameters.AddWithValue("@Telefono", telefono);

                miSqlCommand2.ExecuteNonQuery();

                miConexionSql.Close();

                MessageBox.Show("Se ha aceptado correctamente");
            }
            else
            {
                MessageBox.Show("No has seleccionado ningún libro del usuario para intercambiar, ve a la opción ver sus libros primero.");
            }
        }

        // Ver su estado
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string consulta = "SELECT estado, libroCambiar FROM INTERCAMBIOS WHERE Id = @IdElemento";
            string consulta2 = "SELECT c.telefono FROM INTERCAMBIOS i INNER JOIN LIBROS l ON i.lLibro = l.Id INNER JOIN CUENTAS c ON l.uUsuario = c.Id WHERE i.Id = @IdSelect";

            SQLiteCommand miSqlCommand = new SQLiteCommand(consulta, miConexionSql);
            SQLiteCommand miSqlCommand2 = new SQLiteCommand(consulta2, miConexionSql);

            miConexionSql.Open();

            miSqlCommand.Parameters.AddWithValue("@IdElemento", listaEnviados.SelectedValue);
            miSqlCommand2.Parameters.AddWithValue("@IdSelect", listaEnviados.SelectedValue);

            SQLiteDataAdapter miAdaptadorSql = new SQLiteDataAdapter(miSqlCommand);
            SQLiteDataAdapter miAdaptadorSql2 = new SQLiteDataAdapter(miSqlCommand2);

            DataTable estadoRespuesta = new DataTable();
            miAdaptadorSql.Fill(estadoRespuesta);

            DataTable telefonoRespuesta = new DataTable();
            miAdaptadorSql2.Fill(telefonoRespuesta);

            if (estadoRespuesta.Rows.Count > 0)
            {
                string estado = estadoRespuesta.Rows[0]["estado"].ToString();
                int libro = Convert.ToInt32(estadoRespuesta.Rows[0]["libroCambiar"]);

                string consulta3 = "SELECT titulo FROM LIBROS WHERE Id = @IdLibro";
                SQLiteCommand miSqlCommand3 = new SQLiteCommand(consulta3, miConexionSql);
                miSqlCommand3.Parameters.AddWithValue("@IdLibro", libro);

                SQLiteDataAdapter miAdaptadorSql3 = new SQLiteDataAdapter(miSqlCommand3);

                DataTable libroRespuesta = new DataTable();
                miAdaptadorSql3.Fill(libroRespuesta);

                if (libro > -1)
                {
                    string libroNombre = libroRespuesta.Rows[0]["titulo"].ToString();
                    MessageBox.Show("El estado de esta solicitud es: " + estado + " y te lo quiere cambiar por el libro: " + libroNombre);
                }
                else
                {
                    MessageBox.Show("El estado de esta solicitud es: " + estado);
                }

                if (estado == "aceptado")
                {
                    string telefono = telefonoRespuesta.Rows[0]["telefono"].ToString();
                    MessageBox.Show("Puedes hablarle para terminar con el cambio, su número de teléfono es: " + telefono);
                }

                miConexionSql.Close();
            }
            else
            {
                MessageBox.Show("No se han encontrado respuestas aún.");
                miConexionSql.Close();
            }
        }

        // Ver sus libros
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string consulta = "SELECT c.usuario FROM INTERCAMBIOS i INNER JOIN CUENTAS c ON i.uUsuarioS = c.Id WHERE i.Id = @IdUsuario";

            SQLiteCommand miSqlCommand = new SQLiteCommand(consulta, miConexionSql);

            miConexionSql.Open();

            miSqlCommand.Parameters.AddWithValue("@IdUsuario", listaRecibido.SelectedValue);

            SQLiteDataAdapter miAdaptadorSql = new SQLiteDataAdapter(miSqlCommand);

            DataTable usuarioRespuesta = new DataTable();
            miAdaptadorSql.Fill(usuarioRespuesta);

            string usuario = usuarioRespuesta.Rows[0]["usuario"].ToString();

            LibrosElegir ventanaLibros = new LibrosElegir(usuario);

            ventanaLibros.ShowDialog();
            idLibro = ventanaLibros.ObtenerIdLibroSeleccionado();

            miConexionSql.Close();
        }
    }
}

