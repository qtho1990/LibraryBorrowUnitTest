using AutoMapper;
using Repository.Request;
using Repository.Response;
using Repository.UnitOfWork;
using Service.Interface;
using Service.Util.JWTUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Inplementation
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJWTHelper _jWTHelper;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IJWTHelper jWTHelper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jWTHelper = jWTHelper;
        }

        public ResponseModel Login (LoginRequest request)
        {
            if (request == null) 
            {
                return new ResponseModel
                {
                    Message = "Request is null",
                    Result = null
                };
            }

            var user = _unitOfWork.UserRepository.Get(filter: x => x.Email.Equals(request.Email), includeProperties: x => x.Role).FirstOrDefault();
            if (user == null) 
            {
                return new ResponseModel
                {
                    Message = $"Can not find the user with email: {request.Email}",
                    Result = null
                };
            }

            if (!user.Password.Equals(request.Password))
            {
                return new ResponseModel
                {
                    Message = $"Wrong Password",
                    Result = null
                };
            }

            if (user.Password.Equals(request.Password))
            {
                var token = _jWTHelper.CreateToken(user);

                return new ResponseModel
                {
                    Message = $"Login Successfully",
                    Result = token
                };
            }

            return new ResponseModel
            {
                Message = "Internal Server",
                Result = null
            };
        }
    }
}
