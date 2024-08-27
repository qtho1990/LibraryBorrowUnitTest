using Repository.Data;
using Repository.Entity;
using Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class BookRentingRepository : GenericRepository<BookRenting>, IBookRentingRepository
    {
        public BookRentingRepository(ApplicationContext context) : base(context) { }
    }
}
