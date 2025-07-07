namespace DigitalWallet.Application.Interfaces;

public interface IWalletService
{
    Task CreateWalletAsync(Guid userId);
    Task<decimal> GetBalanceAsync(Guid userId);
    Task<decimal> DepositAsync(Guid userId, decimal amount);
    Task<decimal> WithdrawAsync(Guid userId, decimal amount);
}
