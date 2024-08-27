using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Response
{
    public class BookResponse
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string Name { get; set; }
        public DateTime BookCreatedDate { get; set; }
        public string Author { get; set; } 
        public int NumOfHiringDays { get; set; }
        public int NumOfBooks { get; set; }
        public int Value { get; set; }
    }
}
