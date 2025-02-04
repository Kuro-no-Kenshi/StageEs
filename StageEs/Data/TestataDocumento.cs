using StageEs.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StageEs.Data
{
    public class TestataDocumento
    {
        [Key]
        public int DocumentId { get; set; }
        [Required(ErrorMessage = "La data del documento è obbligatoria")]
        public DateTime DataDocumento { get; set; }
        [Required(ErrorMessage = "Il numero del documento è obbligatorio")]
        [RegularExpression(@"^[A-Za-z0-9]{10}$", ErrorMessage = "Il numero del documento deve essere di 10 caratteri alfanumerici")]
        public string NumeroDocumento { get; set; }
        [Required(ErrorMessage = "Il cliente è obbligatorio")]
        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }
        public List<RigaDocumento>? RigaDocumento { get; set; }
    }
}
