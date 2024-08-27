using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Response
{
    public class BookRentingResponse
    {
        public IEnumerable<RentingBookResponse> Books { get; set; }
        public UserResponse User { get; set; }

    }
}
