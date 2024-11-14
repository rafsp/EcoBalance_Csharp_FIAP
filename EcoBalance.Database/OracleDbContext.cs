using EcoBalance.Models;
using Microsoft.EntityFrameworkCore;

namespace EcoBalance.Database
{
    public class OracleDbContext : DbContext
    {
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<ConsumoEnergia> Consumos { get; set; }
        public DbSet<ProducaoEnergia> Producoes { get; set; }

        public OracleDbContext(DbContextOptions<OracleDbContext> options) : base(options)
        {
        }
    }
}
