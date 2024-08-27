using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Repository.Entity;
using Repository.Request;
using Repository.Response;
using Repository.UnitOfWork;
using Service.Interface;

namespace Service.Inplementation
{
    public class RentingService : IRentingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RentingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public ResponseModel RentingBooks(RentingRequest request, int userId)
        {
            var user = _unitOfWork.UserRepository.Get(filter: x => x.Id == userId, includeProperties: x => x.Role).FirstOrDefault();
            if (user.Creditvalue == 0)
            {
                return new ResponseModel()
                {
                    Message = $"{user.Fullname} has 0 credit, please return some books before borrowing more",
                    Result = null
                };
            }
            
            BookRentingResponse response = new BookRentingResponse();
            List<RentingBookResponse> bookResponse = new List<RentingBookResponse>();
            RentingBookResponse rentingBookResponse = new RentingBookResponse();
            if (user!.NumberOfBookRenting >= 5)
            {
                return new ResponseModel()
                {
                    Message = "The Student has rented 5 books",
                    Result = null
                };
            }
            
            var books = new List<Book>();
            using var transaction = _unitOfWork.BeginTransaction();
            var bookRentingList = new List<BookRenting>();
            
            // Check after book the number of renting book
            if (user.NumberOfBookRenting + request.BookIds.Count() > 5)
            {
                return new ResponseModel()
                {
                    Message = $"Can not book with {request.BookIds.Count()} books because the user has rented is {user.NumberOfBookRenting}",
                    Result = null
                };
            }

            double totalValue = 0.0;
            foreach (var bookId in request.BookIds)
            {
                // Check valid book
                var book = _unitOfWork.BookRepository.Get(filter: x => x.Id == bookId).FirstOrDefault();
                if (book == null)
                {
                    return new ResponseModel()
                    {
                        Message = $"The list of books has invalid book",
                        Result = null
                    };
                }
                
                // Check number of books
                if (book.NumOfBooks == 0)
                {
                    return new ResponseModel()
                    {
                        Message = $"The book No.{book.Id} is out of stock",
                        Result = null
                    };
                }
                
                // Checking the renting book has rented
                var bookBorrowings = _unitOfWork.BookRentingRepository
                    .Get(filter: x => !x.isBack && x.UserId == user.Id && x.BookId == book.Id).ToList();
                if (!bookBorrowings.IsNullOrEmpty())
                {
                    return new ResponseModel()
                    {
                        Message = $"The book No.{book.Id} has not returned by {user.Fullname}",
                        Result = null
                    };
                }

                totalValue += book.Value;
                
                // Minus 1
                book.NumOfBooks -= 1;
                _unitOfWork.BookRepository.Update(book);
                books.Add(book);
                rentingBookResponse = _mapper.Map<RentingBookResponse>(book);
                var bookRenting = new BookRenting()
                {
                    BookId = bookId,
                    EndRentingDate = DateTime.Now.AddDays(book.NumOfHiringDays),
                    StartRentingDate = DateTime.Now,    
                    UserId = userId,
                };
                bookRentingList.Add(bookRenting);
                rentingBookResponse.EndRentingDate = bookRenting.EndRentingDate;
                rentingBookResponse.StartRentingDate = bookRenting.StartRentingDate;
                bookResponse.Add(rentingBookResponse);
            }
            
            // Checking user has enough value
            if (user.Creditvalue - totalValue < 0)
            {
                return new ResponseModel()
                {
                    Message = $"{user.Fullname} do not have enough credit points for borrowing those books",
                    Result = null
                };
            }

            user.Creditvalue -= totalValue;
            _unitOfWork.UserRepository.Update(user);
            
            // Commit inserting
            try
            {
                _unitOfWork.BookRentingRepository.BulkInsert(bookRentingList);
                _unitOfWork.Save();


                user.NumberOfBookRenting += request.BookIds.Count();
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Save();
                
                transaction.Commit();
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                return new ResponseModel()
                {
                    Result = null,
                    Message = ex.Message
                };
            }

            var userResponse = _mapper.Map<UserResponse>(user);
            
            response.User = userResponse;
            response.Books = bookResponse;
            return new ResponseModel()
            {
                Result = response,
                Message = "Renting books successfully"
            };
        }

        public ResponseModel ReturningBooks(ReturningRequest request, int userId)
        {
            var user = _unitOfWork.UserRepository.Get(filter: x => x.Id == userId, includeProperties: x => x.Role).FirstOrDefault();
            List<BookResponse> responses = new List<BookResponse>();
            Book book = null;
                
            foreach (var bookId in request.BookIds)
            {
                var bookResponse = new BookResponse();

                // Check valid book
                book = _unitOfWork.BookRepository.Get(filter: x => x.Id == bookId).FirstOrDefault();
                if (book == null)
                {
                    return new ResponseModel()
                    {
                        Message = $"The list of books has invalid book",
                        Result = null
                    };
                }

                var bookRentings = _unitOfWork.BookRentingRepository.Get(filter: x => x.UserId == userId && x.BookId == bookId).ToList();
                if (bookRentings.IsNullOrEmpty())
                {
                    return new ResponseModel()
                    {
                        Message = $"{user!.Fullname} did not rent the book No.{bookId}",
                        Result = null
                    };
                }

                foreach(var bookRenting in bookRentings)
                {
                    if (bookRenting.isBack)
                    {
                        return new ResponseModel()
                        {
                            Message = $"{user!.Fullname} has returned the book No.{bookId}",
                            Result = null
                        };
                    }

                    // Returning
                    bookRenting.isBack = true;
                    _unitOfWork.BookRentingRepository.Update(bookRenting);

                    // Update BookNumber
                    book.NumOfBooks += 1;
                    _unitOfWork.BookRepository.Update(book);
                    
                    // update user credit
                    user.Creditvalue += book.Value;

                    bookResponse = _mapper.Map<BookResponse>(book);
                    responses.Add(bookResponse);
                }
            }
            // Update user
            user!.NumberOfBookRenting -= request.BookIds.Count();
            _unitOfWork.UserRepository.Update(user);

            // Commit
            _unitOfWork.Save();

            return new ResponseModel()
            {
                Message = "Returning books successfully",
                Result = responses
            };
        }
    }
}
