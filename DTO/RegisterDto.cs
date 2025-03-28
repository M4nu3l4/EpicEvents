namespace EpicEvents.DTOs
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Ruolo { get; set; } // "Utente" o "Amministratore"
    }
}
