using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageEs.Data;
using StageEs.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StageEs.Controllers
{
    [Route("api/documenti")]
    [ApiController]
    public class DocumentoController : ControllerBase
    {
        private readonly AziendaDbContext _context;

        public DocumentoController(AziendaDbContext context)
        {
            _context = context;
        }

        // GET: api/documenti
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestataDocumento>>> GetAllDocumenti()
        {
            var documenti = await _context.TestataDocumenti
                                           .Include(r => r.RigaDocumento)  // Include anche le righe
                                           .ToListAsync();

            if (!documenti.Any())
            {
                return NotFound("Nessun documento trovato.");
            }

            return Ok(documenti);
        }

        // GET: api/documenti/filter
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<TestataDocumento>>> FilteredGetAllDocumenti(
            [FromQuery] DateTime? DataDocumento, [FromQuery] string? NumeroDocumento, [FromQuery] string? RagioneSociale)
        {
            var query = _context.TestataDocumenti
                                .Include(d => d.RigaDocumento)
                                .Include(d => d.Customer)
                                .AsQueryable();

            if (DataDocumento.HasValue)
            {
                query = query.Where(d => d.DataDocumento.Date >= DataDocumento.Value.Date);
            }

            if (!string.IsNullOrEmpty(NumeroDocumento))
            {
                query = query.Where(d => d.NumeroDocumento.Contains(NumeroDocumento));
            }

            if (!string.IsNullOrEmpty(RagioneSociale))
            {
                query = query.Where(d => d.Customer.RagioneSociale.Contains(RagioneSociale));
            }

            var documenti = await query.ToListAsync();

            if (!documenti.Any())
            {
                return NotFound(new { message = "Nessun documento trovato con i criteri specificati." });
            }

            return Ok(documenti);
        }

        // GET: api/documenti/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TestataDocumento>> GetDocumento(int id)
        {
            var documento = await _context.TestataDocumenti
                .Include(r => r.RigaDocumento)
                .FirstOrDefaultAsync(d => d.DocumentId == id);

            if (documento == null)
            {
                return NotFound(new { message = "Documento non trovato" });
            }

            return Ok(documento);
        }

        // POST: api/documenti
        [HttpPost]
        public async Task<ActionResult<TestataDocumento>> CreateDocumento([FromBody] CreateDocumentoDTO dto)
        {
            var customerExists = await _context.Customers.AnyAsync(c => c.CustomerId == dto.CustomerId);
            if (!customerExists)
            {
                return BadRequest(new { message = "Il cliente specificato non esiste" });
            }

            var documento = new TestataDocumento
            {
                DataDocumento = dto.DataDocumento,
                NumeroDocumento = dto.NumeroDocumento,
                CustomerId = dto.CustomerId,
                RigaDocumento = dto.Righe?.Select(r => new RigaDocumento
                {
                    Descrizione = r.Descrizione,
                    Quantita = r.Quantita
                }).ToList()
            };

            _context.TestataDocumenti.Add(documento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDocumento), new { id = documento.DocumentId }, documento);
        }

        // PUT: api/documenti/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocumento(int id, [FromBody] DocumentoUpdateModel updatedDocumento)
        {
            var documento = await _context.TestataDocumenti.FindAsync(id);

            if (documento == null)
            {
                return NotFound(new { message = "Documento non trovato" });
            }

            documento.NumeroDocumento = updatedDocumento.NumeroDocumento;
            documento.DataDocumento = updatedDocumento.DataDocumento;
            documento.CustomerId = updatedDocumento.CustomerId;

            await _context.SaveChangesAsync();
            return Ok("Documento aggiornato con successo");
        }


        // PATCH: api/documenti/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateDocumentoCampo(int id, [FromBody] UpdateCampoRequest request)
        {
            var documento = await _context.TestataDocumenti.FindAsync(id);
            if (documento == null)
            {
                return NotFound(new { message = "Documento non trovato" });
            }

            switch (request.Campo.ToLower())
            {
                case "data":
                    if (request.Valore is string dataString)
                    {
                        if (DateTime.TryParse(dataString, out DateTime data))
                            documento.DataDocumento = data;
                        else
                            return BadRequest(new { message = "Formato della data non valido" });
                    }
                    else
                        return BadRequest(new { message = "Il campo data deve essere una stringa" });

                    break;

                case "numero":
                    if (request.Valore is string numeroString)
                        documento.NumeroDocumento = numeroString;
                    else
                        return BadRequest(new { message = "Il campo numero deve essere una stringa" });

                    break;

                case "cliente":
                    if (request.Valore is int customerId)
                        documento.CustomerId = customerId;
                    else
                        return BadRequest(new { message = "Il campo cliente deve essere un ID numerico" });

                    break;

                default:
                    return BadRequest(new { message = "Campo non valido" });
            }

            await _context.SaveChangesAsync();
            return Ok("Documento aggiornato con successo");
        }

        // DELETE: api/documenti/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocumento(int id)
        {
            var documento = await _context.TestataDocumenti
                                          .Include(d => d.RigaDocumento)
                                          .FirstOrDefaultAsync(d => d.DocumentId == id);

            if (documento == null)
            {
                return NotFound(new { message = "Documento non trovato" });
            }

            if (documento.RigaDocumento != null && documento.RigaDocumento.Any())
            {
                _context.RigaDocumenti.RemoveRange(documento.RigaDocumento);
            }

            _context.TestataDocumenti.Remove(documento);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Documento e relative righe eliminati con successo" });
        }

    }
}
