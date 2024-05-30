using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dogsitting_backend.Domain.media
{
    public class TeamMedia
    {
        public Guid TeamId { get; set; }
        public Team Team { get; set; }

        public Guid MediaId { get; set; }
        public Media Media { get; set; }

        public int Position{ get; set; }
    }
}
