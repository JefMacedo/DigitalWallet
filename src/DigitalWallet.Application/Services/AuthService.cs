using DigitalWallet.Application.DTOs;
using DigitalWallet.Application.Interfaces;
using DigitalWallet.Application.Validators;
using DigitalWallet.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class AuthService
{
    private readonly IUserRepository _users;
    private readonly IJwtService _jwt;
    private readonly IPasswordService _passwordService;
    private readonly IValidator<LoginRequest> _loginValidator;
    private readonly ILogger<AuthService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IUserRepository users,
                       IJwtService jwt,
                       IPasswordService passwordService,
                       LoginRequestValidator loginValidator,
                       ILogger<AuthService> logger,
                       IHttpContextAccessor httpContextAccessor)
    {
        _users = users;
        _jwt = jwt;
        _passwordService = passwordService;
        _loginValidator = loginValidator;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
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
        var path = _httpContextAccessor.HttpContext?.Request.Path;

        var user = await _users.GetByEmailAsync(dto.Email);
        if (user == null)
        {
            _logger.LogWarning("Tentativa de login inválida. Path: {Path}, Email: {Email}", path, dto.Email);
            throw new UnauthorizedAccessException("E-mail ou senha inválidos.");
        }

        if (!_passwordService.VerifyPassword(user, dto.Password, user.PasswordHash))
        {
            _logger.LogWarning("Tentativa de login inválida. Path: {Path}, Email: {Email}", path, dto.Email);
            throw new UnauthorizedAccessException("E-mail ou senha inválidos.");
        }

        var token = _jwt.GenerateToken(user.Id, user.Email);

        _logger.LogInformation("Usuário autenticado com sucesso: {UserId} - {Email}", user.Id, user.Email);
        return new AuthResponse(token);
    }
}
