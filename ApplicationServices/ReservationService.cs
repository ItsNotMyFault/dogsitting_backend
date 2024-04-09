using dogsitting_backend.Domain;
using dogsitting_backend.Infrastructure;
using MySql.Data.MySqlClient;

namespace dogsitting_backend.ApplicationServices
{
    public class ReservationService
    {
        private readonly IGenericRepository<Reservation> _genericRepository;
        private readonly ReservationSQLRepository ReservationSQLRepository;

        public ReservationService() { }
        public ReservationService(IGenericRepository<Reservation> genereicRepository, ReservationSQLRepository reservationSQLRepository)
        {
            this._genericRepository = genereicRepository;
            this.ReservationSQLRepository = reservationSQLRepository;
        }

        public async Task<IEnumerable<Reservation>> GetReservations()
        {

            //check current logged in User.
            //get his team => get his calendar

            //Validate calendar is available on desired period.
            //  IF NOT propose another team WHO IS. => check other teams.
            //return await this._genericRepository.GetAllAsync();

            return await this.ReservationSQLRepository.GetAllReservationsAsync();

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
