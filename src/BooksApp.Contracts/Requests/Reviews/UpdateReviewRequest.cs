namespace BooksApp.Contracts.Requests.Reviews;

public class UpdateReviewRequest
{
    public Guid ReviewId { get; set; }
    public int Rating { get; set; }
    public string Body { get; set; }
}