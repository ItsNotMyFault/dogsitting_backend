using dogsitting_backend.Domain.media;
using Microsoft.EntityFrameworkCore;

namespace dogsitting_backend.Infrastructure
{
    public class MediaSQLRepository
    {
        private readonly DogsittingDBContext _context;

        public MediaSQLRepository(DogsittingDBContext context)
        {
            _context = context;
        }

        public async Task AddMediaAsync(Media media)
        {
            _context.Medias.Add(media);
            await _context.SaveChangesAsync();
        }

        public async Task AddReservationMediaAsync(ReservationMedia media)
        {
            _context.ReservationMedia.Add(media);
            await _context.SaveChangesAsync();
        }

        public async Task<Media> GetMediaByIdAsync(Guid id)
        {
            return await _context.Medias.FindAsync(id);
        }

        public async Task<List<Media>> GetMediaByReservationIdAsync(Guid id)
        {
            return await _context.Medias.Include(media => media.ReservationMedias).ThenInclude(reserMedia => reserMedia.Media).ToListAsync();
        }

        public async Task DeleteMediaAsync(Guid id)
        {
            Media? media = await _context.Medias.FindAsync(id);
            if (media != null)
            {
                _context.Medias.Remove(media);
                await _context.SaveChangesAsync();
            }
        }

    }
}
