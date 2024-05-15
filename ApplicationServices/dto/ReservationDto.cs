using dogsitting_backend.Domain;
using Google.Protobuf.WellKnownTypes;
using Humanizer;
using NuGet.Protocol.Core.Types;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Composition;
using System.Net.NetworkInformation;
using System;
using System.ComponentModel.DataAnnotations;
namespace dogsitting_backend.ApplicationServices.dto
{
    public class ReservationDto
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int LodgerCount { get; set; }
        public string Notes{ get; set; }
    }
}
