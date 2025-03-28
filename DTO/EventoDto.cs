namespace EpicEvents.DTOs
{
    public class EventoDto
    {
        public int EventoId { get; set; }
        public string Titolo { get; set; }
        public DateTime Data { get; set; }
        public string Luogo { get; set; }
        public int ArtistaId { get; set; }
    }
}
