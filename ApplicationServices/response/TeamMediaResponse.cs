using dogsitting_backend.Domain.media;

namespace dogsitting_backend.ApplicationServices.response
{
    public class TeamMediaResponse : MediaResponse
    {
        public int Position { get; set; }

        public TeamMediaResponse(Media media, int position) : base(media)
        {
            this.Position = position;
        }
    }
}
