using Repository.Request;
using Repository.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IUserService
    {
        ResponseModel GetAllUser();
        ResponseModel GetUserById(int id);
        ResponseModel CreateUser(CreateUserRequest request);
        int DeleteUserById(int id);
        ResponseModel UpdateUserById(int id, UpdateUserRequest request);
    }
}
