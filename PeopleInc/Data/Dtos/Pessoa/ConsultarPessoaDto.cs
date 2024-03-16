namespace PeopleInc.Data.Dtos.Pessoa
{
    public class ConsultarPessoaDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public string? Email { get; set; }
    }
}
