using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikacjaDostepowa.Models
{
    /// <summary>
    /// Klasa <c>UczenNaLekcji</c> zawiera właściwości opisujące ucznia w klasie.
    /// </summary>
    public class UczenNaLekcji
    {
        /// <value>Pobiera imię ucznia.</value>
        [StringLength(20, ErrorMessage = "Imię ucznia jest zbyt długie.")]
        public string dane_osobowe_Imie { get; set; }
        /// <value>Pobiera nazwisko ucznia.</value>
        [StringLength(25, ErrorMessage = "Nazwisko ucznia jest zbyt długie.")]
        public string dane_osobowe_Nazwisko { get; set; }
        /// <value>Pobiera nr w dzienniku ucznia.</value>
        [Range(0, 25, ErrorMessage = "Nieprawidłowy nr w dzienniku ucznia.")]
        public int nr_w_dzienniku { get; set; }
    }
}
