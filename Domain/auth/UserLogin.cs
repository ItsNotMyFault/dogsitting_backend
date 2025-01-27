using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dogsitting_backend.Domain.auth
{
    [PrimaryKey(nameof(ProviderKey), nameof(LoginProvider), nameof(UserId))]
    public class UserLogin
    {
        public UserLogin(string loginProvider, string providerKey, Guid UserId, string? displayName)
        {
            this.LoginProvider = loginProvider;
            this.ProviderKey = providerKey;
            this.UserId = UserId;
            if (!string.IsNullOrEmpty(displayName))
            {
                this.ProviderDisplayName = displayName;
            }
        }

        public UserLogin() { }

        [Key]
        public string LoginProvider { get; set; }
        [Key]
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
