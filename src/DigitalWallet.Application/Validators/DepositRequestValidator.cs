using DigitalWallet.Application.DTOs;
using FluentValidation;

namespace DigitalWallet.Application.Validators;

public class DepositRequestValidator : AbstractValidator<DepositRequest>
{
    public DepositRequestValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("O valor do depósito deve ser maior que zero.");
    }
}
