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
    /// Klasa <c>Uwagi</c> zawiera metody pozwalające na odczyt uwag.
    /// </summary>
    public partial class Uwagi : UserControl
    {
        /// <summary>
        /// Konstruktor klasy Uwagi, ustawia widoczność przycisku dodania uwagi.
        /// </summary>
        public Uwagi()
        {
            DataContext = this;
            InitializeComponent();

            var user = ((MainWindow)Application.Current.MainWindow).LoggedUser;
            DodajButton.Visibility = user.Typ == UserType.Nauczyciel ? Visibility.Visible : Visibility.Collapsed;
        }
        /// <summary>
        /// Pobór tabeli dotyczącej uwag z bazy danych.
        /// </summary>
        public DataTable Data
        {
            get
            {
                var user = ((MainWindow)Application.Current.MainWindow).LoggedUser;
                if (user.Typ == UserType.Uczen)
                    return BazaDanych.GetTable(@"SELECT data_uwagi as data, dane_osobowe_Imie as imie_nauczyciela, dane_osobowe_Nazwisko as nazwisko_nauczyciela, tresc as uwaga
                                                    FROM uwagi u
                                                    JOIN nauczyciel n on u.nauczyciel_id = n.id
                                                    WHERE uczen_iducznia = @id", new MySql.Data.MySqlClient.MySqlParameter("id", user.ID));
                else if (user.Typ == UserType.Nauczyciel)
                {
                    return BazaDanych.GetTable(@"SELECT * FROM uwagiuczniow");
                }
                else return null;
            }
        }

        /// <summary>
        /// Przejście do okna dodania uwag po wciśnięciu przycisku.
        /// </summary>
        /// <param name="sender">Źródło</param>
        /// <param name="e">Dodatkowe argumenty</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).DataContext = new DodajUwagę();
        }
    }
}
