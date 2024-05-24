using dogsitting_backend.ApplicationServices.dto;
using dogsitting_backend.Domain;
using dogsitting_backend.Domain.auth;
using dogsitting_backend.Infrastructure;
using System.Globalization;

namespace dogsitting_backend.ApplicationServices
{
    public class ReservationService
    {
        private readonly IGenericRepository<Reservation> _genericRepository;
        private readonly IGenericRepository<Domain.calendar.Calendar> _calendarGenereicRepository;
        private readonly ReservationSQLRepository ReservationSQLRepository;
        private readonly AuthService _userService;
        private readonly CalendarService _calendarService;
        private readonly TeamService _teamService;

        public ReservationService(
            AuthService userService,
            CalendarService calendarService,
            TeamService teamService,
            IGenericRepository<Reservation> genereicRepository,
            IGenericRepository<Domain.calendar.Calendar> calendarGenereicRepository,
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

        public async Task<IEnumerable<ReservationResponse>> GetReservationsByUserId(Guid userId)
        {
            List<Reservation> reservations = await this.ReservationSQLRepository.GetReservationsByUserIdAsync(userId);
            return reservations.Select(reservation => new ReservationResponse(reservation)).ToList().OrderByDescending(t => t.CreatedAt);
        }

        public async Task<IEnumerable<ReservationResponse>> GetReservationsByTeamName(string teamName)
        {
            Team team = await this._teamService.GetTeamByNormalizedName(teamName);
            List<Reservation> reservations = await this.ReservationSQLRepository.GetReservationsByTeamIdAsync(team.Id);
            return reservations.Select(reservation => new ReservationResponse(reservation)).ToList().OrderByDescending(t => t.CreatedAt);
        }

        public async Task<ReservationResponse> FindReservation(Guid ReservationId)
        {
            Reservation reservation = await this.ReservationSQLRepository.FindById(ReservationId);
            return new ReservationResponse(reservation);

        }

        public async Task ApproveReservation(Guid ReservationId)
        {
            Reservation reservation = await this.ReservationSQLRepository.FindById(ReservationId);
            await this.ReservationSQLRepository.Approve(reservation);
        }

        public async Task CreateReservationForCurrentUser(ReservationDto reservationDto, string teamName)
        {
            if(reservationDto == null)
            {
                throw new ArgumentNullException(nameof(reservationDto));
            }
            if(reservationDto.LodgerCount == 0)
            {
                throw new Exception("LodgerCount parameter must be higher than 0");
            }
            AuthUser user = this._userService.GetCurrentUserAsync().Result;


            Team team = await this._teamService.GetTeamByNormalizedName(teamName);

            Domain.calendar.Calendar calendar = await this._calendarService.GetTeamCalendar(teamName);

            calendar.TeamId = team.Id;
            Reservation reservation = new Reservation(calendar)
            {
                UserId = user.ApplicationUser.Id,
                Client = null,
                DateFrom = reservationDto.DateFrom,
                DateTo = reservationDto.DateTo,
                LodgerCount = reservationDto.LodgerCount,
                Notes = reservationDto.Notes,
            };
            calendar.Availabilities = await this._calendarService.GetCalendarAvailabilities(calendar.Id);
            calendar.ValidateReservation(reservation);

            await this.ReservationSQLRepository.Create(reservation);
            //TODO
            //Validate calendar is available on desired period.
            //  IF NOT propose another team WHO IS. => check other teams.
        }

        public async Task Delete(Guid Id)
        {
            Reservation reservation = await this.ReservationSQLRepository.FindById(Id);
            if(reservation == null)
            {
                throw new Exception("Reservation not found.");
            }
            await this.ReservationSQLRepository.Delete(reservation);
        }


    }
}
