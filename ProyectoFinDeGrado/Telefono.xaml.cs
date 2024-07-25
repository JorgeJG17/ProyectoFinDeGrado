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
using System.Windows.Shapes;

namespace ProyectoFinDeGrado
{
    /// <summary>
    /// Lógica de interacción para Telefono.xaml
    /// </summary>
    public partial class Telefono : Window
    {
        public Telefono()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            telefono = cuadroTelefono.Text;

            Close();
        }

        public string devolverTelefono()
        {

            return telefono;

        }

        private String telefono;
    }
}
