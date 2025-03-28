using EpicEvents.Data;
using EpicEvents.DTOs;
using EpicEvents.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;

namespace EpicEvents.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BigliettoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BigliettoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("biglietti")]
        [Authorize]
        public async Task<IActionResult> AcquistaBiglietto([FromBody] AcquistoBigliettoDto dto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var evento = await _context.Eventi.FindAsync(dto.EventoId);
                if (evento == null)
                    return NotFound("Evento non trovato");

                for (int i = 0; i < dto.Quantita; i++)
                {
                    var biglietto = new Biglietto
                    {
                        EventoId = dto.EventoId,
                        UserId = userId,
                        DataAcquisto = DateTime.UtcNow
                    };

                    _context.Biglietti.Add(biglietto);
                }

                await _context.SaveChangesAsync();

                return Ok("Biglietto acquistato");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Errore durante l'acquisto del biglietto per evento {EventoId}", dto.EventoId);
                return StatusCode(500, "Errore durante l'acquisto");
            }
        }


        [HttpGet("miei")]
        [Authorize(Roles = "Utente")]
        public async Task<ActionResult> GetMieiBiglietti()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var biglietti = await _context.Biglietti
                .Include(b => b.Evento)
                .Where(b => b.UserId == userId)
                .ToListAsync();

            return Ok(biglietti);
        }

        [HttpGet("venduti")]
        [Authorize(Roles = "Amministratore")]
        public async Task<ActionResult> GetBigliettiVenduti()
        {
            var biglietti = await _context.Biglietti
                .Include(b => b.Evento)
                .Include(b => b.User)
                .ToListAsync();

            return Ok(biglietti);
        }
    }
}
