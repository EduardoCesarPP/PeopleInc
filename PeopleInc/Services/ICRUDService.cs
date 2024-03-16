using Microsoft.EntityFrameworkCore;

namespace PeopleInc.Services
{
    public interface ICRUDService<T> /*where T : Identificavel*/
    {
        public void Cadastrar(T registro);
        public void Adicionar(T registro);
        public T RecuperaPorId(long id);
        public IEnumerable<T> Listar(int skip, int take);
        public void Atualizar(long id, T registro);
        public void Deleta(long id);
        public void Salvar();
        public void ValidarCampos(T registro);
    }
}