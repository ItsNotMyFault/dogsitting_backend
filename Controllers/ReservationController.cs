using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Domain;
using dogsitting_backend.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities.IO;

namespace dogsitting_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        public ReservationController()
        {
        }


        [HttpGet(Name = "GetReservations")]
        public ActionResult Get()
        {
            return Ok(0);
        }


        [HttpPost(Name = "PostReservation")]
        public async Task<ActionResult> Post(Reservation reservation)
        {
            return Ok();
        }
    }
}
