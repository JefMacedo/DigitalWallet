using DigitalWallet.Domain.Entities;

namespace DigitalWallet.Domain.Interfaces;

public interface IWalletRepository
{
    Task<Wallet?> GetWalletByUserIdAsync(Guid userId);
    Task CreateAsync(Wallet wallet);
    Task UpdateAsync(Wallet wallet);
}
