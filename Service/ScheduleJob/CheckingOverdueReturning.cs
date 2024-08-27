using Repository.UnitOfWork;

namespace Service.ScheduleJob;

public class CheckingOverdueReturning
{
    private readonly IUnitOfWork _unitOfWork;

    public CheckingOverdueReturning(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public void CheckingTheOverdueReturningBooks()
    {
        var bookRentings = _unitOfWork.BookRentingRepository
            .Get(filter: x => !x.isBack && x.EndRentingDate < DateTime.Now).ToList();
        foreach (var bookRenting in bookRentings)
        {
            var user = _unitOfWork.UserRepository.Get(x => x.Id == bookRenting.UserId).FirstOrDefault();
            var book = _unitOfWork.BookRepository.Get(x => x.Id == bookRenting.BookId).FirstOrDefault();

            user.Creditvalue -= book.Value * 10 / 100;
            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.Save();
        }
        
        Console.WriteLine("Finish");
    }
}