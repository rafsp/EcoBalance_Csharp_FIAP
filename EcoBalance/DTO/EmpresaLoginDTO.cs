using System.ComponentModel.DataAnnotations;

namespace EcoBalance.DTO
{
    public class EmpresaLoginDTO
    {
        [Required(ErrorMessage = "O email é obrigatório.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Senha { get; set; }
    }
}
