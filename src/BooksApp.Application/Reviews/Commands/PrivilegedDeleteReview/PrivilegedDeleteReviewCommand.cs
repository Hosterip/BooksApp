﻿using MediatR;

namespace BooksApp.Application.Reviews.Commands.PrivilegedDeleteReview;

public sealed class PrivilegedDeleteReviewCommand : IRequest
{
    public required Guid ReviewId { get; init; }
}