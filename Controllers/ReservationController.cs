using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Domain;
using dogsitting_backend.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dogsitting_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private ReservationService ReservationService;
        public ReservationController(ReservationService reservationService)
        {
            this.ReservationService = reservationService;
        }


        [HttpGet(Name = "GetReservation")]
        public async Task<ActionResult> Get()
        {
            var test = await this.ReservationService.GetReservations();
            //List<Reservation> reservations = this.ReservationService.GetReservationsByUserId(null).Result.ToList();
            //List<Reservation> reservations = this.ReservationService.GetReservations().Result.ToList();
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string json = JsonConvert.SerializeObject(test.ToList(), settings);
            return Ok(json);
        }


        [HttpPost(Name = "PostReservation")]
        public async Task<ActionResult> Post(Reservation reservation)
        {
            return Ok();
        }
    }
}
