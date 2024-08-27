using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Util.JWTUtil
{
    public interface IJWTHelper
    {
        string CreateToken(User user);
    }
}
