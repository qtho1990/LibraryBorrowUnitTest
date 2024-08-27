using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Request
{
    public class UpdateBookRequest
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
        public int NumOfBooks { get; set; } 
        
        [Required]
        public int NumOfHiringDays { get; set; } 
        
        [Required]
        public int Value { get; set; }
    }
}
