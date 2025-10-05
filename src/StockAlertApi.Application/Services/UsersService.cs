using StockAlertApi.Core.Entities;
using StockAlertApi.Core.Interfaces.Repositories;
using StockAlertApi.Core.Interfaces.Services;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StockAlertApi.Application.Services;

public class UsersService : IUsersService
{
    private readonly IUserRepository _userRepository;

    public UsersService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> RegisterAsync(string username, string email, string password)
    {
        if (await _userRepository.UsernameExistsAsync(username) || await _userRepository.EmailExistsAsync(email))
        {
            return null;
        }

        CreatePasswordHash(password, out string passwordHash, out string passwordSalt);

        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            IsActive = true
        };

        await _userRepository.AddAsync(user);
        return user;
    }

    public async Task<User?> LoginAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);

        if (user == null || !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            return null; //  
        }

        return user;  
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        return _userRepository.GetByIdAsync(id);
    }

    private void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = Convert.ToBase64String(hmac.Key);
            passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }

    private bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt)
    {
        using (var hmac = new HMACSHA512(Convert.FromBase64String(passwordSalt)))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(computedHash) == passwordHash;
        }
    }
}