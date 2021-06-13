using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikacjaDostepowa.Models
{
    /// <summary>
    /// Klasa <c>User</c> zawiera właściwości opisujące użytkownika i metodę formatowania wyświetlania jego danych osobowych.
    /// <list type="bullet">
    /// <item>
    /// <term>ToString </term>
    /// <description>Metoda wykorzystująca typ użytkownika korzystającego z aplikacji do odpowiedniego formatowania i wyświetlania jego danych.</description>
    /// </item>
    /// </list>
    /// </summary>
    public class User
    {
        /// <value>Pobiera indeks użytkownika.</value>
        [Range(-1, 640, ErrorMessage = "Nieprawidłowy indeks użytkownika.")]
        public int ID { get; set; }
        /// <value>Pobiera imię użytkownika.</value>
        [StringLength(20, ErrorMessage = "Imię użytkownika jest zbyt długie.")]
        public string Imie { get; set; }
        /// <value>Pobiera nazwisko użytkownika.</value>
        [StringLength(25, ErrorMessage = "Nazwisko użytkownika jest zbyt długie.")]
        public string Nazwisko { get; set; }
        /// <value>Pobiera oznaczenie klasy.</value>
        [StringLength(2, ErrorMessage = "Oznaczenie klasy jest zbyt długie.")]
        public string Klasa { get; set; }
        /// <value>Pobiera typ użytkownika.</value>
        public UserType Typ { get; set; }
        /// <value>Pobiera informację czy użytkownik jet administratorem.</value>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Metoda wykorzystująca typ użytkownika korzystającego z aplikacji do odpowiedniego formatowania i wyświetlania jego danych.
        /// </summary>
        /// <returns>Odpowiednio sformatowany tekst lub pusty ciąg znaków.</returns>
        public override string ToString()
        {
            if (Typ == UserType.Uczen)
                return $"Uczeń: {Imie} {Nazwisko} ({Klasa})";
            if (Typ == UserType.Nauczyciel)
                return $"Nauczyciel: {Imie} {Nazwisko}";
            if (Typ == UserType.Rodzic)
                return $"Rodzic: {Nazwisko}";
            return "";
        }
    }
}
