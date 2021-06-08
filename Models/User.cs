using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikacjaDostepowa.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Klasa { get; set; }
        public UserType Typ { get; set; }
        public bool IsAdmin { get; set; }
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
