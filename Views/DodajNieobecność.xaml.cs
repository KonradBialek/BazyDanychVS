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
    /// Interaction logic for DodajNieobecność.xaml
    /// </summary>
    public partial class DodajNieobecność : UserControl
    {
        public DodajNieobecność()
        {
            DataContext = this;
            Uczniowie = BazaDanych.ReadAsDictionary(@"SELECT dane_osobowe_Imie, dane_osobowe_Nazwisko, iducznia FROM uczen ORDER BY nr_w_dzienniku ASC").Select(x => new ComboBoxItem() { Content = x["iducznia"], ContentStringFormat = x["dane_osobowe_Imie"] + " " + x["dane_osobowe_Nazwisko"] }).ToList();

            InitializeComponent();
        }
        public List<ComboBoxItem> Uczniowie { get; private set; }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var user = ((MainWindow)Application.Current.MainWindow).LoggedUser;
            var lekcja = (Lekcja.SelectedItem as ComboBoxItem).Tag as Dictionary<string, object>;
            BazaDanych.Execute("INSERT INTO nieobecnosc (stan, nauczyciel_id, uczen_iducznia, lekcja_data, lekcja_nr_w_dniu) VALUES (@stan, @nauczyciel_id, @uczen_iducznia, @lekcja_data, @lekcja_nr_w_dniu)",
                new MySqlParameter("stan", Usprawiedliwiona.IsChecked.Value ? "u" : "nb"),
                new MySqlParameter("nauczyciel_id", user.ID),
                new MySqlParameter("uczen_iducznia", Uczen.SelectionBoxItem),
                new MySqlParameter("lekcja_data", lekcja["data_dnia"]),
                new MySqlParameter("lekcja_nr_w_dniu", lekcja["biezacy_szablon_lekcji_nr_w_dniu"])
                );
            MessageBox.Show("Dodano nieobecność.");
            ((MainWindow)Application.Current.MainWindow).DataContext = new Nieobecności();
        }

        private void Uczen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var user = ((MainWindow)Application.Current.MainWindow).LoggedUser;
            var lekcje = BazaDanych.ReadAsDictionary(@"SELECT l.*
FROM uczen u
JOIN klasa k on u.klasa_oznaczenie = k.oznaczenie
JOIN lekcja l on k.roczny_plan_lekcji_id = l.roczny_plan_lekcji_id
WHERE u.iducznia = @iducznia AND (SELECT 1 FROM nieobecnosc n WHERE n.lekcja_data = l.data_dnia AND n.lekcja_nr_w_dniu = l.biezacy_szablon_lekcji_nr_w_dniu AND n.uczen_iducznia = u.iducznia LIMIT 1) IS NULL
", new MySqlParameter("iducznia", Uczen.SelectionBoxItem))
                .Select(x => new ComboBoxItem() { Tag = x, Content = x["data_dnia"] + " " + x["biezacy_szablon_lekcji_nr_w_dniu"] + " " + x["przedmiot_nazwa"] }).ToList();
            Lekcja.Items.Clear();
            foreach(var x in lekcje)
            {
                Lekcja.Items.Add(x);
            }
        }
    }
}
