using dogsitting_backend.ApplicationServices;
using dogsitting_backend.ApplicationServices.dto;
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

        [HttpGet("{id}", Name = "AReservation")]
        public async Task<ActionResult> GetReservationById([FromRoute] Guid id)
        {
            ReservationResponse reservation = (await this.ReservationService.FindReservation(id));

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            string json = JsonConvert.SerializeObject(reservation, settings);
            return Ok(json);
        }

        [HttpGet("User/{id}", Name = "UserReservations")]
        public async Task<ActionResult> GetUserReservations([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Error forming request getting reservations of user {id}");
            }
            List<ReservationResponse> reservations = (await this.ReservationService.GetReservationsByUserId(id)).ToList();

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            string json = JsonConvert.SerializeObject(reservations, settings);
            return Ok(json);
        }


        [HttpGet("Team/{teamName}")]
        public async Task<ActionResult> GetTeamReservations([FromRoute] string teamName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Error forming request getting reservations of team {teamName}");
            }
            List<ReservationResponse> reservations = (await this.ReservationService.GetReservationsByTeamName(teamName)).ToList();

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            string json = JsonConvert.SerializeObject(reservations, settings);
            return Ok(json);
        }


        [HttpPost("{team}", Name = "CreateReservation")]
        public async Task<ActionResult> CreateReservation([FromBody] ReservationDto reservation, string team)
        {
            await this.ReservationService.CreateReservationForCurrentUser(reservation, team);
            return Ok();
        }

        [HttpPost("{Id}/approve", Name = "ApproveReservation")]
        public async Task<ActionResult> ApproveReservation([FromRoute] Guid Id)
        {

            await this.ReservationService.ApproveReservation(Id);
            return Ok();
        }

        [HttpDelete("{Id}", Name = "DeleteReservation")]
        public async Task<ActionResult> Delete([FromRoute] Guid Id)
        {
            await this.ReservationService.Delete(Id);
            //create a reservation for current logged in user.
            return Ok();
        }
    }
}
