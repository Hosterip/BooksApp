namespace BooksApp.Contracts.Requests.Reviews;

public class CreateReviewRequest
{
    public Guid BookId { get; set; }
    public int Rating { get; set; }
    public string Body { get; set; }
}