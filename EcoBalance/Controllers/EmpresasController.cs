using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoBalance.Database;
using EcoBalance.Models;
using EcoBalance.DTO;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace EcoBalance.Controllers
{
    [Route("ecobalance/[controller]")]
    [ApiController]
    [Tags("Cadastro de Empresas")]
    public class EmpresasController : ControllerBase
    {
        private readonly OracleDbContext _context;
        private readonly IConfiguration _configuration;

        public EmpresasController(OracleDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Registra uma nova empresa.
        /// </summary>
        /// <param name="empresaDto">Os dados da empresa a ser registrada.</param>
        /// <returns>Retorna a empresa registrada com status 201 (Created).</returns>
        [HttpPost("Register")]
        public async Task<ActionResult<Empresa>> Register(EmpresaRegisterDTO empresaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_context.Empresas.Count(e => e.Email == empresaDto.Email) > 0)
                return BadRequest("Já existe uma empresa com esse email.");

            string senhaHash = BCrypt.Net.BCrypt.HashPassword(empresaDto.Senha);

            var empresa = new Empresa
            {
                Nome = empresaDto.Nome,
                Email = empresaDto.Email,
                Telefone = empresaDto.Telefone,
                Cnpj = empresaDto.Cnpj,
                SenhaHash = senhaHash
            };

            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmpresa), new { id = empresa.Id }, empresa);
        }

        /// <summary>
        /// Realiza o login de uma empresa e retorna um token JWT.
        /// </summary>
        /// <param name="loginDto">Os dados de login da empresa.</param>
        /// <returns>Retorna o token JWT com status 200 (OK).</returns>
        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(EmpresaLoginDTO loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.Email == loginDto.Email);
            if (empresa == null)
                return Unauthorized("Empresa não encontrada.");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Senha, empresa.SenhaHash);
            if (!isPasswordValid)
                return Unauthorized("Senha inválida.");

            var token = GenerateJwtToken(empresa);

            return Ok(new { Token = token });
        }

        /// <summary>
        /// Retorna a lista de todas as empresas cadastradas.
        /// </summary>
        /// <returns>Lista de empresas.</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Empresa>>> GetEmpresas()
        {
            return await _context.Empresas.ToListAsync();
        }

        /// <summary>
        /// Retorna os dados de uma empresa específica.
        /// </summary>
        /// <param name="id">ID da empresa a ser retornada.</param>
        /// <returns>Detalhes da empresa.</returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Empresa>> GetEmpresa(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);

            if (empresa == null)
            {
                return NotFound();
            }

            return empresa;
        }

        /// <summary>
        /// Atualiza os dados de uma empresa.
        /// </summary>
        /// <param name="id">ID da empresa a ser atualizada.</param>
        /// <param name="empresaDto">Os dados atualizados da empresa.</param>
        /// <returns>Retorna status 204 (No Content) em caso de sucesso.</returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutEmpresa(int id, EmpresaUpdateDTO empresaDto)
        {
            if (id != empresaDto.Id)
            {
                return BadRequest();
            }

            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
                return NotFound();

            if (_context.Empresas.Any(e => e.Email == empresaDto.Email && e.Id != id))
                return BadRequest("O email já está em uso por outra empresa.");

            empresa.Nome = empresaDto.Nome;
            empresa.Email = empresaDto.Email;
            empresa.Telefone = empresaDto.Telefone;
            empresa.Cnpj = empresaDto.Cnpj;

            if (!string.IsNullOrEmpty(empresaDto.Senha))
            {
                empresa.SenhaHash = BCrypt.Net.BCrypt.HashPassword(empresaDto.Senha);
            }

            _context.Entry(empresa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpresaExists(id))
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
        /// Deleta uma empresa específica.
        /// </summary>
        /// <param name="id">ID da empresa a ser deletada.</param>
        /// <returns>Retorna status 204 (No Content) em caso de sucesso.</returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteEmpresa(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }

            _context.Empresas.Remove(empresa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmpresaExists(int id)
        {
            return _context.Empresas.Any(e => e.Id == id);
        }

        /// <summary>
        /// Gera um token JWT para uma empresa autenticada.
        /// </summary>
        /// <param name="empresa">A empresa que será autenticada.</param>
        /// <returns>Retorna o token JWT gerado.</returns>
        private string GenerateJwtToken(Empresa empresa)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetValue<string>("SecretKey");
            var issuer = jwtSettings.GetValue<string>("Issuer");
            var audience = jwtSettings.GetValue<string>("Audience");
            var expiryInHours = jwtSettings.GetValue<int>("ExpiryInHours");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, empresa.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Id", empresa.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(expiryInHours),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
