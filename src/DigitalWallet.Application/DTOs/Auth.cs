namespace DigitalWallet.Application.DTOs;

public record RegisterUserRequest(string Name, string Email, string Password);
public record LoginRequest(string Email, string Password);
public record AuthResponse(string Token);
