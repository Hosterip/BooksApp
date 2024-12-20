﻿using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Reviews.Results;
using BooksApp.Domain.Review;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Reviews.Commands.CreateReview;

internal sealed class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewResult>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReviewCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ReviewResult> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Books.GetSingleById(request.BookId, cancellationToken);
        var user = await _unitOfWork.Users.GetSingleById(request.UserId, cancellationToken);
        var review = Review.Create(request.Rating, request.Body, user!, book!);

        await _unitOfWork.Reviews.AddAsync(review, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return _mapper.Map<ReviewResult>(review);
    }
}