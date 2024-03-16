namespace PeopleInc.Models
{
    public class Pessoa
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public string? Email { get; set; }

        public void ValidarCampos()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                throw new ArgumentNullException("O campo nome é obrigatório.");
            }
            if (Nome.Length > 50)
            {
                throw new ArgumentOutOfRangeException("O campo nome deve ter no máximo 50 caracteres.");
            }
            if (Idade < 1 || Idade > 150)
            {
                throw new ArgumentException("O campo idade deve estar entre 1 e 150.");
            }
            if (Email.Length > 150)
            {
                throw new ArgumentException("O campo email deve ter no máximo 150 caracteres.");
            }
        }
    }
}
