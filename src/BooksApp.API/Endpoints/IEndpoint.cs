namespace PostsApp.Controllers;

public interface IEndpoint
{ 
    void MapEndpoint(IEndpointRouteBuilder app);
}