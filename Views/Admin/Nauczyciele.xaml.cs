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
    /// Interaction logic for Nauczyciele.xaml
    /// </summary>
    public partial class Nauczyciele : UserControl
    {
        private int AktualnyId;

        public Nauczyciele()
        {
            DataContext = this;
            Lista = BazaDanych.ReadAsClass<Nauczyciel>(@"SELECT * FROM nauczyciel ORDER BY dane_osobowe_Nazwisko, dane_osobowe_Imie");
            Lista.Add(new Nauczyciel { id = -1 });
            InitializeComponent();
            Combo.SelectedItem = Lista.Last();
        }

        public List<Nauczyciel> Lista { get; private set; }

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
            }
            ((MainWindow)Application.Current.MainWindow).DataContext = new Administrator();
        }
        private void Usun_Click(object sender, RoutedEventArgs e)
        {

            if (AktualnyId != -1)
            {
                BazaDanych.Execute("DELETE FROM nauczyciel WHERE id = @id", new MySqlParameter("id", AktualnyId));
            }
    ((MainWindow)Application.Current.MainWindow).DataContext = new Administrator();
        }
    }
}
