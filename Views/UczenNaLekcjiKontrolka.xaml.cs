using AplikacjaDostepowa.Models;
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
    /// Klasa <c>UczenNaLekcjiKontrolka</c> zawiera metodę pozwalającą na ustawienie danych ucznia do wyświetlenia na liście klasy.
    /// </summary>
    public partial class UczenNaLekcjiKontrolka : UserControl
    {
        /// <summary>
        /// Ustawienie danych ucznia do wyświetlenia na liście klasy
        /// </summary>
        /// <param name="uczen">Dane ucznia</param>
        public UczenNaLekcjiKontrolka(UczenNaLekcji uczen)
        {
            InitializeComponent();
            ImieNazwisko.Text = uczen.nr_w_dzienniku + " " + uczen.dane_osobowe_Imie + " " + uczen.dane_osobowe_Nazwisko;
        }
    }
}
