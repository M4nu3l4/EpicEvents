using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EpicEvents.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Biglietto> Biglietti { get; set; }
    }
}
