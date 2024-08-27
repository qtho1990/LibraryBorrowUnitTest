using AutoMapper;
using Repository.Entity;
using Repository.Request;
using Repository.Response;
using Repository.UnitOfWork;
using Service.Interface;

namespace Service.Inplementation
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        ResponseModel IUserService.CreateUser(CreateUserRequest request)
        {
            var dateTimeCreated = request.DateOfBirth;
            if (dateTimeCreated > DateTime.Now)
            {
                return new ResponseModel()
                {
                    Message = "Create fail due to creating date is after present",
                    Result = null
                };
            }

            // Checking user email duplicate
            var userEmailCheckDuplicated = _unitOfWork.UserRepository.Get(filter: x => x.Email.Equals(request.Email)).FirstOrDefault();
            if (userEmailCheckDuplicated != null)
            {
                return new ResponseModel
                {
                    Message = $"The email: {request.Email} has been registered before, please try again",
                    Result = null
                };
            }

            var userMapper = _mapper.Map<User>(request);
            var userCreated = _unitOfWork.UserRepository.Insert(userMapper);
            _unitOfWork.Save();

            var response = _mapper.Map<UserResponse>(userCreated);
            return new ResponseModel()
            {
                Message = "Create new user successfully",
                Result = response
            };
        }

        int IUserService.DeleteUserById(int id)
        {
            var user = _unitOfWork.UserRepository.Get(x => x.Id == id).FirstOrDefault();

            if (user == null)
            {
                return 0;
            }

            user.DelFlag = false;
            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.Save();
            return 1;
        }

        ResponseModel IUserService.GetAllUser()
        {
            var users = _unitOfWork.UserRepository.Get(includeProperties: x => x.Role).ToList();
            var data = _mapper.Map<List<UserResponse>>(users);
            return new ResponseModel()
            {
                Message = "List of users",
                Result = data
            };
        }

        ResponseModel IUserService.GetUserById(int id)
        {
            var user = _unitOfWork.UserRepository.Get(filter: x => x.Id == id, includeProperties: x => x.Role).FirstOrDefault();
            if (user == null)
            {
                return new ResponseModel()
                {
                    Message = "Can not find the user",
                    Result = null
                };
            }
            var data = _mapper.Map<UserResponse>(user);
            return new ResponseModel()
            {
                Message = $"Return book No.{id}",
                Result = data
            };
        }

        ResponseModel IUserService.UpdateUserById(int id, UpdateUserRequest request)
        {
            var user = _unitOfWork.UserRepository.Get(x => x.Id == id).FirstOrDefault();
            if (user == null)
            {
                return new ResponseModel()
                {
                    Message = "Can not find the user",
                    Result = null
                };
            }

            var dateTimeCreated = request.DateOfBirth;
            if (dateTimeCreated > DateTime.Now)
            {
                return new ResponseModel()
                {
                    Message = "Update fail due to creating date is after present",
                    Result = null
                };
            }

            if (!request.Email.Equals(user.Email))
            {
                var userEmailCheckDuplicated = _unitOfWork.UserRepository.Get(filter: x => x.Email.Equals(request.Email)).FirstOrDefault();
                if (userEmailCheckDuplicated != null)
                {
                    return new ResponseModel
                    {
                        Message = $"The email: {request.Email} has been registered before, please try again",
                        Result = null
                    };
                }
            }

            user.Email = request.Email;
            user.DateOfBirth = dateTimeCreated;
            user.PhoneNumber = request.PhoneNumber;
            user.Fullname = request.Fullname;
            user.RoleId = request.RoleId;
            var userUpdated = _unitOfWork.UserRepository.Update(user);
            _unitOfWork.Save();

            var response = _mapper.Map<BookResponse>(userUpdated);
            return new ResponseModel()
            {
                Message = "Create new book successfully",
                Result = response
            };
        }
    }
}
