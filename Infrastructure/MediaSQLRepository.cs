using dogsitting_backend.Domain;
using dogsitting_backend.Domain.media;
using Microsoft.EntityFrameworkCore;

namespace dogsitting_backend.Infrastructure
{
    public class MediaSQLRepository
    {
        private readonly DogsittingDBContext Context;

        public MediaSQLRepository(DogsittingDBContext context)
        {
            this.Context = context;
        }

        public async Task AddMediaAsync(Media media)
        {
            Context.Medias.Add(media);
            await Context.SaveChangesAsync();
        }

        public async Task AddReservationMediaAsync(ReservationMedia media)
        {
            Context.ReservationMedia.Add(media);
            await Context.SaveChangesAsync();
        }

        private async Task AddTeamMediaAsync(TeamMedia media)
        {
            Context.TeamMedia.Add(media);
            await Context.SaveChangesAsync();
        }

        private async Task AddUserMediaAsync(UserMedia media)
        {
            Context.UserMedia.Add(media);
            await Context.SaveChangesAsync();
        }

        public async Task<Media> GetMediaByIdAsync(Guid id)
        {
            return await Context.Medias.FindAsync(id);
        }

        public async Task<Media> GetTeamMediaByMediaIdAsync(Guid id)
        {
            return await Context.Medias.Include(media => media.TeamMedias).Where(media => media.Id == id).FirstAsync();
        }

        public async Task<List<Media>> GetMediaByReservationIdAsync(Guid id)
        {
            return await Context.Medias.Include(media => media.ReservationMedias).ThenInclude(reserMedia => reserMedia.Media).ToListAsync();
        }

        public async Task<List<Media>> GetTeamMediasAsync(Guid id)
        {
            return await Context.Medias.Include(media => media.TeamMedias).ThenInclude(reserMedia => reserMedia.Media).ToListAsync();
        }

        public async Task DeleteReservationMediaAsync(Guid reservationId, Guid mediaId)
        {
            ReservationMedia reservationMedia = await Context.ReservationMedia.Where(teamMedia => teamMedia.ReservationId == reservationId).Include(teamMedia => teamMedia.Media).FirstAsync();

            Context.Medias.Remove(reservationMedia.Media);
            Context.ReservationMedia.Remove(reservationMedia);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAllTeamMediaAsync(Guid teamId)
        {
            List<TeamMedia> teamMediaList = await Context.TeamMedia.Where(teamMedia => teamMedia.TeamId == teamId).Include(teamMedia => teamMedia.Media).ToListAsync();

            teamMediaList.Select(x => x.Media).ToList().ForEach(media =>
            {
                Context.Medias.Remove(media);
            });
            teamMediaList.ForEach(tMedia =>
            {
                Context.TeamMedia.Remove(tMedia);
            });
            await Context.SaveChangesAsync();
        }

        public async Task<List<TeamMedia>> GetTeamMedias(Guid teamId)
        {
            return await this.Context.TeamMedia.Where(teamMedia => teamMedia.TeamId == teamId)
                .Include(teamMedia => teamMedia.Team)
                .Include(teamMedia => teamMedia.Media).ToListAsync();
        }

        public async Task AddTeamMedia(Guid teamId, Media media, int position)
        {
            await this.AddMediaAsync(media);
            TeamMedia teamMedia = new()
            {
                TeamId = teamId,
                MediaId = media.Id,
                Position = position

            };

            await this.AddTeamMediaAsync(teamMedia);
        }

        public async Task DeleteTeamMediaAsync(Guid teamId, Guid mediaId)
        {
            TeamMedia teamMedia = await Context.TeamMedia.Where(media => media.MediaId == mediaId && media.TeamId == teamId).Include(teamMedia => teamMedia.Media).FirstAsync();

            Context.Medias.Remove(teamMedia.Media);
            Context.TeamMedia.Remove(teamMedia);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteMediaAsync(Guid? mediaId)
        {
            Media media = await Context.Medias.FindAsync(mediaId);
            Context.Medias.Remove(media);
            await Context.SaveChangesAsync();
        }

    }
}
