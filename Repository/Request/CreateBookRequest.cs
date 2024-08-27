using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Request
{
    public class CreateBookRequest
    {
        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime BookCreatedDate { get; set; }

        [Required]
        public string Author { get; set; }
        
        [Required]
        [Range(minimum:0, maximum: Int32.MaxValue)]
        public int NumOfBooks { get; set; } 
        
        [Required]
        [Range(minimum:0, maximum: Int32.MaxValue)]
        public int NumOfHiringDays { get; set; } 
        
        [Required]
        [Range(minimum:0, maximum: Int32.MaxValue)]
        public int Value { get; set; }
    }
}
