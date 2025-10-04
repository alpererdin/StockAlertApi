using StockAlertApi.Core.Entities;

namespace StockAlertApi.Core.Interfaces.Services;

public interface IUsersService
{
    Task<User?> RegisterAsync(string username, string email, string password);
    Task<User?> LoginAsync(string username, string password);
    Task<User?> GetByIdAsync(Guid id);
}