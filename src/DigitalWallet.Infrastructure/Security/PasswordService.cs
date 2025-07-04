using DigitalWallet.Application.Interfaces;
using DigitalWallet.Domain.Entities;
using Microsoft.AspNetCore.Identity;

public class PasswordService : IPasswordService
{
    private readonly IPasswordHasher<User> _hasher;

    public PasswordService()
    {
        _hasher = new PasswordHasher<User>();
    }

    public string HashPassword(User user, string password)
        => _hasher.HashPassword(user, password);

    public bool VerifyPassword(User user, string password, string hash)
        => _hasher.VerifyHashedPassword(user, hash, password) == PasswordVerificationResult.Success;
}