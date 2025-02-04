using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace StageEs.Data
{
    public class RigaDocumento
    {
        [Key]
        public int RigaId { get; set; }
        public int DocumentoId { get; set; }
        [Required(ErrorMessage = "La descrizione è obbligatoria")]
        [MinLength(10, ErrorMessage = "La descrizione deve avere almeno 10 caratteri")]
        [MaxLength(100, ErrorMessage = "La descrizione deve avere al massimo 100 caratteri")]
        public string Descrizione { get; set; }
        [Required(ErrorMessage = "La quantità è obbligatoria")]
        [RegularExpression(@"^-?\d+$", ErrorMessage = "La quantità deve essere un numero intero")]
        public int Quantita { get; set; }
    }
}
