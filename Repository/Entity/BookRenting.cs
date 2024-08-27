using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Repository.Entity
{
    public class BookRenting : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StartRentingDate { get; set; }

        [Required]
        public DateTime EndRentingDate { get; set; }

        [Required]
        [DefaultValue("false")]
        public bool isBack { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual User User { get; set; }
        
        public virtual Book Book { get; set; }
    }
}
