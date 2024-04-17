using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace dogsitting_backend.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private ReservationService ReservationService;
        public ReservationController(ReservationService reservationService)
        {
            this.ReservationService = reservationService;
        }


        [HttpGet(Name = "Reservation")]
        public async Task<ActionResult> Get()
        {
            Calendar test = await this.ReservationService.GetReservations(null);
   
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            string json = JsonConvert.SerializeObject(test, settings);
            return Ok(json);
        }



        [HttpPost(Name = "PostReservation")]
        public async Task<ActionResult> Post(Reservation reservation)
        {
            return Ok();
        }
    }
}
