using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dogsitting_backend.Domain.auth
{
    [PrimaryKey(nameof(UserId), nameof(LoginProvider))]
    public class UserToken
    {
        [Key]
        public Guid UserId { get; set; }
        [Key]
        public string LoginProvider { get; set; }
        public string Name { get; set; }
        public string TokenValue { get; set; }
        
    }
}
