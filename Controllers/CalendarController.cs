using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Domain;
using dogsitting_backend.Infrastructure;
using Microsoft.AspNetCore.Authorization;
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
    public class CalendarController : ControllerBase
    {
        private ReservationService ReservationService;
        public CalendarController(ReservationService reservationService, IHttpContextAccessor httpContextAccessor)
        {
            this.ReservationService = reservationService;
            var claimsPrincipal = httpContextAccessor.HttpContext.User;
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("team/{team}")]
        public async Task<ActionResult> Get([FromRoute] string team)
        {
            //param => mode client, mode admin (default selon le login)

            //param mode busy-available /  mode departure events / mode list reservations (default).

            var test = await this.ReservationService.GetCalendar(team);
            //List<Reservation> reservations = this.ReservationService.GetReservationsByUserId(null).Result.ToList();
            //List<Reservation> reservations = this.ReservationService.GetReservations().Result.ToList();
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string json = JsonConvert.SerializeObject(test, settings);
            return Ok(json);

        }
    }
}
