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
    /// Interaction logic for DodajUwagę.xaml
    /// </summary>
    public partial class DodajUwagę : UserControl
    {
        public DodajUwagę()
        {
            DataContext = this;
            Uczniowie = BazaDanych.ReadAsDictionary(@"SELECT dane_osobowe_Imie, dane_osobowe_Nazwisko, iducznia FROM uczen ORDER BY nr_w_dzienniku ASC").Select(x => new ComboBoxItem() { Content = x["iducznia"], ContentStringFormat = x["dane_osobowe_Imie"] + " " + x["dane_osobowe_Nazwisko"] }).ToList();
            InitializeComponent();
        }

        public List<ComboBoxItem> Uczniowie { get; private set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var user = ((MainWindow)Application.Current.MainWindow).LoggedUser;
            BazaDanych.Execute("INSERT INTO uwagi (data_uwagi, tresc, nauczyciel_id, uczen_iducznia) VALUES (@data_uwagi, @tresc, @nauczyciel_id, @uczen_iducznia)",
                new MySqlParameter("data_uwagi", DateTime.Today),
                new MySqlParameter("tresc", Tresc.Text),
                new MySqlParameter("nauczyciel_id", user.ID),
                new MySqlParameter("uczen_iducznia", Uczen.SelectionBoxItem)
                );
            ((MainWindow)Application.Current.MainWindow).DataContext = new Uwagi();
        }
    }
}
