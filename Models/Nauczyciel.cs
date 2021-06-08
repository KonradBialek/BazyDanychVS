using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikacjaDostepowa.Models
{
    public class Nauczyciel
    {
        public int id { get; set; }
        public bool czy_admin { get; set; }
        public string login { get; set; }
        public int wiek { get; set; }
        public string dane_osobowe_Imie { get; set; }
        public string dane_osobowe_Nazwisko { get; set; }
        public override string ToString()
        {
            if (id == -1)
                return "Dodaj nowy";
            else
                return dane_osobowe_Imie + " " + dane_osobowe_Nazwisko;
        }
    }
}
