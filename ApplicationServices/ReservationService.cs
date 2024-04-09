using dogsitting_backend.Domain;
using dogsitting_backend.Infrastructure;
using MySql.Data.MySqlClient;

namespace dogsitting_backend.ApplicationServices
{
    public class ReservationService
    {
        //private readonly IGenericRepository<Reservation> _genericRepository;

        public ReservationService() { }
        //public ReservationService(IGenericRepository<Reservation> genereicRepository)
        //{
        //    this._genericRepository = genereicRepository;
        //}

        public void CreateReservation()
        {
            //check current logged in User.
            //get his team => get his calendar

            //Validate calendar is available on desired period.
            //  IF NOT propose another team WHO IS. => check other teams.

        }


    }
}
