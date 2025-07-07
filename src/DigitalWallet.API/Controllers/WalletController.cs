using DigitalWallet.Application.DTOs;
using DigitalWallet.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DigitalWallet.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class WalletController : ControllerBase
{
    private readonly WalletService _walletService;

    public WalletController(WalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateWallet()
    {
        var userId = GetUserId();
        await _walletService.CreateWalletAsync(userId);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpGet]
    public async Task<IActionResult> GetBalance()
    {
        var userId = GetUserId();
        var balance = await _walletService.GetBalanceAsync(userId);
        return Ok(new { balance });
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] DepositRequest dto)
    {
        var userId = GetUserId();

        return Ok(new { balance = await _walletService.DepositAsync(userId, dto.Amount) });
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw([FromBody] WithdrawRequest dto)
    {
        var userId = GetUserId();

        return Ok(new { balance = await _walletService.WithdrawAsync(userId, dto.Amount) });
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("Usuário não autenticado.");

        return Guid.Parse(userIdClaim);
    }
}
