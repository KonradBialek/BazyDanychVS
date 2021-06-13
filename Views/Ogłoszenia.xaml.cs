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
    /// Klasa <c>Ogłoszenia</c> zawiera metody pozwalające na odczyt ogłoszeń.
    /// </summary>
    public partial class Ogłoszenia : UserControl
    {
        /// <summary>
        /// Konstruktor klasy Ogłoszenia, ustawia widoczność przycisku dodania ogłoszenia.
        /// </summary>
        public Ogłoszenia()
        {
            DataContext = this;
            InitializeComponent();
            var user = ((MainWindow)Application.Current.MainWindow).LoggedUser;
            DodajButton.Visibility = user.Typ == UserType.Nauczyciel ? Visibility.Visible : Visibility.Collapsed;
        }
        /// <summary>
        /// Pobór tabeli dotyczącej ogłoszeń z bazy danych.
        /// </summary>
        public DataTable Data => BazaDanych.GetTable("SELECT tresc from informacje");

        /// <summary>
        /// Przejście do okna dodania ogłoszeń po wciśnięciu przycisku.
        /// </summary>
        /// <param name="sender">Źródło</param>
        /// <param name="e">Dodatkowe argumenty</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).DataContext = new DodawanieOgłoszenia();
        }
    }
}
