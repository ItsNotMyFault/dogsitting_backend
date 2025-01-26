using dogsitting_backend.ApplicationServices.dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dogsitting_backend.Domain.media
{
    [Table("medias")]
    public class Media
    {
        [Key]
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public byte[] FileData { get; set; }
        public DateTime UploadedAt { get; set; }

        public virtual ICollection<ReservationMedia> ReservationMedias { get; set; }
        public virtual ICollection<TeamMedia> TeamMedias { get; set; }
        public virtual ICollection<UserMedia> UserMedias { get; set; }

        public Media(IFormFile file)
        {
            this.FileName = file.FileName;
            this.FileType = file.ContentType;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                this.FileData = ms.ToArray();
            }

            this.FileSize = file.Length;
            this.UploadedAt = DateTime.Now.ToLocalTime();
        }
        public Media() { }//required for DB

    }
}
