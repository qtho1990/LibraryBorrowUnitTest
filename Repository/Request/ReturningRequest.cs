using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Request
{
    public class ReturningRequest
    {
        public IEnumerable<int> BookIds { get; set; }
    }
}
