using dogsitting_backend.ApplicationServices.dto;
using dogsitting_backend.Domain;
using dogsitting_backend.Domain.auth;
using dogsitting_backend.Domain.calendar;
using dogsitting_backend.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace dogsitting_backend.ApplicationServices
{
    public class CalendarService
    {
        private readonly IGenericRepository<Calendar> _calendarGenereicRepository;
        private readonly CalendarSQLRepository _calendarSQLRepository;

        public CalendarService(IGenericRepository<Calendar> calendarGenereicRepository, CalendarSQLRepository calendarSQLRepository)
        {

            this._calendarGenereicRepository = calendarGenereicRepository;
            this._calendarSQLRepository = calendarSQLRepository;
        }


        public async Task<Calendar> GetCalendarById(Guid id)
        {
            return this._calendarGenereicRepository.Build().Where(c => c.Id == id).First();
        }

        /// <summary>
        /// Get a Team's calendar with all it's corresponding reservations
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<Calendar> GetTeamCalendar(string team)
        {
            Calendar calendar = this._calendarGenereicRepository.Build()
            .Include(calendar => calendar.Team)
            .ThenInclude(team => team.Admins)
            .Include(calendar => calendar.Reservations)
            .ThenInclude(reservation => reservation.Client)
            .Where(c => c.Team.NormalizedName == team).First();
            return calendar;
        }

        private async Task<List<Availability>> GetCalendarAvailabilities(Guid calendarId)
        {
            return await this._calendarSQLRepository.GetCalendarAvailabilities(calendarId);
        }

        public async Task<List<CalendarEvent>> GetReservationEvents(string team)
        {
            Calendar calendar = await this.GetTeamCalendar(team);
            return calendar.GetReservationsEvents();
        }

        public async Task<List<BusyCalendarEvent>> GetCalendarBusyEvents(string team)
        {
            Calendar calendar = await this.GetTeamCalendar(team);
            if (calendar == null)
            {
                throw new Exception($"Calendar not found for team: {team}");
            }
            calendar.Availabilities = await this.GetCalendarAvailabilities(calendar.Id);
            List<BusyCalendarEvent> events = calendar.GetComputedBusyEvents();
            return events;
        }

        public async Task<AvailabilitiesResponse> GetCalendarAvailabilitiesEvents(string team)
        {
            Calendar calendar = await this.GetTeamCalendar(team);
            if (calendar == null)
            {
                throw new Exception($"Calendar not found for team: {team}");
            }
            calendar.Availabilities = await this.GetCalendarAvailabilities(calendar.Id);
            List<BusyCalendarEvent> busyEvents = calendar.GetBusyEvents();
            List<AvailableCalendarEvent> availableEvents = calendar.GetAvailableEvents();
            return new AvailabilitiesResponse() { AvailableEvents = availableEvents, BusyEvents = busyEvents };
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

        public async Task AddAvailabilities(string team, List<AvailabilityDto> availabilities)
        {
            List<Availability>  availabilitieList = availabilities.Select(date => new Availability(date.Date, date.IsAvailable)).ToList();
            Calendar calendar = await this.GetTeamCalendar(team);

            availabilitieList.ForEach(availability =>
            {
                availability.CalendarId = calendar.Id;
            });
            await this._calendarSQLRepository.AddAvailabilities(availabilitieList);
        }

        public async Task DeleteAvailabilities(string team, List<AvailabilityDto> availabilityDtoList)
        {
            List<DateTime> availabilitieList = availabilityDtoList.Select(date => DateTime.Parse(date.Date)).ToList();
            
            Calendar calendar = await this.GetTeamCalendar(team);
            var availabilitieList2 = await this._calendarSQLRepository.FindAvailabilities(calendar.Id, availabilitieList);
            await this._calendarSQLRepository.DeleteAvailabilities(availabilitieList2);
        }

        public async Task UpdateCalendarAsync(Calendar calendar)
        {
            await this._calendarSQLRepository.Update(calendar);
        }

        //OPTIONS

        //itérer sur tous les journées CalendarEvent et faire le +X selon le lodgerCount.
        //ça implique de supprimer les événements en double.


    }
}
