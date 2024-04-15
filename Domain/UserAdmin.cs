using dogsitting_backend.Domain;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text.Json.Serialization;

namespace dogsitting_backend.Domain;

public class UserAdmin : ApplicationUser
{
    public UserAdmin()
    {

    }

}


