using DigitalWallet.Application.DTOs;
using FluentValidation;

namespace DigitalWallet.Application.Validators;

public class WithdrawRequestValidator : AbstractValidator<WithdrawRequest>
{
    public WithdrawRequestValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("O valor do saque deve ser maior que zero.");
    }
}
