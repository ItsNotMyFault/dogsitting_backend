using dogsitting_backend.Domain.media;

namespace dogsitting_backend.ApplicationServices.response
{
    public class MediaResponse
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public byte[] FileData { get; set; }
        public DateTime UploadedAt { get; set; }

        public MediaResponse(Media media)
        {
            this.Id = media.Id;
            this.FileName = media.FileName;
            this.FileType = media.FileType;
            this.FileSize = media.FileSize;
            this.FileData = media.FileData;
            this.UploadedAt = media.UploadedAt;
        }
    }
}
