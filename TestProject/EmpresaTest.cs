using EcoBalance.Controllers;
using EcoBalance.Database;
using EcoBalance.DTO;
using EcoBalance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

public class EmpresasControllerTests
{
    private readonly EmpresasController _controller;
    private readonly DbContextOptions<OracleDbContext> _options;
    private readonly Mock<IConfiguration> _mockConfiguration;

    public EmpresasControllerTests()
    {
        _options = new DbContextOptionsBuilder<OracleDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        var context = new OracleDbContext(_options);
        _mockConfiguration = new Mock<IConfiguration>();

        _controller = new EmpresasController(context, _mockConfiguration.Object);
    }

    [Fact]
    public async Task Register_ShouldReturnCreatedAtAction_WhenValidRequest()
    {
        // Arrange
        var empresaDto = new EmpresaRegisterDTO
        {
            Nome = "Empresa Teste",
            Email = "teste@empresa.com",
            Telefone = "123456789",
            Cnpj = "12345678000100",
            Senha = "senhaSegura"
        };

        // Act
        var result = await _controller.Register(empresaDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Empresa>>(result);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        var returnValue = Assert.IsType<Empresa>(createdAtActionResult.Value);
        Assert.Equal(empresaDto.Nome, returnValue.Nome);
        Assert.Equal(empresaDto.Email, returnValue.Email);
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequest_WhenEmailExists()
    {
        // Arrange
        var empresaDto = new EmpresaRegisterDTO
        {
            Nome = "Empresa Teste",
            Email = "teste@empresa.com",
            Telefone = "123456789",
            Cnpj = "12345678000100",
            Senha = "senhaSegura"
        };

        using (var context = new OracleDbContext(_options))
        {
            context.Empresas.Add(new Empresa
            {
                Nome = "Empresa Existente",
                Email = "teste@empresa.com",
                Telefone = "987654321",
                Cnpj = "12345678000101",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword("senhaAntiga")
            });
            await context.SaveChangesAsync();
        }

        // Act
        var result = await _controller.Register(empresaDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Já existe uma empresa com esse email.", badRequestResult.Value);
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        var empresaDto = new EmpresaRegisterDTO
        {
            Nome = "Empresa Teste",
            Email = "teste@empresa.com",
            Telefone = "123456789",
            Cnpj = "",  
            Senha = "senhaSegura"
        };

        _controller.ModelState.AddModelError("Cnpj", "O CNPJ é obrigatório.");

        // Act
        var result = await _controller.Register(empresaDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var validationErrors = Assert.IsType<SerializableError>(badRequestResult.Value);
        Assert.True(validationErrors.ContainsKey("Cnpj"));
    }
}
