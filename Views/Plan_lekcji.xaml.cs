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
    /// Klasa <c>Plan_lekcji</c> zawiera metody pozwalające na odczyt planu lekcji.
    /// </summary>
    public partial class Plan_lekcji : UserControl
    {
        /// <summary>
        /// Konstruktor klasy Plan_lekcji.
        /// </summary>
        public Plan_lekcji()
        {
            DataContext = this;
            InitializeComponent();
        }
        /// <summary>
        /// Pobór tabeli dotyczącej planu lekcji z bazy danych.
        /// </summary>
        public DataTable Data => BazaDanych.GetTable("SELECT data_dnia, biezacy_szablon_lekcji_dzien_w_tyg as dzien_tygodnia, biezacy_szablon_lekcji_nr_w_dniu as nr, przedmiot_nazwa, temat, n.dane_osobowe_Imie imie_nauczyciela, n.dane_osobowe_Nazwisko nazwisko_nauczyciela FROM lekcja l JOIN nauczyciel n on l.nauczyciel_id = n.id ORDER BY data_dnia DESC, biezacy_szablon_lekcji_nr_w_dniu DESC");
    }
}
