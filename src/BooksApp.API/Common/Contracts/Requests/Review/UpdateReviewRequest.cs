namespace PostsApp.Common.Contracts.Requests.Review;

public class UpdateReviewRequest
{
    public Guid ReviewId { get; set; }
    public int Rating { get; set; }
    public string Body { get; set; }
}