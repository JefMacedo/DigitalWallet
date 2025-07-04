using DigitalWallet.Application.Interfaces;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DigitalWalletDbContext _db;

    public UserRepository(DigitalWalletDbContext db)
        => _db = db;

    public async Task<bool> EmailExistsAsync(string email)
        => await _db.Users.AnyAsync(u => u.Email == email);

    public async Task CreateAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
        => await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
}
