using EcoBalance.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcoBalance.Controllers
{
    [Route("ecobalance/[controller]")]
    [ApiController]
    public class ThingerController : ControllerBase
    {
        private readonly ThingerService _thingerService;

        public ThingerController(ThingerService thingerService)
        {
            _thingerService = thingerService;
        }

        /// <summary>
        /// Obtém o registro de produção e consumo diretamente do Thinger.io
        /// </summary>
        /// <returns>Retorna o consumo e produção detectados pelo dispositivo ESP32</returns>
        // GET: ecobalance/Thinger
        [HttpGet]
        public async Task<IActionResult> GetDataFromThinger()
        {
            string token = "token_da_api"; 

            try
            {
                var data = await _thingerService.GetDataFromThinger(token);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao acessar Thinger.io: {ex.Message}");
            }
        }
    }
}
