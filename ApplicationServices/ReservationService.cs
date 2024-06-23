using dogsitting_backend.ApplicationServices.dto;
using dogsitting_backend.ApplicationServices.response;
using dogsitting_backend.Domain;
using dogsitting_backend.Domain.auth;
using dogsitting_backend.Domain.media;
using dogsitting_backend.Domain.repositories;
using dogsitting_backend.Infrastructure;

namespace dogsitting_backend.ApplicationServices
{
    public class ReservationService
    {
        private readonly IReservationRepository ReservationSQLRepository;
        private readonly AuthService _userService;
        private readonly CalendarService _calendarService;
        private readonly TeamService _teamService;

        public ReservationService(
            AuthService userService,
            CalendarService calendarService,
            TeamService teamService,
            IReservationRepository reservationRepository
        )
        {

            this._calendarService = calendarService;
            this._userService = userService;
            this._teamService = teamService;
            this.ReservationSQLRepository = reservationRepository;
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

        public async Task<ReservationResponse> FindReservationResponse(Guid ReservationId)
        {
            Reservation reservation = await this.ReservationSQLRepository.GetByIdAsync(ReservationId);
            return new ReservationResponse(reservation);
        }

        public async Task<ReservationResponse> FindReservationMediaResponse(Guid ReservationId)
        {
            Reservation reservation = await this.ReservationSQLRepository.GetByIdAsync(ReservationId);
            return new ReservationResponse(reservation);
        }

        public async Task ApproveReservation(Guid ReservationId)
        {
            Reservation reservation = await this.ReservationSQLRepository.GetByIdAsync(ReservationId);
            reservation.ApprovedAt = DateTime.Now.ToUniversalTime();
            await this.ReservationSQLRepository.UpdateAsync(reservation);
        }

        public async Task<ReservationResponse> AddReservationToTeamCalendar(ReservationDto reservationDto, string teamName)
        {
            if (reservationDto == null)
            {
                throw new ArgumentNullException(nameof(reservationDto));
            }
            if (reservationDto.LodgerCount == 0)
            {
                throw new Exception("LodgerCount parameter must be higher than 0");
            }
            AuthUser user = this._userService.GetCurrentUserAsync().Result;


            Team team = await this._teamService.GetTeamByNormalizedName(teamName);

            Domain.calendar.Calendar calendar = await this._calendarService.GetTeamCalendar(teamName);

            calendar.TeamId = team.Id;
            Reservation reservation = new(calendar)
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

            await this.ReservationSQLRepository.AddAsync(reservation);
            return new ReservationResponse(reservation);
            //TODO
            //Validate calendar is available on desired period.
            //  IF NOT propose another team WHO IS. => check other teams.
            //Redirect to new page? loads each team's availabilities in different colors?
        }

        public async Task Delete(Guid Id)
        {
            Reservation reservation = await this.ReservationSQLRepository.GetByIdAsync(Id);
            if (reservation == null)
            {
                throw new Exception("Reservation not found.");
            }
            await this.ReservationSQLRepository.DeleteAsync(reservation.Id);
        }

        public async Task<List<MediaResponse>> GetReservationMedias(Guid Id)
        {
            List<ReservationMedia> reservationMedias = await this.ReservationSQLRepository.GetMedias(Id);
            return reservationMedias.Select(x => new MediaResponse(x.Media)).ToList();
        }

        public async Task AddMediaToReservation(Guid Id, IEnumerable<IFormFile> mediaList)
        {
            Reservation reservation = await this.ReservationSQLRepository.GetByIdAsync(Id);
            if (reservation == null)
            {
                throw new Exception("Reservation not found.");
            }
            List<Media> medias = mediaList.Select(media => new Media(media)).ToList();
            foreach (Media media in medias)
            {
                await this.ReservationSQLRepository.LinkMediaAsync(Id, media);
            }
        }

        public async Task RemoveMediaFromReservation(Guid reservationId, Guid mediaId)
        {

            await this.ReservationSQLRepository.UnlinkMediaAsync(reservationId, mediaId);
        }


    }
}
