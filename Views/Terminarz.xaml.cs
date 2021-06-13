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
    /// Klasa <c>Terminarz</c> zawiera metody pozwalające na odczyt terminarza.
    /// </summary>
    public partial class Terminarz : UserControl
    {
        /// <summary>
        /// Konstruktor klasy Terminarz.
        /// </summary>
        public Terminarz()
        {
            DataContext = this;
            InitializeComponent();
        }
        /// <summary>
        /// Pobór tabeli dotyczącej terminarza z bazy danych.
        /// </summary>
        public DataTable Data => BazaDanych.GetTable("SELECT * FROM terminarz");
    }
}
