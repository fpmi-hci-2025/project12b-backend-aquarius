using Application.Contracts;
using Application.Dto.Request;
using Application.Dto.Request.Filters;
using Application.Dto.Response;
using Application.Exceptions;
using Application.Services;
using AutoMapper;
using Domain;
using Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Unit
{
    public class UserServiceTests
    {
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly Mock<IRepository<Role>> _mockRoleRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IRepository<User>>();
            _mockRoleRepository = new Mock<IRepository<Role>>();
            _mockMapper = new Mock<IMapper>();
            _userService = new UserService(_mockUserRepository.Object, _mockRoleRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetUsersAsync_ValidFilters_ReturnsUsers()
        {
            // Arrange
            var filters = new UserFilters { PageNumber = 1, PageSize = 10 };
            var users = new List<User>
            {
                new User { Id = Guid.NewGuid(), Email = "user1@test.com" },
                new User { Id = Guid.NewGuid(), Email = "user2@test.com" }
            };
            var userResponses = new List<UserResponse>
            {
                new UserResponse { Id = users[0].Id, Email = "user1@test.com" },
                new UserResponse { Id = users[1].Id, Email = "user2@test.com" }
            };

            _mockUserRepository.Setup(x => x.GetPagedAsync(1, 10, It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(users);
            _mockMapper.Setup(x => x.Map<IEnumerable<UserResponse>>(users))
                .Returns(userResponses);

            // Act
            var result = await _userService.GetUsersAsync(filters);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockUserRepository.Verify(x => x.GetPagedAsync(1, 10, It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateUserRolesAsync_ValidRequest_UpdatesRoles()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Roles = new List<Role>
                {
                    new Role { Name = "OldRole" }
                }
            };
            var request = new UpdateRolesRequest { Roles = new List<string> { "Admin", "User" } };
            var allRoles = new List<Role>
            {
                new Role { Name = "Admin" },
                new Role { Name = "User" },
                new Role { Name = "Moderator" }
            };

            _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);
            _mockRoleRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(allRoles);
            _mockUserRepository.Setup(x => x.UpdateAsync(user)).Returns(Task.CompletedTask);
            _mockUserRepository.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(1));

            // Act
            await _userService.UpdateUserRolesAsync(userId, request);

            // Assert
            Assert.Equal(2, user.Roles.Count);
            Assert.Contains(user.Roles, r => r.Name == "Admin");
            Assert.Contains(user.Roles, r => r.Name == "User");
            _mockUserRepository.Verify(x => x.UpdateAsync(user), Times.Once);
            _mockUserRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateUserRolesAsync_UserNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new UpdateRolesRequest { Roles = new List<string> { "Admin" } };

            _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _userService.UpdateUserRolesAsync(userId, request));
        }

        [Fact]
        public async Task UpdateUserRolesAsync_NonExistentRole_IgnoresInvalidRole()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Roles = new List<Role>() };
            var request = new UpdateRolesRequest { Roles = new List<string> { "Admin", "NonExistentRole" } };
            var allRoles = new List<Role> { new Role { Name = "Admin" } };

            _mockUserRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);
            _mockRoleRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(allRoles);
            _mockUserRepository.Setup(x => x.UpdateAsync(user)).Returns(Task.CompletedTask);
            _mockUserRepository.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(1));

            // Act
            await _userService.UpdateUserRolesAsync(userId, request);

            // Assert
            Assert.Single(user.Roles);
            Assert.Contains(user.Roles, r => r.Name == "Admin");
            Assert.DoesNotContain(user.Roles, r => r.Name == "NonExistentRole");
        }
    }
}