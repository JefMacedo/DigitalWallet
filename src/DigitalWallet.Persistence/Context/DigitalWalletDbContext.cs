using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Persistence.Context;

public class DigitalWalletDbContext : DbContext
{
    public DigitalWalletDbContext(DbContextOptions<DigitalWalletDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
