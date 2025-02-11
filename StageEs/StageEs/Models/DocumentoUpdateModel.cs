namespace StageEs.Models
{
    public class DocumentoUpdateModel
    {
        public string NumeroDocumento { get; set; }
        public DateOnly DataDocumento { get; set; }
        public int CustomerId { get; set; }
    }
}
