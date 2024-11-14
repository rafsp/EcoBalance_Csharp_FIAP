using EcoBalance.Controllers;
using EcoBalance.Models;
using EcoBalance.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace EcoBalance.Tests
{
    public class ProducoesControllerTests
    {
        private readonly ProducoesController _controller;
        private readonly DbContextOptions<OracleDbContext> _options;

        public ProducoesControllerTests()
        {
            _options = new DbContextOptionsBuilder<OracleDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var context = new OracleDbContext(_options);
            _controller = new ProducoesController(context);
        }

        [Fact]
        public async Task GetProducoes_ShouldReturnOk_WhenProducoesExist()
        {
            // Arrange
            using (var context = new OracleDbContext(_options))
            {
                context.Producoes.Add(new ProducaoEnergia { Id = 1, EmpresaId = 1, Producao = 150, Data = DateTime.Now });
                context.Producoes.Add(new ProducaoEnergia { Id = 2, EmpresaId = 2, Producao = 250, Data = DateTime.Now });
                await context.SaveChangesAsync();
            }

            // Act
            var result = await _controller.GetProducoes();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<ProducaoEnergia>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var producoes = Assert.IsAssignableFrom<IEnumerable<ProducaoEnergia>>(okResult.Value);
            Assert.Equal(2, producoes.Count());
        }

        [Fact]
        public async Task GetProducaoEnergia_ShouldReturnNotFound_WhenProducaoDoesNotExist()
        {
            // Act
            var result = await _controller.GetProducaoEnergia(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ProducaoEnergia>>(result);
            var notFoundResult = Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task PostProducaoEnergia_ShouldReturnCreatedAtAction_WhenValid()
        {
            // Arrange
            var producao = new ProducaoEnergia
            {
                EmpresaId = 1,
                Producao = 300,
                Data = DateTime.Now
            };

            // Act
            var result = await _controller.PostProducaoEnergia(producao);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ProducaoEnergia>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<ProducaoEnergia>(createdAtActionResult.Value);
            Assert.Equal(producao.Producao, returnValue.Producao);
            Assert.Equal(producao.Data, returnValue.Data);
        }

        [Fact]
        public async Task PutProducaoEnergia_ShouldReturnNoContent_WhenUpdatedSuccessfully()
        {
            // Arrange
            using (var context = new OracleDbContext(_options))
            {
                context.Producoes.Add(new ProducaoEnergia { Id = 1, EmpresaId = 1, Producao = 150, Data = DateTime.Now });
                await context.SaveChangesAsync();
            }

            var updatedProducao = new ProducaoEnergia
            {
                Id = 1,
                EmpresaId = 1,
                Producao = 500,
                Data = DateTime.Now
            };

            // Act
            var result = await _controller.PutProducaoEnergia(1, updatedProducao);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProducaoEnergia_ShouldReturnNoContent_WhenDeletedSuccessfully()
        {
            // Arrange
            using (var context = new OracleDbContext(_options))
            {
                context.Producoes.Add(new ProducaoEnergia { Id = 1, EmpresaId = 1, Producao = 150, Data = DateTime.Now });
                await context.SaveChangesAsync();
            }

            // Act
            var result = await _controller.DeleteProducaoEnergia(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProducaoEnergia_ShouldReturnNotFound_WhenProducaoDoesNotExist()
        {
            // Act
            var result = await _controller.DeleteProducaoEnergia(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
