using AutoMapper;
using Moq;
using Repository.Entity;
using Repository.Request;
using Repository.UnitOfWork;
using Service.Inplementation;
using Service.Interface;
using System.Data;
using System.Linq.Expressions;

namespace LibrarySystem.Test.Service
{
    [TestFixture]
    public class RentingServiceUnitTest
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private IRentingService _rentingService;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _rentingService = new RentingService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Test]
        public void RentingServiceTest_01_HasRented5Books()
        {
            int userId = 1;
            RentingRequest request = new RentingRequest();

            var studentCheck = new User()
            {
                Id = 1,
                Fullname = "Test",
                DateOfBirth = DateTime.Now,
                NumberOfBookRenting = 5,
                Password = "Test",
                PhoneNumber = "Test",
                Email = "Test",
                RoleId = 1,
            };

            var users = new List<User> { studentCheck }.AsQueryable();

            // Mocking
            _mockUnitOfWork.Setup(x => x.UserRepository.Get(
                     It.Is<Expression<Func<User, bool>>>(expr => ((LambdaExpression)expr).Body.ToString().Contains("")),
                     It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                     It.IsAny<int?>(),
                     It.IsAny<int?>(),
                     It.Is<Expression<Func<User, object>>[]>(props => props.Length == 1 && props[0].Body.ToString().Contains(""))
                 )).Returns(users);


            // Actual 
            var response = _rentingService.RentingBooks(request, userId);

            //Assertion
            var message = "The Student has rented 5 books";

            Assert.NotNull(response);
            Assert.AreEqual(message, response.Message);
            Assert.IsNull(response.Result);
        }

        [Test]
        public void RentingServiceTest_02_CheckingRentingDate()
        {
            int userId = 1;
            RentingRequest request = new RentingRequest()
            {
                BookIds = new List<int>(),
                // StartRentingDate = DateTime.Now.AddDays(-5),
                // EndRentingdate = DateTime.Now
            };


            var studentCheck = new User()
            {
                Id = 1,
                Fullname = "Test",
                DateOfBirth = DateTime.Now,
                NumberOfBookRenting = 0,
                Password = "Test",
                PhoneNumber = "Test",
                Email = "Test",
                RoleId = 1,
            };

            var users = new List<User> { studentCheck }.AsQueryable();

            // Mocking
            _mockUnitOfWork.Setup(x => x.UserRepository.Get(
                     It.Is<Expression<Func<User, bool>>>(expr => ((LambdaExpression)expr).Body.ToString().Contains("")),
                     It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                     It.IsAny<int?>(),
                     It.IsAny<int?>(),
                     It.Is<Expression<Func<User, object>>[]>(props => props.Length == 1 && props[0].Body.ToString().Contains(""))
                 )).Returns(users);

            // Actual 
            var response = _rentingService.RentingBooks(request, userId);

            //Assertion
            var message = "Can not rent for the past";

            Assert.NotNull(response);
            Assert.AreEqual(message, response.Message);
            Assert.IsNull(response.Result);
        }

        [Test]
        public void RentingServiceTest_03_CheckingStartDateOverEndDate()
        {
            int userId = 1;
            RentingRequest request = new RentingRequest()
            {
                BookIds = new List<int>(),
                // StartRentingDate = DateTime.Now.AddDays(4),
                // EndRentingdate = DateTime.Now.AddDays(3)
            };


            var studentCheck = new User()
            {
                Id = 1,
                Fullname = "Test",
                DateOfBirth = DateTime.Now,
                NumberOfBookRenting = 0,
                Password = "Test",
                PhoneNumber = "Test",
                Email = "Test",
                RoleId = 1,
            };

            var users = new List<User> { studentCheck }.AsQueryable();

            // Mocking
            _mockUnitOfWork.Setup(x => x.UserRepository.Get(
                     It.Is<Expression<Func<User, bool>>>(expr => ((LambdaExpression)expr).Body.ToString().Contains("")),
                     It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                     It.IsAny<int?>(),
                     It.IsAny<int?>(),
                     It.Is<Expression<Func<User, object>>[]>(props => props.Length == 1 && props[0].Body.ToString().Contains(""))
                 )).Returns(users);

            // Actual 
            var response = _rentingService.RentingBooks(request, userId);

            //Assertion
            var message = "Can not rent when end date is before start date";

            Assert.NotNull(response);
            Assert.AreEqual(message, response.Message);
            Assert.IsNull(response.Result);
        }

        [Test]
        public void RentingServiceTest_04_CheckingValidBooks()
        {
            int userId = 1;
            RentingRequest request = new RentingRequest()
            {
                BookIds = new List<int>()
                {
                    1, 2, 3
                },
                // StartRentingDate = DateTime.Now.AddDays(3),
                // EndRentingdate = DateTime.Now.AddDays(4)
            };


            var studentCheck = new User()
            {
                Id = 1,
                Fullname = "Test",
                DateOfBirth = DateTime.Now,
                NumberOfBookRenting = 0,
                Password = "Test",
                PhoneNumber = "Test",
                Email = "Test",
                RoleId = 1,
            };

            Book book = null; 

            var users = new List<User> { studentCheck }.AsQueryable();
            var books = new List<Book> { book }.AsQueryable();

            // Mocking Student
            _mockUnitOfWork.Setup(x => x.UserRepository.Get(
                     It.Is<Expression<Func<User, bool>>>(expr => ((LambdaExpression)expr).Body.ToString().Contains("")),
                     It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                     It.IsAny<int?>(),
                     It.IsAny<int?>(),
                     It.Is<Expression<Func<User, object>>[]>(props => props.Length == 1 && props[0].Body.ToString().Contains(""))
                 )).Returns(users);

            // Mocking Book
            _mockUnitOfWork.Setup(x => x.BookRepository.Get(
                     It.Is<Expression<Func<Book, bool>>>(expr => ((LambdaExpression)expr).Body.ToString().Contains("")),
                     It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(),
                     It.IsAny<int?>(),
                     It.IsAny<int?>(),
                     It.Is<Expression<Func<Book, object>>[]>(props => props.Length == 1 && props[0].Body.ToString().Contains(""))
                 )).Returns(books);

            // Actual 
            var response = _rentingService.RentingBooks(request, userId);

            //Assertion
            var message = "The list of books has invalid book";

            Assert.NotNull(response);
            Assert.AreEqual(message, response.Message);
            Assert.IsNull(response.Result);
        }

        [Test]
        public void RentingServiceTest_05_CheckingNumberOfBooks()
        {
            int userId = 1;
            RentingRequest request = new RentingRequest()
            {
                BookIds = new List<int>()
                {
                    1, 2, 3
                },
                // StartRentingDate = DateTime.Now.AddDays(3),
                // EndRentingdate = DateTime.Now.AddDays(4)
            };
            
            
            var student = new User()
            {
                Id = 1,
                Fullname = "Test",
                DateOfBirth = DateTime.Now,
                NumberOfBookRenting = 0,
                Password = "Test",
                PhoneNumber = "Test",
                Email = "Test",
                RoleId = 1,
            };
            
            var book = new Book
            {
                Id = 1,
                NumOfBooks = 0,
                Author = "Test",
                CreatedDate = DateTime.Now,
                ISBN = "Test",
                Name = "Test"
            };
            
            var users = new List<User> { student }.AsQueryable();
            var books = new List<Book> { book }.AsQueryable();
            
            // Mocking Student
            _mockUnitOfWork.Setup(x => x.UserRepository.Get(
                     It.Is<Expression<Func<User, bool>>>(expr => ((LambdaExpression)expr).Body.ToString().Contains("")),
                     It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                     It.IsAny<int?>(),
                     It.IsAny<int?>(),
                     It.Is<Expression<Func<User, object>>[]>(props => props.Length == 1 && props[0].Body.ToString().Contains(""))
                 )).Returns(users);
            
             // Mocking Book
            _mockUnitOfWork.Setup(x => x.BookRepository.Get(
                     It.Is<Expression<Func<Book, bool>>>(expr => ((LambdaExpression)expr).Body.ToString().Contains("")),
                     It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(),
                     It.IsAny<int?>(),
                     It.IsAny<int?>(),
                     It.IsAny<Expression<Func<Book, object>>[]>()
                 )).Returns(books);
            
            
            // Actual 
            var response = _rentingService.RentingBooks(request, userId);
            
            // Assertion
            var message = $"The book No.{book.Id} is out of stock";
            
            Assert.NotNull(response);
            Assert.AreEqual(message, response.Message);
            Assert.IsNull(response.Result);
        }

        [Test]
        public void RentingServiceTest_06_CheckingSomeBooksHasNotReturned()
        {
            int userId = 1;
            RentingRequest request = new RentingRequest()
            {
                BookIds = new List<int>()
                {
                    1, 2, 3
                },
                // StartRentingDate = DateTime.Now.AddDays(3),
                // EndRentingdate = DateTime.Now.AddDays(4)
            };


            var student = new User()
            {
                Id = 1,
                Fullname = "Test",
                DateOfBirth = DateTime.Now,
                NumberOfBookRenting = 0,
                Password = "Test",
                PhoneNumber = "Test",
                Email = "Test",
                RoleId = 1,
            };

            var book = new Book
            {
                Id = 1,
                NumOfBooks = 10,
                Author = "Test",
                CreatedDate = DateTime.Now,
                ISBN = "Test",
                Name = "Test"
            };
            
            var bookRenting = new BookRenting()
            {
                Id = 1,
                StartRentingDate = DateTime.Now.AddDays(-4),
                EndRentingDate = DateTime.Now.AddDays(-3),
                UserId = 1,
                BookId = 1,
                isBack = false
            };

            var users = new List<User> { student }.AsQueryable();
            var books = new List<Book> { book }.AsQueryable();
            var bookRentings = new List<BookRenting> { bookRenting }.AsQueryable();

            // Mocking Student
            _mockUnitOfWork.Setup(x => x.UserRepository.Get(
                It.Is<Expression<Func<User, bool>>>(expr => ((LambdaExpression)expr).Body.ToString().Contains("")),
                It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.Is<Expression<Func<User, object>>[]>(props =>
                    props.Length == 1 && props[0].Body.ToString().Contains(""))
            )).Returns(users);

            // Mocking Book
            _mockUnitOfWork.Setup(x => x.BookRepository.Get(
                It.Is<Expression<Func<Book, bool>>>(expr => ((LambdaExpression)expr).Body.ToString().Contains("")),
                It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<Expression<Func<Book, object>>[]>()
            )).Returns(books);

            // Mocking BookRenting
            _mockUnitOfWork.Setup(x => x.BookRentingRepository.Get(
                It.Is<Expression<Func<BookRenting, bool>>>(expr => ((LambdaExpression)expr).Body.ToString().Contains("")),
                It.IsAny<Func<IQueryable<BookRenting>, IOrderedQueryable<BookRenting>>>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<Expression<Func<BookRenting, object>>[]>()
            )).Returns(bookRentings);

            // Actual 
            var response = _rentingService.RentingBooks(request, userId);

            // Assertion
            var message = $"Some of the books must be returned after continue renting";

            Assert.NotNull(response);
            Assert.AreEqual(message, response.Message);
            Assert.IsNull(response.Result);
        }
        
        [Test]
        public void RentingServiceTest_07_CheckingHasMoreThan5BooksAfterRenting()
        {
            int userId = 1;
            RentingRequest request = new RentingRequest()
            {
                BookIds = new List<int>()
                {
                    1, 2, 3
                },
                // StartRentingDate = DateTime.Now.AddDays(3),
                // EndRentingdate = DateTime.Now.AddDays(4)
            };


            var student = new User()
            {
                Id = 1,
                Fullname = "Test",
                DateOfBirth = DateTime.Now,
                NumberOfBookRenting = 4,
                Password = "Test",
                PhoneNumber = "Test",
                Email = "Test",
                RoleId = 1,
            };

            var book = new Book
            {
                Id = 1,
                NumOfBooks = 10,
                Author = "Test",
                CreatedDate = DateTime.Now,
                ISBN = "Test",
                Name = "Test"
            };
            
            var bookRenting = new BookRenting()
            {
                Id = 1,
                StartRentingDate = DateTime.Now.AddDays(1),
                EndRentingDate = DateTime.Now.AddDays(3),
                UserId = 1,
                BookId = 1,
                isBack = false
            };

            var users = new List<User> { student }.AsQueryable();
            var books = new List<Book> { book }.AsQueryable();
            var bookRentings = new List<BookRenting> { bookRenting }.AsQueryable();

            // Mocking Student
            _mockUnitOfWork.Setup(x => x.UserRepository.Get(
                It.Is<Expression<Func<User, bool>>>(expr => ((LambdaExpression)expr).Body.ToString().Contains("")),
                It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.Is<Expression<Func<User, object>>[]>(props =>
                    props.Length == 1 && props[0].Body.ToString().Contains(""))
            )).Returns(users);

            // Mocking Book
            _mockUnitOfWork.Setup(x => x.BookRepository.Get(
                It.Is<Expression<Func<Book, bool>>>(expr => ((LambdaExpression)expr).Body.ToString().Contains("")),
                It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<Expression<Func<Book, object>>[]>()
            )).Returns(books);

            // Mocking BookRenting
            _mockUnitOfWork.Setup(x => x.BookRentingRepository.Get(
                It.Is<Expression<Func<BookRenting, bool>>>(expr => ((LambdaExpression)expr).Body.ToString().Contains("")),
                It.IsAny<Func<IQueryable<BookRenting>, IOrderedQueryable<BookRenting>>>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<Expression<Func<BookRenting, object>>[]>()
            )).Returns(bookRentings);

            // Actual 
            var response = _rentingService.RentingBooks(request, userId);

            // Assertion
            var message = $"Can not book with {request.BookIds.Count()} number of books because the user has rented is {student.NumberOfBookRenting}";

            Assert.NotNull(response);
            Assert.AreEqual(message, response.Message);
            Assert.IsNull(response.Result);
        }
        
        [Test]
        public void RentingServiceTest_08_CheckingOverlap()
        {
            int userId = 1;
            RentingRequest request = new RentingRequest()
            {
                BookIds = new List<int>()
                {
                    1, 2, 3
                },
                // StartRentingDate = DateTime.Now.AddDays(3),
                // EndRentingdate = DateTime.Now.AddDays(4)
            };


            var student = new User()
            {
                Id = 1,
                Fullname = "Test",
                DateOfBirth = DateTime.Now,
                NumberOfBookRenting = 1,
                Password = "Test",
                PhoneNumber = "Test",
                Email = "Test",
                RoleId = 1,
            };

            var book = new Book
            {
                Id = 1,
                NumOfBooks = 10,
                Author = "Test",
                CreatedDate = DateTime.Now,
                ISBN = "Test",
                Name = "Test"
            };
            
            var bookRenting = new BookRenting()
            {
                Id = 1,
                StartRentingDate = DateTime.Now.AddDays(1),
                EndRentingDate = DateTime.Now.AddDays(3),
                UserId = 1,
                BookId = 1,
                isBack = false
            };

            var users = new List<User> { student }.AsQueryable();
            var books = new List<Book> { book }.AsQueryable();
            var bookRentings = new List<BookRenting> { bookRenting }.AsQueryable();

            // Mocking Student
            _mockUnitOfWork.Setup(x => x.UserRepository.Get(
                It.Is<Expression<Func<User, bool>>>(expr => ((LambdaExpression)expr).Body.ToString().Contains("")),
                It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.Is<Expression<Func<User, object>>[]>(props =>
                    props.Length == 1 && props[0].Body.ToString().Contains(""))
            )).Returns(users);

            // Mocking Book
            _mockUnitOfWork.Setup(x => x.BookRepository.Get(
                It.Is<Expression<Func<Book, bool>>>(expr => ((LambdaExpression)expr).Body.ToString().Contains("")),
                It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<Expression<Func<Book, object>>[]>()
            )).Returns(books);

            // Mocking BookRenting
            _mockUnitOfWork.Setup(x => x.BookRentingRepository.Get(
                It.Is<Expression<Func<BookRenting, bool>>>(expr => ((LambdaExpression)expr).Body.ToString().Contains("")),
                It.IsAny<Func<IQueryable<BookRenting>, IOrderedQueryable<BookRenting>>>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<Expression<Func<BookRenting, object>>[]>()
            )).Returns(bookRentings);

            // Actual 
            var response = _rentingService.RentingBooks(request, userId);

            // Assertion
            var message = $"The list books you renting has been rented";

            Assert.NotNull(response);
            Assert.AreEqual(message, response.Message);
            Assert.IsNull(response.Result);
        }
        
        [Test]
        public void RentingServiceTest_09_Success()
        {
            int userId = 1;
            RentingRequest request = new RentingRequest()
            {
                BookIds = new List<int>()
                {
                    1, 2, 3
                },
                // StartRentingDate = DateTime.Now.AddDays(3),
                // EndRentingdate = DateTime.Now.AddDays(4)
            };

            var student = new User()
            {
                Id = 1,
                Fullname = "Test",
                DateOfBirth = DateTime.Now,
                NumberOfBookRenting = 1,
                Password = "Test",
                PhoneNumber = "Test",
                Email = "Test",
                RoleId = 1,
            };

            var book = new Book
            {
                Id = 1,
                NumOfBooks = 10,
                Author = "Test",
                CreatedDate = DateTime.Now,
                ISBN = "Test",
                Name = "Test"
            };

            var users = new List<User> { student }.AsQueryable();
            var books = new List<Book> { book }.AsQueryable();
            var bookRentings = new List<BookRenting>().AsQueryable();

            // Mocking Student
            _mockUnitOfWork.Setup(x => x.UserRepository.Get(
                It.Is<Expression<Func<User, bool>>>(expr => ((LambdaExpression)expr).Body.ToString().Contains("")),
                It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.Is<Expression<Func<User, object>>[]>(props =>
                    props.Length == 1 && props[0].Body.ToString().Contains(""))
            )).Returns(users);

            // Mocking Book
            _mockUnitOfWork.Setup(x => x.BookRepository.Get(
                It.Is<Expression<Func<Book, bool>>>(expr => ((LambdaExpression)expr).Body.ToString().Contains("")),
                It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<Expression<Func<Book, object>>[]>()
            )).Returns(books);

            // Mocking BookRenting
            _mockUnitOfWork.Setup(x => x.BookRentingRepository.Get(
                It.Is<Expression<Func<BookRenting, bool>>>(expr => ((LambdaExpression)expr).Body.ToString().Contains("")),
                It.IsAny<Func<IQueryable<BookRenting>, IOrderedQueryable<BookRenting>>>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<Expression<Func<BookRenting, object>>[]>()
            )).Returns(bookRentings);
            
            var mockTransaction = new Mock<IDbTransaction>();
            _mockUnitOfWork.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            
            mockTransaction.Setup(t => t.Commit());
            mockTransaction.Setup(t => t.Rollback());

            // Actual 
            var response = _rentingService.RentingBooks(request, userId);

            // Assertion
            var message = "Renting books successfully";

            Assert.NotNull(response);
            Assert.AreEqual(message, response.Message);
            Assert.IsNotNull(response.Result);
        }
    }
}
