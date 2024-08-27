using Microsoft.EntityFrameworkCore.Storage;
using Repository.Data;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext context;
        private IBookRepository? bookRepository;
        private IUserRepository? userRepository;
        private IBookRentingRepository? bookRentingRepository;
        public UnitOfWork(ApplicationContext context)
        {
            this.context = context;
        }

        public IBookRepository BookRepository 
        {
            get
            {
                if (this.bookRepository == null)
                {
                    this.bookRepository = new BookRepository(context);
                }
                return this.bookRepository;
            }
        }

        IUserRepository IUnitOfWork.UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new UserRepository(context);
                }
                return this.userRepository;
            }
        }

        public IBookRentingRepository BookRentingRepository 
        {
            get
            {
                if (this.bookRentingRepository == null)
                {
                    this.bookRentingRepository = new BookRentingRepository(context);
                }
                return this.bookRentingRepository;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        public IDbTransaction BeginTransaction()
        {
            var transaction = context.Database.BeginTransaction();
            return transaction.GetDbTransaction();
        }
    }
}
