﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageEs.Data;
using StageEs.Models;
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
            var documento = await _context.TestataDocumenti
                .Include(d => d.RigaDocumento)
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
            Console.WriteLine($"Riga ID: {rigaId}");
            Console.WriteLine($"Descrizione: {updatedRiga.Descrizione}, Quantità: {updatedRiga.Quantita}");

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


        // PATCH: api/righe-documento/{rigaId}
        [HttpPatch("{rigaId}")]
        public async Task<IActionResult> UpdateCampoRiga(int rigaId, [FromBody] UpdateCampoRequest request)
        {
            var riga = await _context.RigaDocumenti.FindAsync(rigaId);

            if (riga == null)
                return NotFound(new { message = "Riga documento non trovata" });

            switch (request.Campo.ToLower())
            {
                case "descrizione":

                    if (request.Valore is string descrizione)
                        riga.Descrizione = descrizione;
                    else
                        return BadRequest(new { message = "Il campo descrizione deve essere una stringa" });

                    break;

                case "quantita":

                    if (request.Valore is int quantita)
                        riga.Quantita = quantita;                    
                    else
                        return BadRequest(new { message = "Il campo quantità deve essere un numero" });

                    break;

                default:
                    return BadRequest(new { message = "Campo non valido" });

            }

            await _context.SaveChangesAsync();

            return Ok("Riga aggiornata con successo");

        }



        // DELETE: api/righe-documento/{rigaId}
        [HttpDelete("{rigaId}")]
        public async Task<IActionResult> DeleteRigaDocumento(int rigaId)
        {
            var riga = await _context.RigaDocumenti.FindAsync(rigaId);
            if (riga == null)
            {
                return NotFound(new { message = "Riga non trovata" });
            }

            _context.RigaDocumenti.Remove(riga);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Riga eliminata con successo" });
        }

    }
}
