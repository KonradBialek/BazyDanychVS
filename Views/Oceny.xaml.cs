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
    /// Klasa <c>Oceny</c> zawiera metody pozwalające na odczyt ocen.
    /// </summary>
    public partial class Oceny : UserControl
    {
        /// <summary>
        /// Konstruktor klasy Oceny, ustawia widoczność przycisku dodania oceny.
        /// </summary>
        public Oceny()
        {
            DataContext = this;
            InitializeComponent();

            var user = ((MainWindow)Application.Current.MainWindow).LoggedUser;
            DodajButton.Visibility = user.Typ == UserType.Nauczyciel ? Visibility.Visible : Visibility.Collapsed;
        }
        /// <summary>
        /// Pobór tabeli dotyczącej ocen uczniów z bazy danych.
        /// </summary>
        public DataTable Current
        {
            get
            {
                var user = ((MainWindow)Application.Current.MainWindow).LoggedUser;
                if (user.Typ == UserType.Uczen)
                    return BazaDanych.GetTable(@"SELECT data, wartosc, przedmiot_nazwa as przedmiot, waga, opis, n.dane_osobowe_Imie as Imię_nauczyciela, n.dane_osobowe_Nazwisko as Nazwisko_nauczyciela
                        FROM ocena
                        LEFT JOIN nauczyciel n on ocena.nauczyciel_id = n.id
                        WHERE uczen_iducznia = @id", new MySql.Data.MySqlClient.MySqlParameter("id", user.ID));
                else if (user.Typ == UserType.Nauczyciel)
                {
                    return BazaDanych.GetTable(@"SELECT u.nr_w_dzienniku as nr, u.dane_osobowe_Imie as Imię_ucznia, u.dane_osobowe_Nazwisko as Nazwisko_ucznia, przedmiot_nazwa as nazwa_przedmiotu, wartosc as wartość_oceny, waga, opis
                        FROM ocena
                        LEFT JOIN uczen u on ocena.uczen_iducznia = u.iducznia
                        WHERE ocena.nauczyciel_id = @id", new MySql.Data.MySqlClient.MySqlParameter("id", user.ID));
                }
                else return null;
            }
        }
        /// <summary>
        /// Pobór tabeli dotyczącej średniej ocen uczniów z bazy danych.
        /// </summary>
        public DataTable Total
        {
            get
            {
                var user = ((MainWindow)Application.Current.MainWindow).LoggedUser;
                if (user.Typ == UserType.Uczen)
                    return BazaDanych.GetTable(@"SELECT przedmiot_nazwa as nazwa_przedmiotu, sum(waga*wartosc)/sum(wartosc) as srednia
                        FROM ocena
                        WHERE uczen_iducznia = @id
                        GROUP BY nazwa_przedmiotu", new MySql.Data.MySqlClient.MySqlParameter("id", user.ID));
                else if (user.Typ == UserType.Nauczyciel)
                {
                    return BazaDanych.GetTable(@"SELECT u.nr_w_dzienniku as nr, u.dane_osobowe_Imie as Imię_ucznia, u.dane_osobowe_Nazwisko as Nazwisko_ucznia, przedmiot_nazwa as nazwa_przedmiotu, sum(waga*wartosc)/sum(wartosc) as srednia
                        FROM ocena
                        LEFT JOIN uczen u on ocena.uczen_iducznia = u.iducznia
                        GROUP BY iducznia, nr, Imię_ucznia, Nazwisko_ucznia, nazwa_przedmiotu");
                }
                else return null;
            }
        }

        /// <summary>
        /// Przejście do okna dodania nieobecności po wciśnięciu przycisku.
        /// </summary>
        /// <param name="sender">Źródło</param>
        /// <param name="e">Dodatkowe argumenty</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).DataContext = new DodawanieOceny();
        }
    }
}
