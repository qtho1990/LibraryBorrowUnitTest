using AutoMapper;
using Repository.Entity;
using Repository.Request;
using Repository.Response;
using Repository.UnitOfWork;
using Service.Interface;

namespace Service.Inplementation
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ResponseModel CreateBook(CreateBookRequest request)
        {
            var dateTimeCreated = request.BookCreatedDate;
            if (dateTimeCreated > DateTime.Now)
            {
                return new ResponseModel()
                {
                    Message = "Create fail due to creating date is after present",
                    Result = null
                };
            }

            var bookCheckDuplicated = _unitOfWork.BookRepository.Get(filter: x => x.ISBN == request.ISBN).FirstOrDefault();
            if (bookCheckDuplicated != null)
            {
                return new ResponseModel
                {
                    Message = $"The Book with ISBN: {request.ISBN} has been created",
                    Result = null
                };
            }

            var book = _mapper.Map<Book>(request);
            var bookCreated = _unitOfWork.BookRepository.Insert(book);
            _unitOfWork.Save();

            var response = _mapper.Map<BookResponse>(bookCreated);
            return new ResponseModel()
            {
                Message = "Create new book successfully",
                Result = response
            };
        }

        public int DeleteBookById(int id)
        {
            var book = _unitOfWork.BookRepository.Get(x => x.Id == id).FirstOrDefault();

            if (book == null)
            {
                return 0;
            }

            book.DelFlag = true;
            _unitOfWork.BookRepository.Update(book);
            _unitOfWork.Save(); 
            return 1;
        }

        public ResponseModel GetAllBook()
        {
            var books = _unitOfWork.BookRepository.Get().ToList();
            var data = _mapper.Map<List<BookResponse>>(books);
            return new ResponseModel()
            {
                Message = "List of books",
                Result = data
            };
        }

        public ResponseModel GetBookById(int id)
        {
            var book = _unitOfWork.BookRepository.Get(x => x.Id == id).FirstOrDefault();
            if (book == null)
            {
                return new ResponseModel()
                {
                    Message = "Can not find the book",
                    Result = null
                };
            }
            var data = _mapper.Map<BookResponse>(book);
            return new ResponseModel()
            {
                Message = $"Return book No.{id}",
                Result = data
            };
        }

        public ResponseModel UpdateBookById(int id, UpdateBookRequest request)
        {
            var book = _unitOfWork.BookRepository.Get(x => x.Id == id).FirstOrDefault();
            if (book == null)
            {
                return new ResponseModel()
                {
                    Message = "Can not find the book",
                    Result = null
                };
            }

            var dateTimeCreated = request.BookCreatedDate;
            if (dateTimeCreated < DateTime.Now)
            {
                return new ResponseModel()
                {
                    Message = "Update fail due to creating date is after present",
                    Result = null
                };
            }

            if (!request.ISBN.Equals(book.ISBN))
            {
                var bookCheckDuplicated = _unitOfWork.BookRepository.Get(filter: x => x.ISBN == request.ISBN);
                if (bookCheckDuplicated != null)
                {
                    return new ResponseModel
                    {
                        Message = $"The Book with ISBN: {request.ISBN} has been created",
                        Result = null
                    };
                }
            }

            book.ISBN = request.ISBN;
            book.CreatedDate = dateTimeCreated;
            book.Author = request.Author;
            book.Name = request.Name;
            book.NumOfBooks = request.NumOfBooks;
            book.NumOfHiringDays = request.NumOfHiringDays;
            book.Value = request.Value;
            var bookUpdated = _unitOfWork.BookRepository.Update(book);
            _unitOfWork.Save();

            var response = _mapper.Map<BookResponse>(bookUpdated);
            return new ResponseModel()
            {
                Message = "Create new book successfully",
                Result = response
            };
        }
    }
}
