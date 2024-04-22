using dogsitting_backend.Domain;
using dogsitting_backend.Domain.calendar;
using dogsitting_backend.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace dogsitting_backend.ApplicationServices
{
    public class ReservationService
    {
        private readonly IGenericRepository<Reservation> _genericRepository;
        private readonly IGenericRepository<Calendar> _calendarGenereicRepository;
        private readonly ReservationSQLRepository ReservationSQLRepository;

        public ReservationService() { }
        public ReservationService(IGenericRepository<Reservation> genereicRepository, IGenericRepository<Calendar> calendarGenereicRepository, ReservationSQLRepository reservationSQLRepository)
        {
            this._genericRepository = genereicRepository;
            this._calendarGenereicRepository = calendarGenereicRepository;
            this.ReservationSQLRepository = reservationSQLRepository;
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


        public async Task<IEnumerable<Reservation>> GetReservationsByUserId(string userId)
        {
            return await this.ReservationSQLRepository.GetReservationsByUserIdAsync(Guid.Parse(userId));

        }


        public void CreateReservation()
        {
            //check current logged in User.
            //get his team => get his calendar

            //Validate calendar is available on desired period.
            //  IF NOT propose another team WHO IS. => check other teams.


        }


    }
}
