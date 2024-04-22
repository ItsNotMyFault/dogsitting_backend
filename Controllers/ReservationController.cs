using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Domain;
using dogsitting_backend.Domain.auth;
using dogsitting_backend.Domain.calendar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace dogsitting_backend.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private ReservationService ReservationService;
        private AuthUser _authUser;
        public ReservationController(ReservationService reservationService, UserManager<AuthUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.ReservationService = reservationService;
            var claimsPrincipal = httpContextAccessor.HttpContext.User;
            this._authUser = userManager.GetUserAsync(claimsPrincipal).Result;
        }


        [HttpGet(Name = "Reservation")]
        public async Task<ActionResult> Get()
        {
            List<Reservation> reservations = (await this.ReservationService.GetReservationsByUserId(this._authUser.ApplicationUser.Id.ToString())).ToList();
   
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            string json = JsonConvert.SerializeObject(reservations, settings);
            return Ok(json);
        }



        [HttpPost(Name = "PostReservation")]
        public async Task<ActionResult> Post(Reservation reservation)
        {
            //create a reservation for current logged in user.
            return Ok();
        }
    }
}
