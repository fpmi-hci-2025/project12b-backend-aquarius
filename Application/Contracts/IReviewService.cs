using Application.Dto.Request;
using Application.Dto.Request.Filters;
using Application.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts;

public interface IReviewService
{
    Task<IEnumerable<ReviewResponse>> GetReviews(ReviewFilters filters);

    Task<ReviewResponse> CreateReview(Guid userId, CreateReviewRequest request);
}
