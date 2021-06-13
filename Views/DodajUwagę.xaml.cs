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
    /// Klasa <c>DodajUwagę</c> zawiera metody pozwalające na dodawanie uwagi.
    /// </summary>
    public partial class DodajUwagę : UserControl
    {
        /// <summary>
        /// Konstruktor klasy DodajUwagę, pobiera tabelę uczniów.
        /// </summary>
        public DodajUwagę()
        {
            DataContext = this;
            Uczniowie = BazaDanych.ReadAsDictionary(@"SELECT dane_osobowe_Imie, dane_osobowe_Nazwisko, iducznia FROM uczen ORDER BY nr_w_dzienniku ASC").Select(x => new ComboBoxItem() { Content = x["iducznia"], ContentStringFormat = x["dane_osobowe_Imie"] + " " + x["dane_osobowe_Nazwisko"] }).ToList();
            InitializeComponent();
        }

        /// <value>Pobiera listę uczniów.</value>
        public List<ComboBoxItem> Uczniowie { get; private set; }
        /// <summary>
        /// Dodanie uwagi po wciśnięciu przycisku.
        /// </summary>
        /// <param name="sender">Źródło</param>
        /// <param name="e">Dodatkowe argumenty</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var user = ((MainWindow)Application.Current.MainWindow).LoggedUser;
            BazaDanych.Execute("INSERT INTO uwagi (data_uwagi, tresc, nauczyciel_id, uczen_iducznia) VALUES (@data_uwagi, @tresc, @nauczyciel_id, @uczen_iducznia)",
                new MySqlParameter("data_uwagi", DateTime.Today),
                new MySqlParameter("tresc", Tresc.Text),
                new MySqlParameter("nauczyciel_id", user.ID),
                new MySqlParameter("uczen_iducznia", Uczen.SelectionBoxItem)
                );
            MessageBox.Show("Dodano uwagę.");
            ((MainWindow)Application.Current.MainWindow).DataContext = new Uwagi();
        }
    }
}
