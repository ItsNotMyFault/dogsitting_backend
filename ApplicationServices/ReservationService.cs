using dogsitting_backend.Domain;
using dogsitting_backend.Infrastructure;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<Reservation>> GetReservations()
        {

            //check current logged in User.
            //get his team => get his calendar


            var calendars = this._calendarGenereicRepository.GetAll();
            var testCalendar = calendars.First();
            List<Reservation> reservations = await this.ReservationSQLRepository.GetReservationsByCalendarIdAsync(testCalendar.Id);
            return reservations;

        }


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

            //Validate calendar is available on desired period.
            //  IF NOT propose another team WHO IS. => check other teams.

        }


    }
}
