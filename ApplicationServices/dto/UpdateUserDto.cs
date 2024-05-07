using dogsitting_backend.Domain;
using Google.Protobuf.WellKnownTypes;
using Humanizer;
using NuGet.Protocol.Core.Types;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Composition;
using System.Net.NetworkInformation;
using System;
namespace dogsitting_backend.ApplicationServices.dto
{
    public class UpdateUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
