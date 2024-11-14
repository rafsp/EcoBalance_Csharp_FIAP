using System.ComponentModel.DataAnnotations;

namespace EcoBalance.DTO
{
    public class EmpresaUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [StringLength(100, ErrorMessage = "O email não pode exceder 100 caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [StringLength(30, ErrorMessage = "O telefone não pode exceder 30 caracteres.")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O cnpj é obrigatório.")]
        [StringLength(30, ErrorMessage = "O cnpj não pode exceder 30 caracteres.")]
        public string Cnpj { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        public string Senha { get; set; }
    }
}
