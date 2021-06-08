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
    /// Logika interakcji dla klasy Nieobecności.xaml
    /// </summary>
    public partial class Nieobecności : UserControl
    {
        public Nieobecności()
        {
            DataContext = this;
            InitializeComponent();
            LoadSummary();
            var user = ((MainWindow)Application.Current.MainWindow).LoggedUser;
            DodajButton.Visibility = user.Typ == UserType.Nauczyciel ? Visibility.Visible : Visibility.Collapsed;
        }

        public DataTable Data
        {
            get
            {
                var user = ((MainWindow)Application.Current.MainWindow).LoggedUser;
                if (user.Typ == UserType.Uczen)
                    return BazaDanych.GetTable(@"select `n`.`lekcja_data` as data, `n`.`lekcja_nr_w_dniu` as nr_lekcji_w_danym_dniu, `n`.`stan`
                        from uczen u
                        join nieobecnosc n on u.iducznia = n.uczen_iducznia
                        left join lekcja l on n.lekcja_data = l.data_dnia and n.lekcja_nr_w_dniu = l.biezacy_szablon_lekcji_nr_w_dniu
                        where u.iducznia = @id", new MySql.Data.MySqlClient.MySqlParameter("id", user.ID));
                else if (user.Typ == UserType.Nauczyciel)
                {
                    return BazaDanych.GetTable(@"select u.dane_osobowe_Imie, u.dane_osobowe_Nazwisko, u.nr_w_dzienniku, u.klasa_oznaczenie, `n`.`lekcja_data`, `n`.`lekcja_nr_w_dniu`, `n`.`stan`
                        from uczen u
                        join nieobecnosc n on u.iducznia = n.uczen_iducznia
                        left join lekcja l on n.lekcja_data = l.data_dnia and n.lekcja_nr_w_dniu = l.biezacy_szablon_lekcji_nr_w_dniu");
                }
                else return null;
            }
        }
        public void LoadSummary()
        {
            string query;
            var user = ((MainWindow)Application.Current.MainWindow).LoggedUser;
            if (user.Typ == UserType.Uczen)
            {
                 query = @"SELECT (SELECT count(*)
FROM uczen u
LEFT JOIN klasa k on u.klasa_oznaczenie = k.oznaczenie
LEFT JOIN lekcja l on k.roczny_plan_lekcji_id = l.roczny_plan_lekcji_id
WHERE u.iducznia = @iducznia) as suma,  (SELECT count(*) FROM nieobecnosc n WHERE uczen_iducznia = @iducznia) as nieobecnosci, (SELECT count(*) FROM nieobecnosc n WHERE uczen_iducznia = @iducznia AND stan = 'nb') as nieobecnosci_nieusprawiedliwione";
            }
            else
            {
                 query = @"SELECT (SELECT count(*)
FROM uczen u
LEFT JOIN klasa k on u.klasa_oznaczenie = k.oznaczenie
LEFT JOIN lekcja l on k.roczny_plan_lekcji_id = l.roczny_plan_lekcji_id) as suma, (SELECT count(*) FROM nieobecnosc n) as nieobecnosci, (SELECT count(*) FROM nieobecnosc n WHERE stan = 'nb') as nieobecnosci_nieusprawiedliwione
";
            }
            var data = BazaDanych.ReadAsDictionary(query, new MySql.Data.MySqlClient.MySqlParameter("iducznia", user.ID)).Single();
            Podsumowanie.Text = $"Podsumowanie:\r\n" +
                $"Obecności: {(long)data["suma"] - (long)data["nieobecnosci"]} ({((100f * ((long)data["suma"] - (long)data["nieobecnosci"])) / (long)data["suma"]).ToString("0.##")}%)\r\n" +
                $"Nieobecności: {data["nieobecnosci"]}\r\n" +
                $"W tym nieusprawiedliwione: {data["nieobecnosci_nieusprawiedliwione"]}";
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).DataContext = new DodajNieobecność();
        }
    }
}
