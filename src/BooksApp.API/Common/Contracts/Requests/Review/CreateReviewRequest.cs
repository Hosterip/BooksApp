namespace PostsApp.Common.Contracts.Requests.Review;

public class CreateReviewRequest
{
    public Guid BookId { get; set; }
    public int Rating { get; set; }
    public string Body { get; set; }
}