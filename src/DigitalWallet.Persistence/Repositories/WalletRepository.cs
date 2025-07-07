using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Interfaces;
using DigitalWallet.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Persistence.Repositories;

public class WalletRepository : IWalletRepository
{
    private readonly DigitalWalletDbContext _context;

    public WalletRepository(DigitalWalletDbContext context)
    {
        _context = context;
    }

    public async Task<Wallet?> GetWalletByUserIdAsync(Guid userId)
    {
        return await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
    }

    public async Task CreateAsync(Wallet wallet)
    {
        _context.Wallets.Add(wallet);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Wallet wallet)
    {
        _context.Wallets.Update(wallet);
        await _context.SaveChangesAsync();
    }
}
