using System.Linq.Expressions;
using AutoMapper;
using Moq;
using Repository.Entity;
using Repository.Request;
using Repository.Response;
using Repository.UnitOfWork;
using Service.Inplementation;
using Service.Interface;

namespace LibrarySystem.Test.Service;

public class CreateUserSeviceUnitTest
{
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private Mock<IMapper> _mockMapper;
    private IUserService _userService;
    
    [SetUp]
    public void Setup()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _userService = new UserService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Test]
    public void CreateUserServiceTest_01_CheckingBirthDay()
    {
        int userId = 1;
        CreateUserRequest request = new CreateUserRequest()
        {
            DateOfBirth = DateTime.Now.AddDays(2)
        };

        // Actual 
        var response = _userService.CreateUser(request);

        //Assertion
        var message = "Create fail due to creating date is after present";

        Assert.NotNull(response);
        Assert.AreEqual(message, response.Message);
        Assert.IsNull(response.Result);
    }
    
    [Test]
    public void CreateUserServiceTest_02_CheckingDuplicate()
    {
        int userId = 1;
        CreateUserRequest request = new CreateUserRequest()
        {
            DateOfBirth = DateTime.Now.AddDays(-1222)
        };
        var user = new User()
        {
            Id = 1,
            Fullname = "Test",
            DateOfBirth = DateTime.Now.AddDays(-1000),
            NumberOfBookRenting = 5,
            Password = "Test",
            PhoneNumber = "Test",
            Email = "Test",
            RoleId = 1,
        };

        var users = new List<User> { user }.AsQueryable();

        //Mocking
        _mockUnitOfWork.Setup(x => x.UserRepository.Get(
            It.Is<Expression<Func<User, bool>>>(expr => ((LambdaExpression)expr).Body.ToString().Contains("")),
            It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            It.Is<Expression<Func<User, object>>[]>(props => props.Length == 0)
        )).Returns(users);
        
        // Actual 
        var response = _userService.CreateUser(request);
        
        //Assertion
        var message = $"The email: {request.Email} has been registered before, please try again";

        Assert.NotNull(response);
        Assert.AreEqual(message, response.Message);
        Assert.IsNull(response.Result);
    }
    
    [Test]
    public void CreateUserServiceTest_03_Success()
    {
        int userId = 1;
        CreateUserRequest request = new CreateUserRequest()
        {
            DateOfBirth = DateTime.Now.AddDays(-1222),
            Email = "test@gmail"
        };
        User user = null;

        var users = new List<User> { user }.AsQueryable();

        UserResponse userResponse = new UserResponse()
        {
            Id = 1
        };
        
        //Mocking
        _mockUnitOfWork.Setup(x => x.UserRepository.Get(
            It.Is<Expression<Func<User, bool>>>(expr => ((LambdaExpression)expr).Body.ToString().Contains("")),
            It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            It.Is<Expression<Func<User, object>>[]>(props => props.Length == 0)
        )).Returns(users);
        // Mocking mapper
        _mockMapper.Setup(m => m.Map<UserResponse>(It.IsAny<User>()))
            .Returns(userResponse);
        
        // Actual 
        var response = _userService.CreateUser(request);
        
        //Assertion
        var message = $"Create new user successfully";

        Assert.NotNull(response);
        Assert.AreEqual(message, response.Message);
        Assert.IsNotNull(response.Result);
    }
}