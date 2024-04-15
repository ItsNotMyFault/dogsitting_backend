using dogsitting_backend.Domain;
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

        public async Task<Calendar> GetReservations(string param)
        {

            //check current logged in User.
            //get his team => get his calendar
            //var calendars = this._calendarGenereicRepository.GetAll();

            //works
            List<Calendar> calendars = this._calendarGenereicRepository.Build()
                .Include(t => t.Team)
                .Include(t => t.Reservations)
                .ThenInclude(t => t.Client).ToList();
            //var calendars = this._calendarGenereicRepository.Build().Include(t => t.Team).ThenInclude(t => t.Admins);
            //var calendars = this._calendarGenereicRepository.Build().Include("Reservations").ToList();
            Calendar testCalendar = calendars.First();
            testCalendar.GetArrivalDepartureEvents();
            testCalendar.GetBusyEvents();


            //TODO breaks HERE
            //List<Reservation> reservations = await this.ReservationSQLRepository.GetReservationsByCalendarIdAsync(testCalendar.Id);

            //List<CalendarEvent> events = [];
            //reservations.ForEach(reservation =>
            //{
            //    events.AddRange(reservation.GetEvents());
            //});


            return testCalendar;

        }

        //OPTIONS

        //itérer sur tous les journées CalendarEvent et faire le +X selon le lodgerCount.
        //ça implique de supprimer les événements en double.


        public async Task<IEnumerable<Reservation>> GetReservationsByUserId(string userId)
        {
            userId = "e0b2801d-f67c-11ee-a26a-00155dd4f39d";
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
