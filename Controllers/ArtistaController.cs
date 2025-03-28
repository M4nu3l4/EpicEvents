using EpicEvents.Data;
using EpicEvents.DTOs;
using EpicEvents.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EpicEvents.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Amministratore")]
    public class ArtistaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ArtistaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ArtistaDto>>> GetArtisti()
        {
            var artisti = await _context.Artisti
                .Select(a => new ArtistaDto
                {
                    ArtistaId = a.ArtistaId,
                    Nome = a.Nome,
                    Genere = a.Genere,
                    Biografia = a.Biografia
                }).ToListAsync();

            return Ok(artisti);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArtistaDto>> GetArtista(int id)
        {
            var artista = await _context.Artisti.FindAsync(id);
            if (artista == null) return NotFound();

            return Ok(new ArtistaDto
            {
                ArtistaId = artista.ArtistaId,
                Nome = artista.Nome,
                Genere = artista.Genere,
                Biografia = artista.Biografia
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreaArtista(ArtistaDto dto)
        {
            try
            {
                var artista = new Artista
                {
                    Nome = dto.Nome,
                    Genere = dto.Genere,
                    Biografia = dto.Biografia
                };

                _context.Artisti.Add(artista);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetArtista), new { id = artista.ArtistaId }, artista);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Errore nella creazione dell'artista");
                return StatusCode(500, "Errore interno durante la creazione.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ModificaArtista(int id, ArtistaDto dto)
        {
            if (id != dto.ArtistaId) return BadRequest();

            try
            {
                var artista = await _context.Artisti.FindAsync(id);
                if (artista == null) return NotFound();

                artista.Nome = dto.Nome;
                artista.Genere = dto.Genere;
                artista.Biografia = dto.Biografia;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Errore nella modifica dell'artista");
                return StatusCode(500, "Errore interno durante l’aggiornamento.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminaArtista(int id)
        {
            try
            {
                var artista = await _context.Artisti.FindAsync(id);
                if (artista == null) return NotFound();

                _context.Artisti.Remove(artista);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Errore nell'eliminazione dell'artista");
                return StatusCode(500, "Errore interno durante l’eliminazione.");
            }
        }
    }
}
