using AplikacjaDostepowa.Models;
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

namespace AplikacjaDostepowa.Views.Admin
{
    /// <summary>
    /// Klasa <c>Nauczyciele</c> zawiera metody pozwalające na modyfikację i wyświetlanie danych nauuczycieli.
    /// </summary>
    public partial class Nauczyciele : UserControl
    {
        /// <value>Pobiera Id wybranego nauczyciela.</value>
        private int AktualnyId;

        /// <summary>
        /// Konstruktor klasy Nauczyciele, pobiera i wyświetla listę nauczycieii.
        /// </summary>
        public Nauczyciele()
        {
            DataContext = this;
            Lista = BazaDanych.ReadAsClass<Nauczyciel>(@"SELECT * FROM nauczyciel ORDER BY dane_osobowe_Nazwisko, dane_osobowe_Imie");
            Lista.Add(new Nauczyciel { id = -1 });
            InitializeComponent();
            Combo.SelectedItem = Lista.Last();
        }

        /// <value>Pobiera listę nayczycieli.</value>
        public List<Nauczyciel> Lista { get; private set; }

        /// <summary>
        /// Wyświetlenie danych wybranego nauczyciela.
        /// </summary>
        /// <param name="sender">Źródło</param>
        /// <param name="e">Dodatkowe argumenty</param>
        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = Combo.SelectedItem as Nauczyciel;
            AktualnyId = item.id;
            Imię.Text = item.dane_osobowe_Imie;
            Nazwisko.Text = item.dane_osobowe_Nazwisko;
            Wiek.Text = item.wiek.ToString();
            Login.Text = item.login;
            CzyAdmin.IsChecked = item.czy_admin;
        }

        /// <summary>
        /// Dodanie lub zaktualizowanie danych nauczyciela w odpowiednich tabelach.
        /// </summary>
        /// <param name="sender">Źródło</param>
        /// <param name="e">Dodatkowe argumenty</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!BazaDanych.ReadAsArray("SELECT 1 FROM dane_osobowe WHERE Imie = @Imie AND Nazwisko = @Nazwisko", new MySqlParameter("Imie", Imię.Text), new MySqlParameter("Nazwisko", Nazwisko.Text)).Any())
            {
                BazaDanych.Execute("INSERT INTO dane_osobowe (Imie, Nazwisko) VALUES(@Imie, @Nazwisko)", new MySqlParameter("Imie", Imię.Text), new MySqlParameter("Nazwisko", Nazwisko.Text));
            }
            if (AktualnyId == -1)
            {
                BazaDanych.Execute("INSERT INTO nauczyciel (dane_osobowe_Imie, dane_osobowe_Nazwisko, wiek, login, czy_admin, haslo) VALUES (@dane_osobowe_Imie, @dane_osobowe_Nazwisko, @wiek, @login, @czy_admin, @haslo)",
                   new MySqlParameter("dane_osobowe_Imie", Imię.Text),
                   new MySqlParameter("dane_osobowe_Nazwisko", Nazwisko.Text),
                   new MySqlParameter("wiek", Wiek.Text),
                   new MySqlParameter("login", Login.Text),
                   new MySqlParameter("czy_admin", CzyAdmin.IsChecked),
                   new MySqlParameter("haslo", "69f157f5a264958d01d15f9624eb82f3")
                   );
                MessageBox.Show("Dodano nauczyciela.");
            }
            else
            {
                BazaDanych.Execute("UPDATE nauczyciel SET dane_osobowe_Imie=@dane_osobowe_Imie, dane_osobowe_Nazwisko=@dane_osobowe_Nazwisko, wiek=@wiek, login=@login, czy_admin=@czy_admin WHERE id = @id",
                    new MySqlParameter("dane_osobowe_Imie", Imię.Text),
                    new MySqlParameter("dane_osobowe_Nazwisko", Nazwisko.Text),
                    new MySqlParameter("wiek", Wiek.Text),
                    new MySqlParameter("login", Login.Text),
                    new MySqlParameter("czy_admin", CzyAdmin.IsChecked),
                    new MySqlParameter("id", AktualnyId)
                    );
                MessageBox.Show("Zaktualizowano nauczyciela, jeżeli nie wywołano wiadomości z błędem.");
            }
            ((MainWindow)Application.Current.MainWindow).DataContext = new Administrator();
        }
        /// <summary>
        /// Usunięcie danych nauczyciela z odpowiednich tabel.
        /// </summary>
        /// <param name="sender">Źródło</param>
        /// <param name="e">Dodatkowe argumenty</param>
        private void Usun_Click(object sender, RoutedEventArgs e)
        {

            if (AktualnyId != -1)
            {
                
                BazaDanych.Execute("SET FOREIGN_KEY_CHECKS = 0");
                BazaDanych.Execute("UPDATE klasa SET nauczyciel_id = 0 WHERE nauczyciel_id = @id", new MySqlParameter("id", AktualnyId));
                BazaDanych.Execute("UPDATE lekcja SET nauczyciel_id = 0 WHERE nauczyciel_id = @id", new MySqlParameter("id", AktualnyId));
                BazaDanych.Execute("DELETE FROM nauczyciel_moze_prowadzic_przedmiot WHERE nauczyciel_id = @id", new MySqlParameter("id", AktualnyId));
                BazaDanych.Execute("DELETE FROM nauczyciel_prowadzi_przedmiot WHERE nauczyciel_id = @id", new MySqlParameter("id", AktualnyId));
                BazaDanych.Execute("UPDATE nieobecnosc SET nauczyciel_id = 0 WHERE nauczyciel_id = @id", new MySqlParameter("id", AktualnyId));
                BazaDanych.Execute("UPDATE ocena SET nauczyciel_id = 0 WHERE nauczyciel_id = @id", new MySqlParameter("id", AktualnyId));
                BazaDanych.Execute("UPDATE uwagi SET nauczyciel_id = 0 WHERE nauczyciel_id = @id", new MySqlParameter("id", AktualnyId));
                BazaDanych.Execute("DELETE FROM nauczyciel WHERE id = @id", new MySqlParameter("id", AktualnyId));
                MessageBox.Show("Usunięto nauczyciela.");
            }
    ((MainWindow)Application.Current.MainWindow).DataContext = new Administrator();
        }
    }
}
