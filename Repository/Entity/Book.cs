using System.ComponentModel.DataAnnotations;

namespace Repository.Entity
{
    public class Book : BaseEntity

    {

    [Key] public int Id { get; set; }

    [Required] public string ISBN { get; set; }

    [Required] public string Name { get; set; }

    [Required] public DateTime BookCreatedDate { get; set; }

    [Required] public string Author { get; set; }

    [Required] public int NumOfBooks { get; set; }
    
    [Required] public int NumOfHiringDays { get; set; }
    [Required] public double Value { get; set; }

    public virtual ICollection<BookRenting> BookRentings { get; set; }
    }
}
