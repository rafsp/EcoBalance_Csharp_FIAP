using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoBalance.Database;
using EcoBalance.Models;
using Microsoft.AspNetCore.Authorization;

namespace EcoBalance.Controllers
{
    /// <summary>
    /// API Controller para Produção de Energia
    /// </summary>
    [Route("ecobalance/[controller]")]
    [ApiController]
    [Tags("Produção de Energia")]
    public class ProducoesController : ControllerBase
    {
        private readonly OracleDbContext _context;

        /// <summary>
        /// Construtor do controlador
        /// </summary>
        /// <param name="context">Instância do contexto OracleDbContext para interagir com a base de dados</param>
        public ProducoesController(OracleDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os registros de Produção de Energia
        /// </summary>
        /// <returns>Lista de todos os registros de produção de energia com informações sobre a empresa associada</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProducaoEnergia>>> GetProducoes()
        {
            return await _context.Producoes
                .Include(pe => pe.Empresa)
                .ToListAsync();
        }

        /// <summary>
        /// Obtém um único registro de Produção de Energia
        /// </summary>
        /// <param name="id">ID do registro de produção de energia</param>
        /// <returns>O registro de produção de energia específico se encontrado</returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ProducaoEnergia>> GetProducaoEnergia(int id)
        {
            var producao = await _context.Producoes
                .Include(pe => pe.Empresa)
                .FirstOrDefaultAsync(pe => pe.Id == id);

            if (producao == null)
            {
                return NotFound();
            }

            return producao;
        }

        /// <summary>
        /// Cria um novo registro de Produção de Energia
        /// </summary>
        /// <param name="producaoEnergia">Objeto contendo as informações do novo registro de produção de energia</param>
        /// <returns>Retorna o registro de produção de energia recém-criado</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProducaoEnergia>> PostProducaoEnergia(ProducaoEnergia producaoEnergia)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Producoes.Add(producaoEnergia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducaoEnergia), new { id = producaoEnergia.Id }, producaoEnergia);
        }

        /// <summary>
        /// Atualiza um registro existente de Produção de Energia
        /// </summary>
        /// <param name="id">ID do registro a ser atualizado</param>
        /// <param name="producaoEnergia">Objeto contendo as novas informações de produção de energia</param>
        /// <returns>Status de resposta para a atualização do registro</returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutProducaoEnergia(int id, ProducaoEnergia producaoEnergia)
        {
            if (id != producaoEnergia.Id)
            {
                return BadRequest();
            }

            _context.Entry(producaoEnergia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProducaoEnergiaExists(id))
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
        /// Deleta um registro de Produção de Energia
        /// </summary>
        /// <param name="id">ID do registro a ser deletado</param>
        /// <returns>Status de resposta para a exclusão do registro</returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProducaoEnergia(int id)
        {
            var producaoEnergia = await _context.Producoes.FindAsync(id);
            if (producaoEnergia == null)
            {
                return NotFound();
            }

            _context.Producoes.Remove(producaoEnergia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Verifica se um registro de Produção de Energia existe
        /// </summary>
        /// <param name="id">ID do registro de produção de energia a ser verificado</param>
        /// <returns>Retorna verdadeiro se o registro existir</returns>
        private bool ProducaoEnergiaExists(int id)
        {
            return _context.Producoes.Any(e => e.Id == id);
        }
    }
}
