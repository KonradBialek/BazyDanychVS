using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Klasa <c>DodawanieOceny</c> zawiera metody pozwalające na dodawanie oceny.
    /// </summary>
    public partial class DodawanieOceny : UserControl
    {
        /// <value>Pobiera listę przedmiotów.</value>
        public List<string> Przedmioty { get; private set; }
        /// <value>Pobiera listę uczniów.</value>
        public List<ComboBoxItem> Uczniowie { get; private set; }
        /// <summary>
        /// Konstruktor klasy DodawanieOceny, pobiera tabelę uczniów i przedmiotów.
        /// </summary>
        public DodawanieOceny()
        {
            DataContext = this;
            Przedmioty = BazaDanych.ReadAsArray(@"SELECT nazwa From przedmiot").Select(x => (string)x[0]).ToList();
            Uczniowie = BazaDanych.ReadAsDictionary(@"SELECT dane_osobowe_Imie, dane_osobowe_Nazwisko, iducznia FROM uczen ORDER BY nr_w_dzienniku ASC").Select(x => new ComboBoxItem() { Content = x["iducznia"], ContentStringFormat = x["dane_osobowe_Imie"] + " " + x["dane_osobowe_Nazwisko"] }).ToList();
            InitializeComponent();
        }
        /// <summary>
        /// Dodanie oceny po wciśnięciu przycisku.
        /// </summary>
        /// <param name="sender">Źródło</param>
        /// <param name="e">Dodatkowe argumenty</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var user = ((MainWindow)Application.Current.MainWindow).LoggedUser;
            BazaDanych.Execute("INSERT INTO ocena (data, wartosc, nauczyciel_id, przedmiot_nazwa, waga, opis, uczen_iducznia) VALUES (@data, @wartosc, @nauczyciel_id, @przedmiot_nazwa, @waga, @opis, @uczen_iducznia)",
                new MySqlParameter("data", DateTime.Today),
                new MySqlParameter("wartosc", float.Parse(Wartosc.Text.Replace(',', '.'), CultureInfo.InvariantCulture)),
                new MySqlParameter("nauczyciel_id", user.ID),
                new MySqlParameter("przedmiot_nazwa", Przedmiot.SelectedValue),
                new MySqlParameter("waga", int.Parse(Waga.Text)),
                new MySqlParameter("opis", Opis.Text),
                new MySqlParameter("uczen_iducznia", Uczen.SelectionBoxItem)
                );
            MessageBox.Show("Dodano ocenę.");
            ((MainWindow)Application.Current.MainWindow).DataContext = new Oceny();
        }
    }
}
