using DigitalWallet.Domain.Entities;

namespace DigitalWallet.Application.Interfaces;

public interface IUserRepository
{
    Task<bool> EmailExistsAsync(string email);
    Task CreateAsync(User user);
    Task<User?> GetByEmailAsync(string email);
}
