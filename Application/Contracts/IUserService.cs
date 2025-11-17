using Application.Dto.Request.Filters;
using Application.Dto.Request;
using Application.Dto.Response;

namespace Application.Contracts;

public interface IUserService
{
    Task<IEnumerable<UserResponse>> GetUsersAsync(UserFilters filters);
    Task UpdateUserRolesAsync(Guid userId, UpdateRolesRequest request);
}
