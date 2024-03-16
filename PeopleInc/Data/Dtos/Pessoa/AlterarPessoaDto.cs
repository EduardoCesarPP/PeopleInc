using System.ComponentModel.DataAnnotations;

namespace PeopleInc.Data.Dtos.Pessoa
{
    public class AlterarPessoaDto
    {
        [Required(ErrorMessage = "O campo nome é obrigatório.")]
        [MaxLength(50, ErrorMessage = "O campo nome deve ter no máximo 50 caracteres")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo idade é obrigatório.")]
        [Range(1, 150, ErrorMessage = "O campo idade deve estar entre 1 e 150")]
        public int Idade { get; set; }
        [MaxLength(150, ErrorMessage = "O campo email deve ter no máximo 150 caracteres")]
        public string? Email { get; set; }
    }
}
