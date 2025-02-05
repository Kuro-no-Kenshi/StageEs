using System.ComponentModel.DataAnnotations;

namespace StageEs.Data
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "La ragione sociale è obbligatoria.")]
        [MinLength(3, ErrorMessage = "La ragione sociale deve contenere almeno 3 caratteri.")]
        [MaxLength(100, ErrorMessage = "La ragione sociale non può superare i 100 caratteri.")]
        [RegularExpression(@"^[a-zA-Z0-9&.,\s-]+(?:\s(S\.r\.l\.|S\.p\.A\.|S\.n\.c\.|S\.a\.s\.|S\.a\.p\.a\.))?$",
        ErrorMessage = "La ragione sociale deve contenere solo caratteri validi e includere la forma giuridica.")]
        public string RagioneSociale { get; set; }

        [Required(ErrorMessage = "La Partita IVA è obbligatoria.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "La Partita IVA deve essere composta da 11 cifre numeriche.")]
        public string PIVA { get; set; }

        [Required(ErrorMessage = "Il Codice Fiscale è obbligatorio.")]
        [RegularExpression(@"^[A-Z0-9]{11,16}$", ErrorMessage = "Il Codice Fiscale deve essere alfanumerico e lungo 11 o 16 caratteri.")]
        public string CodFisc { get; set; }

        [Required(ErrorMessage = "La città è obbligatoria.")]
        [RegularExpression(@"^[A-Za-zàáâãäåæçèéêëìíîïòóôõöøùúûüýÿ\s\-]+$", ErrorMessage = "La città deve contenere solo lettere, spazi e trattini.")]
        public string Citta { get; set; }

        [Required(ErrorMessage = "Il CAP è obbligatorio.")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Il CAP deve essere composto da 5 cifre.")]
        public string Cap { get; set; }

        [Required(ErrorMessage = "La provincia è obbligatoria.")]
        [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "La provincia deve essere composta da 2 lettere maiuscole.")]
        public string Provincia { get; set; }

        [Required(ErrorMessage = "La via è obbligatoria.")]
        [RegularExpression(@"^[A-Za-z0-9\s\.\-]+(\s\d+[A-Za-z]?\-?\d*\/?[A-Za-z]?)?$", ErrorMessage = "La via deve contenere solo lettere, numeri, trattini e numeri civici.")]
        public string Via { get; set; }
    }
}
