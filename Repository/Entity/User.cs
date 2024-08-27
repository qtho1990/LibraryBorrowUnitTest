using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Repository.Entity
{
    public class User : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Fullname { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public int NumberOfBookRenting { get; set; }
        
        [Required]
        [DefaultValue(100)]
        public double Creditvalue { get; set; }

        [Required]
        public int RoleId { get; set; } 
        public virtual Role Role { get; set; }

        public virtual ICollection<BookRenting> BookRentings { get; set; }
    }
}
