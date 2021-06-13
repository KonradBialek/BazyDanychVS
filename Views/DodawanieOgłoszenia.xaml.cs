using MySql.Data.MySqlClient;
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

namespace AplikacjaDostepowa.Views
{
    /// <summary>
    /// Klasa <c>DodawanieOgłoszenia</c> zawiera metodę pozwalającą na dodawanie ogłoszenia.
    /// </summary>
    public partial class DodawanieOgłoszenia : UserControl
    {
        /// <summary>
        /// Konstruktor klasy DodawanieOceny, pobiera tabelę uczniów i przedmiotów.
        /// </summary>
        public DodawanieOgłoszenia()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Dodanie ogłoszenia po wciśnięciu przycisku.
        /// </summary>
        /// <param name="sender">Źródło</param>
        /// <param name="e">Dodatkowe argumenty</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BazaDanych.Execute("INSERT INTO informacje (tresc) VALUES (@tresc)",
                new MySqlParameter("tresc", Opis.Text)
                );
            MessageBox.Show("Dodano ogłoszenie.");
            ((MainWindow)Application.Current.MainWindow).DataContext = new Ogłoszenia();
        }
    }
}
