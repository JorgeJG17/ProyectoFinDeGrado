using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration; //libreria
using System.Data.SqlClient;
using System.Data;

namespace ProyectoFinDeGrado
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Boton para iniciar
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Iniciar ventanaIniciar = new Iniciar();

            ventanaIniciar.ShowDialog();
        }

        //Cerrar app
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Este programa es creado por Jorge Jiménez Garrido");
        }
    }
}
