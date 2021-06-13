using AplikacjaDostepowa.Models;
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

namespace AplikacjaDostepowa.Views
{
    /// <summary>
    /// Klasa <c>Lekcja</c> zawiera metody pozwalające na dodawanie lekcji.
    /// </summary>
    public partial class Lekcja : UserControl
    {
        /// <value>Zawiera dzień tygodnia.</value>
        private string dzienTygodnia;
        /// <value>Zawiera model lekcji.</value>
        private LekcjaModel lekcja;
        /// <value>Zawiera listę klas.</value>
        private List<Dictionary<string, object>> Klasy;
        /// <value>Zawiera tydzień roboczy.</value>
        static Dictionary<DayOfWeek, string> week = new Dictionary<DayOfWeek, string> { { DayOfWeek.Monday, "Poniedzialek" }, { DayOfWeek.Tuesday, "Wtorek" }, { DayOfWeek.Wednesday, "Sroda" }, { DayOfWeek.Thursday, "Czwartek" }, { DayOfWeek.Friday, "Piatek" }, { DayOfWeek.Saturday, "Sobota" }, { DayOfWeek.Sunday, "Niedziela" } };
        /// <summary>
        /// Konstruktor klasy Lekcja, pobiera przedmioty i klasy.
        /// </summary>
        public Lekcja()
        {
            dzienTygodnia = week[DateTime.Now.DayOfWeek];
            LoadPrzedmioty();
            Klasy = BazaDanych.ReadAsDictionary("SELECT * FROM klasa");
            DataContext = this;
            InitializeComponent();
        }
        /// <value>Pobiera listę klas.</value>
        public List<string> KlasyNazwy => Klasy.Select(x => x["oznaczenie"] as string).ToList();

        /// <value>Pobiera listę przedmiotów.</value>
        public List<string> Przedmioty { get; private set; }

