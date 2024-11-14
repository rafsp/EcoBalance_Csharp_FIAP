using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EcoBalance.Models
{
    [Table("ecobalance_consumos")]
    public class ConsumoEnergia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public double Consumo { get; set; }
        public int EmpresaId { get; set; }
        public Empresa? Empresa { get; set; }
    }
}
