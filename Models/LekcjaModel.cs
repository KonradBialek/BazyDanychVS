using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikacjaDostepowa.Models
{
    /// <summary>
    /// Klasa <c>LekcjaModel</c> zawiera właściwości opisujące temat lekcji i przedmiot, z którego jest lekcja.
    /// </summary>
    class LekcjaModel
    {
        /// <value>Pobiera temat lekcji.</value>
        [StringLength(45, ErrorMessage = "Temat lekcji jest zbyt długi.")]
        public string temat { get; set; }

        /// <value>Pobiera nazwę przedmiotu.</value>
        [StringLength(20, ErrorMessage = "Nazwa przedmiotu jest zbyt długa.")]
        public string przedmiot_nazwa { get; set; }
    }
}
