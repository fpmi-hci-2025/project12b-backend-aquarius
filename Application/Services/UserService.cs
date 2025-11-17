
using Application.Contracts;
using Application.Dto.Request;
using Application.Dto.Request.Filters;
using Application.Dto.Response;
using Application.Exceptions;
using AutoMapper;
using Domain;
using Entities;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Role> _roleRepository;
    private readonly IMapper _mapper;

    public UserService(
        IRepository<User> userRepository, 
        IRepository<Role> roleRepository, 
        IMapper mapper)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserResponse>> GetUsersAsync(UserFilters filters)
    {
        var users = await _userRepository.GetPagedAsync(filters.PageNumber, filters.PageSize, x => true);

        var usersResponse = _mapper.Map<IEnumerable<UserResponse>>(users);

        return usersResponse;
    }

    public async Task UpdateUserRolesAsync(Guid userId, UpdateRolesRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            throw new NotFoundException($"User {userId} was not found");
        }

        user.Roles.Clear();

        var allRoles = await _roleRepository.GetAllAsync();

        foreach (var roleName in request.Roles)
        {
            var role = allRoles.FirstOrDefault(r => r.Name == roleName);
            if (role != null)
            {
                user.Roles.Add(role);
            }
        }

        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();
    }
}
