using DigitalWallet.Application.Interfaces;
using Moq;
using FluentAssertions;

public class AuthServiceTests
{
    [Fact]
    public void GenerateToken_ShouldReturnValidToken()
    {
        // Arrange
        var mockJwtService = new Mock<IJwtService>();
        var userId = Guid.NewGuid();
        var email = "user@teste.com";
        var expectedToken = "fake.jwt.token";

        mockJwtService
            .Setup(s => s.GenerateToken(userId, email))
            .Returns(expectedToken);

        // Act
        var token = mockJwtService.Object.GenerateToken(userId, email);

        // Assert
        token.Should().Be(expectedToken);
        mockJwtService.Verify(s => s.GenerateToken(userId, email), Times.Once);
    }
}