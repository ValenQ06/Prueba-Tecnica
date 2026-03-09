using Microsoft.EntityFrameworkCore;
using PruebaComerciantes.Application.DTOs;
using PruebaComerciantes.Infrastructure.Data;
using PruebaComerciantes.Infrastructure.Services;
using Xunit;

namespace PruebaComerciantes.Tests.Services
{
    public class AuthServiceTests
    {
        [Fact]
        public async Task Login_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            var context = new ApplicationDbContext(options);

            var jwtGenerator = new Infrastructure.Security.JwtGenerator(
                new ConfigurationBuilder().Build());

            var service = new AuthService(context, jwtGenerator);

            var request = new LoginRequestDto
            {
                CorreoElectronico = "test@test.com",
                Password = "123456"
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.Login(request));
        }
    }
}