using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }

        [ForeignKey(nameof(UserId))]
        public Users Users { get; set; }
    }
}