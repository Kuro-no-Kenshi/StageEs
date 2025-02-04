using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageEs.Data;
using System.Linq;
using System.Threading.Tasks;

namespace StageEs.Controllers
{
    [Route("api/righe-documento")]
    [ApiController]
    public class RigaDocumentoController : ControllerBase
    {
        private readonly AziendaDbContext _context;

        public RigaDocumentoController(AziendaDbContext context)
        {
            _context = context;
        }

        // GET: api/righe-documento/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<RigaDocumento>> GetRigaDocumento(int id)
        {
            var riga = await _context.RigaDocumenti.FindAsync(id);

            if (riga == null)
            {
                return NotFound(new { message = "Riga documento non trovata" });
            }

            return Ok(riga);
        }

        // POST: api/righe-documento/{documentId}
        [HttpPost("{documentId}")]
        public async Task<ActionResult<RigaDocumento>> CreateRigaDocumento(int documentId, [FromBody] RigaDocumento riga)
        {
            // Trova il documento a cui associare la riga
            var documento = await _context.TestataDocumenti
                .Include(d => d.RigaDocumento)  // Includiamo le righe del documento
                .FirstOrDefaultAsync(d => d.DocumentId == documentId);

            if (documento == null)
            {
                return NotFound(new { message = "Documento non trovato" });
            }

            riga.DocumentoId = documentId;
            _context.RigaDocumenti.Add(riga);
            documento.RigaDocumento.Add(riga);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRigaDocumento), new { id = riga.RigaId }, riga);
        }

        // PUT: api/righe-documento/{rigaId}
        [HttpPut("{rigaId}")]
        public async Task<IActionResult> UpdateRigaDocumento(int rigaId, [FromBody] RigaDocumento updatedRiga)
        {

            //necessario idDocumento per collegare la riga al documento, non necessario idRiga perchè lo prende dall'url

            var riga = await _context.RigaDocumenti.FindAsync(rigaId);
            if (riga == null)
            {
                return NotFound(new { message = "Riga non trovata" });
            }

            riga.Descrizione = updatedRiga.Descrizione;
            riga.Quantita = updatedRiga.Quantita;

            await _context.SaveChangesAsync();

            return Ok("Riga aggiornata con successo");
        }

        // DELETE: api/righe-documento/{rigaId}
        [HttpDelete("{rigaId}")]
        public async Task<IActionResult> DeleteRigaDocumento(int rigaId)
        {

            //necessario solo idRiga

            var riga = await _context.RigaDocumenti.FindAsync(rigaId);
            if (riga == null)
            {
                return NotFound(new { message = "Riga non trovata" });
            }

            _context.RigaDocumenti.Remove(riga);
            await _context.SaveChangesAsync();

            return Ok("Riga eliminata con successo");
        }
    }
}
