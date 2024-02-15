using PostsApp.Contracts.Requests.Book;

namespace PostsApp.Contracts.Requests.Post;

public class UpdateBookRequest : BookRequest
{
    public int Id { get; set; }
}