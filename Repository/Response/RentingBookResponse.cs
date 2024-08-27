namespace Repository.Response;

public class RentingBookResponse
{
    public int Id { get; set; }
    public string ISBN { get; set; }
    public string Name { get; set; }
    public DateTime BookCreatedDate { get; set; }
    public string Author { get; set; } 
    public int NumOfHiringDays { get; set; } 
    public int Value { get; set; }

    public DateTime StartRentingDate { get; set; }
    public DateTime EndRentingDate { get; set; }
}