using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikacjaDostepowa.Models
{
    /// <summary>
    /// Klasa <c>Nauczyciel</c> zawiera właściwości opisujące nauczyciela i metodę formatowania wyświetlania jego danych osobowych na liście wyboru do edycji.
    /// <list type="bullet">
    /// <item>
    /// <term>ToString </term>
    /// <description>Metoda przekształcająca id nauczyciela dodawanego do aplikacji do odpowiedniego formatowania i wyświetlania jego danych na liście wyboru do edycji.</description>
    /// </item>
    /// </list>
    /// </summary>
    public class Nauczyciel
    {
        /// <value>Pobiera indeks nauczyciela.</value>
        [Range(-1, 50, ErrorMessage = "Nieprawidłowy indeks nauczyciela.")]
        public int id { get; set; }
        /// <value>Pobiera informację czy nauczyciel jet administratorem.</value>
        public bool czy_admin { get; set; }
        /// <value>Pobiera login nauczyciela.</value>
        [StringLength(15, ErrorMessage = "Login jest zbyt długi.")]
        public string login { get; set; }
        /// <value>Pobiera wiek nauczyciela.</value>
        [Range(0, 100, ErrorMessage = "Nieprawidłowy wiek nauczyciela.")]
        public int wiek { get; set; }
        /// <value>Pobiera imię nauczyciela.</value>
        [StringLength(20, ErrorMessage = "Imię nauczyciela jest zbyt długie.")]
        public string dane_osobowe_Imie { get; set; }
        /// <value>Pobiera nazwisko nauczyciela.</value>
        [StringLength(25, ErrorMessage = "Nazwisko nauczyciela jest zbyt długie.")]
        public string dane_osobowe_Nazwisko { get; set; }
    
        /// <summary>
        /// Metoda wykorzystująca id nauczyciela dodawanego do aplikacji do odpowiedniego formatowania i wyświetlania jego danych na liście wyboru do edycji.
        /// </summary>
        /// <returns>Odpowiednio sformatowany tekst.</returns>  
        public override string ToString()
        {
            if (id == -1)
                return "Dodaj nowy";
            else
                return dane_osobowe_Imie + " " + dane_osobowe_Nazwisko;
        }
    }
}
