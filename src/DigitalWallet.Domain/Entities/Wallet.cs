namespace DigitalWallet.Domain.Entities;

public class Wallet
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }

    public decimal Balance { get; set; } = 0m;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("O valor do depósito deve ser positivo.");

        Balance += amount;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("O valor do saque deve ser positivo.");

        if (Balance < amount)
            throw new InvalidOperationException("Saldo insuficiente.");

        Balance -= amount;
        UpdatedAt = DateTime.UtcNow;
    }
}
