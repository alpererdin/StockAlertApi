using StockAlertApi.Core.Entities;
using StockAlertApi.Core.Interfaces.Repositories;
using StockAlertApi.Core.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace StockAlertApi.Application.Services;

public class UsersService : IUsersService
{
    private readonly IUserRepository _userRepository;

    public UsersService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<User?> LoginAsync(string username, string password)
    {
        throw new NotImplementedException();
    }

    public Task<User?> RegisterAsync(string username, string email, string password)
    {
        throw new NotImplementedException();
    }
}