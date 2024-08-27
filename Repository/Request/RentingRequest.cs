using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Request
{
    public class RentingRequest
    {
        [Required]
        [MaxLength(5)]
        [MinLength(0)]
        public IEnumerable<int> BookIds { get; set; }
    }
}
