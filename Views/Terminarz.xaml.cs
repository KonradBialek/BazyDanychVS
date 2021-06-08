using System;
using System.Collections.Generic;
using System.Data;
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

namespace AplikacjaDostepowa.Views
{
    /// <summary>
    /// Logika interakcji dla klasy Terminarz.xaml
    /// </summary>
    public partial class Terminarz : UserControl
    {
        public Terminarz()
        {
            DataContext = this;
            InitializeComponent();
        }
        public DataTable Data => BazaDanych.GetTable("SELECT * FROM terminarz");
    }
}
