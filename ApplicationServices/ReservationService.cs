using dogsitting_backend.Domain;
using dogsitting_backend.Domain.auth;
using dogsitting_backend.Domain.calendar;
using dogsitting_backend.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities.IO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace dogsitting_backend.ApplicationServices
{
    public class ReservationService
    {
        private readonly IGenericRepository<Reservation> _genericRepository;
        private readonly IGenericRepository<Calendar> _calendarGenereicRepository;
        private readonly ReservationSQLRepository ReservationSQLRepository;
        private readonly AuthService _userService;
        private readonly CalendarService _calendarService;
        private readonly TeamService _teamService;

        public ReservationService() { }
        public ReservationService(
            AuthService userService,
            CalendarService calendarService,
            TeamService teamService,
            IGenericRepository<Reservation> genereicRepository,
            IGenericRepository<Calendar> calendarGenereicRepository,
            ReservationSQLRepository reservationSQLRepository
        )
        {

            this._calendarService = calendarService;
            this._userService = userService;
            this._teamService = teamService;
            this._genericRepository = genereicRepository;
            this._calendarGenereicRepository = calendarGenereicRepository;
            this.ReservationSQLRepository = reservationSQLRepository;
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByUserId(Guid userId)
        {
            return await this.ReservationSQLRepository.GetReservationsByUserIdAsync(userId);

        }

        public async Task<IEnumerable<TeamReservationResponse>> GetReservationsByTeamName(string teamName)
        {
            Team team = await this._teamService.GetTeamByNormalizedName(teamName);
            List<Reservation> reservations = await this.ReservationSQLRepository.GetReservationsByTeamIdAsync(team.Id);

            List<TeamReservationResponse> teamReservations = reservations.Select(reservation => new TeamReservationResponse(reservation)).ToList();
            return teamReservations;

        }


        public async Task Create(Reservation reservation, string teamName)
        {
            //reservation.Id = Guid.NewGuid();
            AuthUser user = this._userService.GetCurrentUserAsync().Result;
            if (reservation.Client.Id != user.ApplicationUser.Id)
            {
                throw new Exception("Cannot create a reservation for another user than yourself.");
            }
            reservation.UserId = user.ApplicationUser.Id;
            reservation.Client = null;

            Team team = await this._teamService.GetTeamByNormalizedName(teamName);

            Calendar calendar = await this._calendarService.GetTeamCalendar(teamName);
            calendar.TeamId = team.Id;
            reservation.CalendarId = calendar.Id;
            
            await this.ReservationSQLRepository.Create(reservation);
            //check current logged in User.
            //get his team => get his calendar

            //Validate calendar is available on desired period.
            //  IF NOT propose another team WHO IS. => check other teams.
        }

        public async Task Delete(Guid Id)
        {
            Reservation reservation = await this.ReservationSQLRepository.FindById(Id);
            await this.ReservationSQLRepository.Delete(reservation);
        }


    }
}
