using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoBalance.Database;
using EcoBalance.Models;
using Microsoft.AspNetCore.Authorization;

namespace EcoBalance.Controllers
{
    /// <summary>
    /// API Controller para Consumo de Energia
    /// </summary>
    [Route("ecobalance/[controller]")]
    [ApiController]
    [Tags("Consumo de Energia")]
    public class ConsumosController : ControllerBase
    {
        private readonly OracleDbContext _context;

        /// <summary>
        /// Construtor do controlador
        /// </summary>
        /// <param name="context">Instância do contexto OracleDbContext para interagir com a base de dados</param>
        public ConsumosController(OracleDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os registros de Consumo de Energia
        /// </summary>
        /// <returns>Lista de todos os consumos de energia com informações sobre a empresa associada</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ConsumoEnergia>>> GetConsumos()
        {
            return await _context.Consumos
                .Include(ce => ce.Empresa)
                .ToListAsync();
        }

        /// <summary>
        /// Obtém um único registro de Consumo de Energia
        /// </summary>
        /// <param name="id">ID do registro de consumo de energia</param>
        /// <returns>O consumo de energia específico se encontrado</returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ConsumoEnergia>> GetConsumoEnergia(int id)
        {
            var consumo = await _context.Consumos
                .Include(ce => ce.Empresa)
                .FirstOrDefaultAsync(ce => ce.Id == id);

            if (consumo == null)
            {
                return NotFound();
            }

            return consumo;
        }

        /// <summary>
        /// Criar um novo registro de Consumo de Energia
        /// </summary>
        /// <param name="consumoEnergia">Objeto contendo as informações do novo consumo de energia</param>
        /// <returns>Retorna o consumo de energia recém-criado</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ConsumoEnergia>> PostConsumoEnergia(ConsumoEnergia consumoEnergia)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Consumos.Add(consumoEnergia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetConsumoEnergia), new { id = consumoEnergia.Id }, consumoEnergia);
        }

        /// <summary>
        /// Atualiza um registro existente de Consumo de Energia
        /// </summary>
        /// <param name="id">ID do registro a ser atualizado</param>
        /// <param name="consumoEnergia">Objeto contendo as novas informações de consumo de energia</param>
        /// <returns>Status de resposta para a atualização do registro</returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutConsumoEnergia(int id, ConsumoEnergia consumoEnergia)
        {
            if (id != consumoEnergia.Id)
            {
                return BadRequest();
            }

            _context.Entry(consumoEnergia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsumoEnergiaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Deleta um registro de Consumo de Energia
        /// </summary>
        /// <param name="id">ID do registro a ser deletado</param>
        /// <returns>Status de resposta para a exclusão do registro</returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteConsumoEnergia(int id)
        {
            var consumoEnergia = await _context.Consumos.FindAsync(id);
            if (consumoEnergia == null)
            {
                return NotFound();
            }

            _context.Consumos.Remove(consumoEnergia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Verifica se um registro de Consumo de Energia existe
        /// </summary>
        /// <param name="id">ID do registro de consumo de energia a ser verificado</param>
        /// <returns>Retorna verdadeiro se o registro existir</returns>
        private bool ConsumoEnergiaExists(int id)
        {
            return _context.Consumos.Any(e => e.Id == id);
        }
    }
}
