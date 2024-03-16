using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using PeopleInc.Data;

namespace PeopleInc.Services
{
    public abstract class CRUDService<T, C> : ICRUDService<T> /*where T : Identificavel*/
    {
        protected PeopleIncContext _context;
        public void Cadastrar(T registro)
        {
            Adicionar(registro);
            Salvar();
        }
        public abstract void Adicionar(T registro);
        public abstract T? RecuperaPorId(long id);
        public abstract IEnumerable<T> Listar(int skip, int take);
        public abstract void Atualizar(long id, T registro);
        public abstract void Deleta(long id);
        public abstract void ValidarCampos(T registro);
        public void Salvar()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                SqliteException sqlException = (SqliteException)ex.InnerException;
                if (sqlException.SqliteErrorCode == 19 && sqlException.SqliteExtendedErrorCode == 2067)
                {
                    throw new ArgumentException("O e-mail informado já está cadastrado para outra pessoa.");
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
