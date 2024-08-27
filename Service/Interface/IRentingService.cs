using Repository.Request;
using Repository.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IRentingService
    {
        ResponseModel RentingBooks(RentingRequest request, int userId);

        ResponseModel ReturningBooks(ReturningRequest request, int userId);
    }
}
