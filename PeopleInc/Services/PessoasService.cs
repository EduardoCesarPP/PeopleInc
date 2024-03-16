using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PeopleInc.Data;
using PeopleInc.Models;

namespace PeopleInc.Services
{
    public class PessoasService : CRUDService<Pessoa, PeopleIncContext>
    {
        public PessoasService(PeopleIncContext context)
        {
            _context = context;
        }
        public override Pessoa? RecuperaPorId(long id)
        {
            return _context.Pessoas.FirstOrDefault(pessoa => pessoa.Id == id);
        }
        public override IEnumerable<Pessoa> Listar(int skip, int take)
        {
            return _context.Pessoas.Skip(skip).Take(take);
        }
        public override void Adicionar(Pessoa registro)
        {
            ValidarCampos(registro);
            _context.Pessoas.Add(registro);
        }
        public override void Deleta(long id)
        {
            var pessoa = RecuperaPorId(id);
            if (pessoa == null) throw new KeyNotFoundException();
            _context.Remove(pessoa);
            Salvar();
        }
        public override void Atualizar(long id, Pessoa registro)
        {
            ValidarCampos(registro);
            if (RecuperaPorId(id) == null) throw new KeyNotFoundException();
            registro.Id = id;
            _context.ChangeTracker.Clear();
            _context.Pessoas.Update(registro);
            Salvar();
        }

        public override void ValidarCampos(Pessoa registro)
        {
            registro.ValidarCampos();
        }
    }
}
