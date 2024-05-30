using dogsitting_backend.Domain;
using dogsitting_backend.Domain.media;
using dogsitting_backend.Domain.repositories;
using Microsoft.EntityFrameworkCore;

namespace dogsitting_backend.Infrastructure
{
    public class ReservationSQLRepository : IReservationRepository
    {
        private DogsittingDBContext Context { get; set; }
        public MediaSQLRepository MediaRepository { get; set; }

        public ReservationSQLRepository(DogsittingDBContext context, MediaSQLRepository mediaRepository)
        {
            this.Context = context;
            this.MediaRepository = mediaRepository;
        }


        public Task<Reservation> GetByIdAsync(Guid id)
        {
            return this.Context.Reservations.Where(reservation => reservation.Id == id).Include(reserv => reserv.Calendar).ThenInclude(calendar => calendar.Team).ThenInclude(team => team.Admins).FirstAsync();
        }

        public async Task<List<Reservation>> GetAllAsync()
        {
            return await this.Context.Reservations.Include(reserv => reserv.Calendar).ThenInclude(calendar => calendar.Team).ThenInclude(team => team.Admins).ToListAsync();
        }

        public async Task AddAsync(Reservation entity)
        {
            entity.CreatedAt = DateTime.Now.ToUniversalTime();
            this.Context.Reservations.Add(entity);
            await this.Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reservation entity)
        {
            entity.ApprovedAt = DateTime.Now.ToUniversalTime();
            this.Context.Reservations.Update(entity);
            await this.Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            Reservation reservation = await this.Context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                this.Context.Reservations.Remove(reservation);
                await this.Context.SaveChangesAsync();
            }

        }

        public async Task<List<Reservation>> GetReservationsByUserIdAsync(Guid userId)
        {
            return await this.Context.Reservations.Include(reserv => reserv.Calendar).ThenInclude(calendar => calendar.Team).ThenInclude(team => team.Admins).Where(e => e.Client.Id == userId).ToListAsync();
        }

        public async Task<List<Reservation>> GetReservationsByTeamIdAsync(Guid teamId)
        {
            return await this.Context.Reservations.Include(reserv => reserv.Calendar).ThenInclude(cal => cal.Team).ThenInclude(team => team.Admins).Where(e => e.Calendar.Team.Id == teamId).ToListAsync();
        }


        public async Task<List<ReservationMedia>> GetMedias(Guid reservationId)
        {
            return await this.Context.ReservationMedia.Where(reservMedia => reservMedia.ReservationId == reservationId).Include(resr => resr.Media).ToListAsync();
        }
        public async Task LinkMediaAsync(Guid reservationId, Media media)
        {
            await MediaRepository.AddMediaAsync(media);

            Reservation? reservation = await Context.Reservations.FindAsync(reservationId);
            var reservationMedia = new ReservationMedia
            {
                ReservationId = reservationId,
                MediaId = media.Id
            };

            if (reservation != null)
            {
           
                await MediaRepository.AddReservationMediaAsync(reservationMedia);
            }
        }


        public async Task UnlinkMediaAsync(Guid reservationId, Guid mediaId)
        {
            await MediaRepository.DeleteReservationMediaAsync(reservationId,mediaId);
        }
    }
}
