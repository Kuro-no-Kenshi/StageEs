namespace StageEs.Models
{
    public class CreateDocumentoDTO
    {
        public DateTime DataDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public int CustomerId { get; set; } // Solo l'ID del cliente
        public List<CreateRigaDocumentoDTO>? Righe { get; set; } // Lista di righe
    }

    public class CreateRigaDocumentoDTO
    {
        public string Descrizione { get; set; }
        public int Quantita { get; set; }
    }

}
