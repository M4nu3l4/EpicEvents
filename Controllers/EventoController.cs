using EpicEvents.Data;
using EpicEvents.DTOs;
using EpicEvents.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EpicEvents.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<EventoDto>>> GetEventi()
        {
            var eventi = await _context.Eventi
                .Select(e => new EventoDto
                {
                    EventoId = e.EventoId,
                    Titolo = e.Titolo,
                    Data = e.Data,
                    Luogo = e.Luogo,
                    ArtistaId = e.ArtistaId
                }).ToListAsync();

            return Ok(eventi);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<EventoDto>> GetEvento(int id)
        {
            var evento = await _context.Eventi.FindAsync(id);
            if (evento == null) return NotFound();

            return Ok(new EventoDto
            {
                EventoId = evento.EventoId,
                Titolo = evento.Titolo,
                Data = evento.Data,
                Luogo = evento.Luogo,
                ArtistaId = evento.ArtistaId
            });
        }

        [HttpPost]
        [Authorize(Roles = "Amministratore")]
        public async Task<ActionResult> CreaEvento(EventoDto dto)
        {
            try
            {
                var evento = new Evento
                {
                    Titolo = dto.Titolo,
                    Data = dto.Data,
                    Luogo = dto.Luogo,
                    ArtistaId = dto.ArtistaId
                };

                _context.Eventi.Add(evento);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetEvento), new { id = evento.EventoId }, evento);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Errore nella creazione dell'evento");
                return StatusCode(500, "Errore interno durante la creazione.");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Amministratore")]
        public async Task<IActionResult> ModificaEvento(int id, EventoDto dto)
        {
            if (id != dto.EventoId) return BadRequest();

            try
            {
                var evento = await _context.Eventi.FindAsync(id);
                if (evento == null) return NotFound();

                evento.Titolo = dto.Titolo;
                evento.Data = dto.Data;
                evento.Luogo = dto.Luogo;
                evento.ArtistaId = dto.ArtistaId;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Errore nella modifica dell'evento");
                return StatusCode(500, "Errore interno durante l’aggiornamento.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Amministratore")]
        public async Task<IActionResult> EliminaEvento(int id)
        {
            try
            {
                var evento = await _context.Eventi.FindAsync(id);
                if (evento == null) return NotFound();

                _context.Eventi.Remove(evento);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Errore nell'eliminazione dell'evento");
                return StatusCode(500, "Errore interno durante l’eliminazione.");
            }
        }
    }
}
