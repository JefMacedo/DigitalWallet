using DigitalWallet.Application.DTOs;
using DigitalWallet.Application.Interfaces;
using DigitalWallet.Application.Validators;
using DigitalWallet.Domain.Entities;
using FluentValidation;

public class AuthService
{
    private readonly IUserRepository _users;
    private readonly IJwtService _jwt;
    private readonly IPasswordService _passwordService;
    private readonly IValidator<LoginRequest> _loginValidator;

    public AuthService(IUserRepository users, IJwtService jwt, IPasswordService passwordService, LoginRequestValidator loginValidator)
    {
        _users = users;
        _jwt = jwt;
        _passwordService = passwordService;
        _loginValidator = loginValidator;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterUserRequest dto)
    {
        if (await _users.EmailExistsAsync(dto.Email))
            throw new Exception("E-mail já registrado.");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email
        };

        user.PasswordHash = _passwordService.HashPassword(user, dto.Password);

        await _users.CreateAsync(user);
        var token = _jwt.GenerateToken(user.Id, user.Email);
        return new AuthResponse(token);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest dto)
    {
        await _loginValidator.ValidateAndThrowAsync(dto);

        var user = await _users.GetByEmailAsync(dto.Email);
        if (user == null)
            throw new UnauthorizedAccessException("E-mail ou senha inválidos.");

        if (!_passwordService.VerifyPassword(user, dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("E-mail ou senha inválidos.");

        var token = _jwt.GenerateToken(user.Id, user.Email);
        return new AuthResponse(token);
    }
}
