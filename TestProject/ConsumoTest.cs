using EcoBalance.Controllers;
using EcoBalance.Models;
using EcoBalance.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace EcoBalance.Tests
{
    public class ConsumosControllerTests
    {
        private readonly ConsumosController _controller;
        private readonly DbContextOptions<OracleDbContext> _options;

        public ConsumosControllerTests()
        {
            _options = new DbContextOptionsBuilder<OracleDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var context = new OracleDbContext(_options);

            _controller = new ConsumosController(context);
        }

        [Fact]
        public async Task GetConsumos_ShouldReturnOk_WhenConsumosExist()
        {
            // Arrange
            using (var context = new OracleDbContext(_options))
            {
                context.Consumos.Add(new ConsumoEnergia { Id = 1, EmpresaId = 1, Consumo = 100, Data = DateTime.Now });
                context.Consumos.Add(new ConsumoEnergia { Id = 2, EmpresaId = 2, Consumo = 200, Data = DateTime.Now });
                await context.SaveChangesAsync();
            }

            // Act
            var result = await _controller.GetConsumos();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<ConsumoEnergia>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var consumos = Assert.IsAssignableFrom<IEnumerable<ConsumoEnergia>>(okResult.Value);
            Assert.Equal(2, consumos.Count());
        }

        [Fact]
        public async Task GetConsumoEnergia_ShouldReturnNotFound_WhenConsumoDoesNotExist()
        {
            // Act
            var result = await _controller.GetConsumoEnergia(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ConsumoEnergia>>(result);
            var notFoundResult = Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task PostConsumoEnergia_ShouldReturnCreatedAtAction_WhenValid()
        {
            // Arrange
            var consumo = new ConsumoEnergia
            {
                EmpresaId = 1,
                Consumo = 150,
                Data = DateTime.Now
            };

            // Act
            var result = await _controller.PostConsumoEnergia(consumo);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ConsumoEnergia>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<ConsumoEnergia>(createdAtActionResult.Value);
            Assert.Equal(consumo.Consumo, returnValue.Consumo);
            Assert.Equal(consumo.Data, returnValue.Data);
        }

        [Fact]
        public async Task PutConsumoEnergia_ShouldReturnNoContent_WhenUpdatedSuccessfully()
        {
            // Arrange
            using (var context = new OracleDbContext(_options))
            {
                context.Consumos.Add(new ConsumoEnergia { Id = 1, EmpresaId = 1, Consumo = 100, Data = DateTime.Now });
                await context.SaveChangesAsync();
            }

            var updatedConsumo = new ConsumoEnergia
            {
                Id = 1,
                EmpresaId = 1,
                Consumo = 250,
                Data = DateTime.Now
            };

            // Act
            var result = await _controller.PutConsumoEnergia(1, updatedConsumo);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteConsumoEnergia_ShouldReturnNoContent_WhenDeletedSuccessfully()
        {
            // Arrange
            using (var context = new OracleDbContext(_options))
            {
                context.Consumos.Add(new ConsumoEnergia { Id = 1, EmpresaId = 1, Consumo = 100, Data = DateTime.Now });
                await context.SaveChangesAsync();
            }

            // Act
            var result = await _controller.DeleteConsumoEnergia(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteConsumoEnergia_ShouldReturnNotFound_WhenConsumoDoesNotExist()
        {
            // Act
            var result = await _controller.DeleteConsumoEnergia(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
