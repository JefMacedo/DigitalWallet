using DigitalWallet.Application.Interfaces;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace DigitalWallet.Application.Services;

public class WalletService : IWalletService
{
    private readonly IWalletRepository _walletRepository;
    private readonly ILogger<WalletService> _logger;

    public WalletService(IWalletRepository walletRepository, ILogger<WalletService> logger)
    {
        _walletRepository = walletRepository;
        _logger = logger;
    }

    public async Task CreateWalletAsync(Guid userId)
    {
        var existing = await _walletRepository.GetWalletByUserIdAsync(userId);
        if (existing is not null)
            throw new InvalidOperationException("Usuário já possui uma carteira.");

        var wallet = new Wallet
        {
            UserId = userId,
            Balance = 0,
            CreatedAt = DateTime.UtcNow
        };

        await _walletRepository.CreateAsync(wallet);

        _logger.LogInformation("Carteira criada para o usuário {UserId}", userId);
    }

    public async Task<decimal> GetBalanceAsync(Guid userId)
    {
        var wallet = await _walletRepository.GetWalletByUserIdAsync(userId)
            ?? throw new InvalidOperationException("Carteira não encontrada.");

        _logger.LogInformation("Consulta de saldo para o usuário {UserId}", userId);
        return wallet.Balance;
    }

    public async Task<decimal> DepositAsync(Guid userId, decimal amount)
    {
        var wallet = await _walletRepository.GetWalletByUserIdAsync(userId)
            ?? throw new InvalidOperationException("Carteira não encontrada.");

        wallet.Deposit(amount);
        await _walletRepository.UpdateAsync(wallet);

        _logger.LogInformation("Depósito realizado para o usuário {UserId}: +{Amount}", userId, amount);
        
        return wallet.Balance;
    }

    public async Task<decimal> WithdrawAsync(Guid userId, decimal amount)
    {
        var wallet = await _walletRepository.GetWalletByUserIdAsync(userId)
            ?? throw new InvalidOperationException("Carteira não encontrada.");

        wallet.Withdraw(amount);
        await _walletRepository.UpdateAsync(wallet);

        _logger.LogInformation("Saque realizado para o usuário {UserId}: +{Amount}", userId, amount);

        return wallet.Balance;
    }
}
