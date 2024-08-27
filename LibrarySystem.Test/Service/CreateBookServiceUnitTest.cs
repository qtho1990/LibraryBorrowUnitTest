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

public class CreateBookServiceUnitTest
{
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private Mock<IMapper> _mockMapper;
    private IBookService _bookService;
    
    [SetUp]
    public void Setup()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _bookService = new BookService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Test]
    public void CreateBookServiceTest_01_CheckingDate()
    {
        CreateBookRequest request = new CreateBookRequest()
        {
            BookCreatedDate = DateTime.Now.AddDays(100)
        };

        // Actual 
        var response = _bookService.CreateBook(request);

        //Assertion
        var message = "Create fail due to creating date is after present";

        Assert.NotNull(response);
        Assert.AreEqual(message, response.Message);
        Assert.IsNull(response.Result);
    }
    
    [Test]
    public void CreateBookServiceTest_02_CheckingDuplicateBook()
    {
        CreateBookRequest request = new CreateBookRequest()
        {
            BookCreatedDate = DateTime.Now.AddDays(-1000)
        };

        var book = new Book()
        {
            Id = 1,
            CreatedDate = DateTime.Now,
            Author = "Test",
            Name = "Test",
            NumOfBooks = 10,
            ISBN = "Test"
        };

        var books = new List<Book> { book }.AsQueryable();

        //Mocking
        _mockUnitOfWork.Setup(x => x.BookRepository.Get(
            It.IsAny<Expression<Func<Book, bool>>>(),
            It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(),
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            It.IsAny<Expression<Func<Book, object>>[]>()
        )).Returns(books);
        
        // Actual 
        var response = _bookService.CreateBook(request);

        //Assertion
        var message = $"The Book with ISBN: {request.ISBN} has been created";

        Assert.NotNull(response);
        Assert.AreEqual(message, response.Message);
        Assert.IsNull(response.Result);
    }
    
    [Test]
    public void CreateBookServiceTest_03_Success()
    {
        CreateBookRequest request = new CreateBookRequest()
        {
            BookCreatedDate = DateTime.Now.AddDays(-1000),
            Author = "Test",
            Name = "test",
            ISBN = "TEST"
        };

        var books = new List<Book>().AsQueryable();
        BookResponse bookResponse = new BookResponse()
        {
            Id = 1
        };
        //Mocking
        _mockUnitOfWork.Setup(x => x.BookRepository.Get(
            It.IsAny<Expression<Func<Book, bool>>>(),
            It.IsAny<Func<IQueryable<Book>, IOrderedQueryable<Book>>>(),
            It.IsAny<int?>(),
            It.IsAny<int?>(),
            It.IsAny<Expression<Func<Book, object>>[]>()
        )).Returns(books);
        
        _mockMapper.Setup(m => m.Map<BookResponse>(It.IsAny<Book>()))
            .Returns(bookResponse);
        
        // Actual 
        var response = _bookService.CreateBook(request);

        //Assertion
        var message = $"Create new book successfully";

        Assert.NotNull(response);
        Assert.AreEqual(message, response.Message);
        Assert.IsNotNull(response.Result);
    }
}