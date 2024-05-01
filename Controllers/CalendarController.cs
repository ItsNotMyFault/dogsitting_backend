using dogsitting_backend.ApplicationServices;
using dogsitting_backend.Domain.calendar;
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
        private CalendarService calendarService;
        public CalendarController(ReservationService reservationService, CalendarService calendarService, IHttpContextAccessor httpContextAccessor)
        {
            this.ReservationService = reservationService;
            this.calendarService = calendarService;
            var claimsPrincipal = httpContextAccessor.HttpContext.User;
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("team/{team}/arrivaldepartures")]
        public async Task<ActionResult> GetArrivalDepartures([FromRoute] string team)
        {
            List<CalendarEvent> departureEvents = await this.calendarService.GetCalendarDepartureEvents(team);
            List<CalendarEvent> arrivalEvents = await this.calendarService.GetCalendarArrivalEvents(team);
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string json = JsonConvert.SerializeObject(new
            {
                DepartureEvents = departureEvents,
                ArrivalEvents = arrivalEvents
            }, settings);
            return Ok(json);

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("team/{team}/busyevents")]
        public async Task<ActionResult> GetBusyEvents([FromRoute] string team)
        {
            //param => mode client, mode admin (default selon le login)

            //param mode busy-available /  mode departure events / mode list reservations (default).

            List<BusyCalendarEvent> events = await this.calendarService.GetCalendarBusyEvents(team);
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string json = JsonConvert.SerializeObject(events, settings);
            return Ok(json);

        }
        //
    }
}
