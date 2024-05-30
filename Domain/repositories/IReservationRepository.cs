using dogsitting_backend.ApplicationServices.response;
using dogsitting_backend.Domain.media;
using dogsitting_backend.Infrastructure;

namespace dogsitting_backend.Domain.repositories
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task LinkMediaAsync(Guid reservationId, Media media);
        Task UnlinkMediaAsync(Guid reservationId, Guid mediaId);
        Task<List<ReservationMedia>> GetMedias(Guid reservationId);
        Task<Reservation> GetByIdAsync(Guid id);
        Task<List<Reservation>> GetAllAsync();
        Task<List<Reservation>> GetReservationsByUserIdAsync(Guid userId);
        Task<List<Reservation>> GetReservationsByTeamIdAsync(Guid teamId);
    }
}
