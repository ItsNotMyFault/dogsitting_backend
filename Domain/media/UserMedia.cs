using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dogsitting_backend.Domain.media
{
    public class UserMedia
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }

        public Guid MediaId { get; set; }
        public Media Media { get; set; }
    }
}
