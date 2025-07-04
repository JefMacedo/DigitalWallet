using DigitalWallet.Application.DTOs;
using FluentValidation;

namespace DigitalWallet.Application.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("E-mail é obrigatório.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Senha é obrigatória.");
    }
}
