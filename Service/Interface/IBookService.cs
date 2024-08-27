using Repository.Request;
using Repository.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IBookService
    {
        ResponseModel GetAllBook();
        ResponseModel GetBookById(int id);
        ResponseModel CreateBook(CreateBookRequest request);
        int DeleteBookById(int id);
        ResponseModel UpdateBookById(int id, UpdateBookRequest request);
    }
}
