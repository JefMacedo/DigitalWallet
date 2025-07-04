using DigitalWallet.Domain.Entities;

namespace DigitalWallet.Application.Interfaces;

public interface IPasswordService
{
    string HashPassword(User user, string password);
    bool VerifyPassword(User user, string password, string hash);
}
