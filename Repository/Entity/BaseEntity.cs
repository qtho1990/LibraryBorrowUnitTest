using System.ComponentModel.DataAnnotations;

namespace Repository.Entity;

public class BaseEntity
{
    [Required]
    public DateTime CreatedDate { get; set; }
    
    [Required]
    public DateTime UpdatedDate { get; set; }
    
    [Required] 
    public bool DelFlag { get; set; }
}