        /// <summary>
        /// Wczytanie numerów lekcji.
        /// </summary>
        void LoadNumbers()
        {
            numerCombo.ItemsSource = BazaDanych.ReadAsArray(@"SELECT biezacy_szablon_lekcji_nr_w_dniu From klasa k
join roczny_plan_lekcji_has_biezacy_szablon_lekcji rplhbsl on k.roczny_plan_lekcji_id = rplhbsl.roczny_plan_lekcji_id
WHERE k.oznaczenie = @klasa AND biezacy_szablon_lekcji_dzien_w_tyg = @dzien", new MySqlParameter("klasa", klasaCombo.SelectedItem as string), new MySqlParameter("dzien", dzienTygodnia)).Select(x => (int)x[0]).ToList();

        }
        /// <summary>
        /// Wczytanie przedmiotów.
        /// </summary>
        void LoadPrzedmioty()
        {
            Przedmioty = BazaDanych.ReadAsArray(@"SELECT nazwa From przedmiot").Select(x => (string)x[0]).ToList();
        }
        /// <summary>
        /// Wczytanie uczniów.
        /// </summary>
        void LoadPeople()
        {
            var uczniowie = BazaDanych.ReadAsClass<UczenNaLekcji>(@"SELECT dane_osobowe_Imie, dane_osobowe_Nazwisko, nr_w_dzienniku From klasa k
join uczen u on k.oznaczenie = u.klasa_oznaczenie
WHERE k.oznaczenie=@klasa
ORDER BY nr_w_dzienniku ASC", new MySqlParameter("klasa", klasaCombo.SelectedItem as string));
            listaUczniow.Children.Clear();
            foreach (var uczen in uczniowie)
            {
                listaUczniow.Children.Add(new UczenNaLekcjiKontrolka(uczen));
            }
        }
        /// <summary>
        /// Wczytanie lekcji.
        /// </summary>
        void LoadLesson()
        {
            var user = ((MainWindow)Application.Current.MainWindow).LoggedUser;
            lekcja = BazaDanych.ReadAsClass<LekcjaModel>(@"select * from lekcja l
WHERE data_dnia = @data AND biezacy_szablon_lekcji_nr_w_dniu = @nr AND roczny_plan_lekcji_id = @roczny_plan_lekcji_id", new MySqlParameter("data", DateTime.Today), new MySqlParameter("nr", numerCombo.SelectedValue), new MySqlParameter("roczny_plan_lekcji_id", Klasy.First(x => x["oznaczenie"] == klasaCombo.SelectedItem)["roczny_plan_lekcji_id"])).FirstOrDefault();
            if (lekcja == null)
            {
                lekcja = new LekcjaModel();
            }
            Temat.Visibility = Visibility.Visible;
            Temat.Text = lekcja.temat;
            przedmiotCombo.SelectedItem = lekcja.przedmiot_nazwa;
        }
        /// <summary>
        /// Wyświetlenie numerów lekcji, uczniów i lekcji.
        /// </summary>
        /// <param name="sender">Źródło</param>
        /// <param name="e">Dodatkowe argumenty</param>
        private void klasaCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadNumbers();
            if (klasaCombo.SelectedItem != null && numerCombo.SelectedItem != null)
            {
                LoadPeople();
                LoadLesson();
            }
        }

        /// <summary>
        /// Dodanie lekcji po wciśnięciu przycisku.
        /// </summary>
        /// <param name="sender">Źródło</param>
        /// <param name="e">Dodatkowe argumenty</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var user = ((MainWindow)Application.Current.MainWindow).LoggedUser;
            var isInDB = BazaDanych.ReadAsClass<LekcjaModel>(@"select * from lekcja l
WHERE data_dnia = @data AND biezacy_szablon_lekcji_nr_w_dniu = @nr AND roczny_plan_lekcji_id = @roczny_plan_lekcji_id", new MySqlParameter("data", DateTime.Today), new MySqlParameter("nr", numerCombo.SelectedValue), new MySqlParameter("roczny_plan_lekcji_id", Klasy.First(x => x["oznaczenie"] == klasaCombo.SelectedItem)["roczny_plan_lekcji_id"])).Any();
            var parameters = new[] {                    new MySqlParameter("data_dnia", DateTime.Today),
                    new MySqlParameter("biezacy_szablon_lekcji_nr_w_dniu", numerCombo.SelectedItem),
                    new MySqlParameter("biezacy_szablon_lekcji_dzien_w_tyg", dzienTygodnia),
                    new MySqlParameter("nauczyciel_id", user.ID),
                    new MySqlParameter("temat", Temat.Text),
                    new MySqlParameter("przedmiot_nazwa", przedmiotCombo.Text),
                    new MySqlParameter("roczny_plan_lekcji_id", Klasy.First(x => x["oznaczenie"] == klasaCombo.SelectedItem)["roczny_plan_lekcji_id"])};
            if (isInDB)
            {
                BazaDanych.Execute("UPDATE lekcja SET temat=@temat, przedmiot_nazwa= @przedmiot_nazwa WHERE data_dnia = @data_dnia AND biezacy_szablon_lekcji_nr_w_dniu = @biezacy_szablon_lekcji_nr_w_dniu AND roczny_plan_lekcji_id = @roczny_plan_lekcji_id", parameters);
                MessageBox.Show("Zaktualizowano lekcję.");
            }
            else
            {
                BazaDanych.Execute("INSERT INTO lekcja (data_dnia, biezacy_szablon_lekcji_nr_w_dniu, biezacy_szablon_lekcji_dzien_w_tyg, nauczyciel_id, temat, roczny_plan_lekcji_id, przedmiot_nazwa, informacje_id) VALUES (@data_dnia, @biezacy_szablon_lekcji_nr_w_dniu, @biezacy_szablon_lekcji_dzien_w_tyg, @nauczyciel_id, @temat, @roczny_plan_lekcji_id, @przedmiot_nazwa, 1)", parameters);
                BazaDanych.Execute("INSERT INTO biezacy_szablon_lekcji (nr_w_dniu, dzien_w_tyg) VALUES (@biezacy_szablon_lekcji_nr_w_dniu, @biezacy_szablon_lekcji_dzien_w_tyg)", parameters);
                MessageBox.Show("Dodano lekcję.");
            }
        }
        /// <summary>
        /// Zmiana wybranego numeru lekcji.
        /// </summary>
        /// <param name="sender">Źródło</param>
        /// <param name="e">Dodatkowe argumenty</param>
        private void numerCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (klasaCombo.SelectedItem != null && numerCombo.SelectedItem != null)
            {
                LoadPeople();
                LoadLesson();
            }
        }
    }
}
