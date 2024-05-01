using dogsitting_backend.Domain;
using dogsitting_backend.Domain.auth;
using dogsitting_backend.Domain.calendar;
using dogsitting_backend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace dogsitting_backend.ApplicationServices
{
    public class CalendarService
    {
        private readonly IGenericRepository<Calendar> _calendarGenereicRepository;

        public CalendarService() { }
        public CalendarService(IGenericRepository<Calendar> calendarGenereicRepository)
        {

            this._calendarGenereicRepository = calendarGenereicRepository;
        }

        /// <summary>
        /// Get a Team's calendar with all it's corresponding reservations
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<Calendar> GetTeamCalendar(string param)
        {
            List<Calendar> calendars = this._calendarGenereicRepository.Build()
            .Include(calendar => calendar.Team)
            .ThenInclude(team => team.Admins)
            .Include(calendar => calendar.Reservations)
            .ThenInclude(reservation => reservation.Client).ToList();
            Calendar testCalendar = calendars.First();
            return testCalendar;
        }

        public async Task<List<Calendar>> GetAllCalendars()
        {
            List<Calendar> calendars = this._calendarGenereicRepository.Build()
            .Include(calendar => calendar.Team)
            .ThenInclude(team => team.Admins)
            .Include(calendar => calendar.Reservations)
            .ThenInclude(reservation => reservation.Client).ToList();
            return calendars;
        }

        public async Task<List<BusyCalendarEvent>> GetCalendarBusyEvents(string team)
        {
            Calendar calendar = await this.GetTeamCalendar(team);
            List<BusyCalendarEvent> events = calendar.GetBusyEvents();
            return events;

        }

        public async Task<List<CalendarEvent>> GetCalendarArrivalEvents(string team)
        {
            Calendar calendar = await this.GetTeamCalendar(team);
            List<CalendarEvent> events = calendar.GetArrivalEvents();
            return events;
        }

        public async Task<List<CalendarEvent>> GetCalendarDepartureEvents(string team)
        {
            Calendar calendar = await this.GetTeamCalendar(team);
            List<CalendarEvent> events = calendar.GetDepartureEvents();
            return events;
        }

        //OPTIONS

        //itérer sur tous les journées CalendarEvent et faire le +X selon le lodgerCount.
        //ça implique de supprimer les événements en double.


    }
}
