namespace PostsApp.Common.Contracts.Requests.Review;

public class UpdateReviewRequest
{
    public int ReviewId { get; set; }
    public int Rating { get; set; }
    public string Body { get; set; }
}