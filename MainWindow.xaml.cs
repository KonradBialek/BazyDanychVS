//using AplikacjaDostepowa.ViewModels;
using AplikacjaDostepowa.Models;
using AplikacjaDostepowa.Views;
using MySql.Data.MySqlClient;
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

namespace AplikacjaDostepowa
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Logowanie.Visibility = Visibility.Visible;
        }

        public void Zaloguj(User qwe)
        {
            LoggedUser = qwe;
            DataContext = new Oceny();
            Logowanie.Visibility = Visibility.Collapsed;
            UserName.Text = qwe.ToString();
            ArchiwumButton.Visibility = qwe.IsAdmin ? Visibility.Visible : Visibility.Collapsed;
            AdminButton.Visibility = qwe.IsAdmin ? Visibility.Visible : Visibility.Collapsed;
            LekcjaButton.Visibility = qwe.Typ == UserType.Nauczyciel ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Oceny(object sender, RoutedEventArgs e)
        {
            DataContext = new Oceny();

        }
        private void Nieobecnośći(object sender, RoutedEventArgs e)
        {
            DataContext = new Nieobecności();
        }
        private void Ogłoszenia(object sender, RoutedEventArgs e)
        {
            DataContext = new Ogłoszenia();
        }
        private void Terminarz(object sender, RoutedEventArgs e)
        {
            DataContext = new Terminarz();
        }
        private void Uwagi(object sender, RoutedEventArgs e)
        {
            DataContext = new Uwagi();
        }
        private void Plan_lekcji(object sender, RoutedEventArgs e)
        {
            DataContext = new Plan_lekcji();
        }
        private void Archiwum(object sender, RoutedEventArgs e)
        {
            DataContext = new Archiwum();
        }
        private void Wyloguj(object sender, RoutedEventArgs e)
        {
            LoggedUser = null;
            Logowanie.Visibility = Visibility.Visible;
        }
        private void Admin(object sender, RoutedEventArgs e)
        {
            DataContext = new Administrator();
        }
        private void Lekcja(object sender, RoutedEventArgs e)
        {
            DataContext = new Lekcja();
        }
        public User LoggedUser { get; set; } = null;

        public User UserInBase(string login, string haslo)
        {
            string query = $"select idrodzic, nazwisko from rodzic where haslo = @haslo AND login = @login";
            var users = BazaDanych.ReadAsDictionary(query, new MySqlParameter("haslo", haslo), new MySqlParameter("login", login));
            if (users.Any())
            {
                return new User { ID = (int)users[0]["idrodzic"], Nazwisko = (string)users[0]["nazwisko"], Typ = UserType.Rodzic };
            }
            query = $"select iducznia, dane_osobowe_Imie, dane_osobowe_Nazwisko, klasa_oznaczenie from uczen where haslo = @haslo AND login = @login";
            users = BazaDanych.ReadAsDictionary(query, new MySqlParameter("haslo", haslo), new MySqlParameter("login", login));
            if (users.Any())
            {
                return new User { ID = (int)users[0]["iducznia"], Imie = (string)users[0]["dane_osobowe_Imie"], Nazwisko = (string)users[0]["dane_osobowe_Nazwisko"], Klasa = (string)users[0]["klasa_oznaczenie"], Typ = UserType.Uczen };
            }
            query = $"select id, dane_osobowe_Imie, dane_osobowe_Nazwisko, czy_admin from nauczyciel where haslo = @haslo AND login = @login";
            users = BazaDanych.ReadAsDictionary(query, new MySqlParameter("haslo", haslo), new MySqlParameter("login", login));
            if (users.Any())
            {
                return new User { ID = (int)users[0]["id"], Imie = (string)users[0]["dane_osobowe_Imie"], Nazwisko = (string)users[0]["dane_osobowe_Nazwisko"], Typ = UserType.Nauczyciel, IsAdmin = (bool)users[0]["czy_admin"] };
            }
            return null;
        }

    }
}
