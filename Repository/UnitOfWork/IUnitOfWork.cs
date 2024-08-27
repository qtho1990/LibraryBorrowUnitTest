using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        IBookRepository BookRepository { get; }
        IUserRepository UserRepository{ get; }
        IBookRentingRepository BookRentingRepository{ get; }
        int Save();
        void Dispose();
        IDbTransaction BeginTransaction();
    }
}
