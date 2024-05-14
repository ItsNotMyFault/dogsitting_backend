using dogsitting_backend.ApplicationServices;
using dogsitting_backend.ApplicationServices.dto;
using dogsitting_backend.Domain;
using dogsitting_backend.Domain.calendar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace dogsitting_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalendarController : ControllerBase
    {
        private CalendarService calendarService;
        public CalendarController(CalendarService calendarService, IHttpContextAccessor httpContextAccessor)
        {
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
            List<BusyCalendarEvent> events = await this.calendarService.GetCalendarBusyEvents(team);
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string json = JsonConvert.SerializeObject(events, settings);
            return Ok(json);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("team/{team}/availableevents")]
        public async Task<ActionResult> GetAvailableEvents([FromRoute] string team)
        {
            AvailabilitiesResponse availabilitiesResponse = await this.calendarService.GetCalendarAvailabilitiesEvents(team);
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string json = JsonConvert.SerializeObject(availabilitiesResponse, settings);
            return Ok(json);
        }

        //TODO
        //lister les dates avec réservations (full ou busy)
        //lister les dates disponibles
        //lister les dates non disponibles.
        //permettre à l'admin de retirer/ajouter des dates dispos ou non-dispos
        //======================
        //BusyEvetn (background color) pour des réservations avec calcul des lodgercount.
        //availableEvent (background color) pour des availabilities
        //ReservationEvent (labeled event) pour des réservations



        [HttpPost]
        [AllowAnonymous]
        [Route("team/{team}/availabilities")]
        public async Task<ActionResult> AddAvailabilityEvents([FromRoute] string team, [FromBody] List<AvailabilityDto> dates)
        {
            await this.calendarService.AddAvailabilities(team, dates);
            return Ok();
        }

        [HttpPut]
        [AllowAnonymous]
        [Route("team/{team}/availabilities")]
        public async Task<ActionResult> DeleteAvailabilityEvents([FromRoute] string team, [FromBody] List<AvailabilityDto> dates)
        {
            await this.calendarService.DeleteAvailabilities(team, dates);
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("team/{team}/reservationevents")]
        public async Task<ActionResult> GetReservationsEvents([FromRoute] string team)
        {
            List<CalendarEvent> events = await this.calendarService.GetReservationEvents(team);
            JsonSerializerSettings settings = new()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string json = JsonConvert.SerializeObject(events, settings);
            return Ok(json);

        }

    }
}
