using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleInc.Data;
using PeopleInc.Data.Dtos.Pessoa;
using PeopleInc.Models;
using PeopleInc.Services;

namespace PeopleInc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PessoaController : ControllerBase
    {
        private IMapper _mapper;
        private ICRUDService<Pessoa> _pessoasService;
        private ICSVService _csvService;
        public PessoaController(IMapper mapper, ICRUDService<Pessoa> pessoasService, ICSVService csvService)
        {
            _mapper = mapper;
            _pessoasService = pessoasService;
            _csvService = csvService;
        }
        [HttpPost]
        public IActionResult Cadastrar([FromBody] CadastrarPessoaDto pessoaDto)
        {
            try
            {
                Pessoa pessoa = _mapper.Map<Pessoa>(pessoaDto);
                _pessoasService.Cadastrar(pessoa);
                return CreatedAtAction(nameof(RecuperarPorId), new { id = pessoa.Id }, pessoa);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("csv")]
        public async Task<IActionResult> CadastrarCsv([FromForm] IFormFileCollection files)
        {
            bool isProcessing = false;
            try
            {
                ChecarArquivo(files);
                List<CadastrarPessoaDto> pessoas = _csvService.ReadCSV<CadastrarPessoaDto>(files[0].OpenReadStream()).ToList();
                isProcessing = true;
                foreach (CadastrarPessoaDto pessoaDto in pessoas)
                {
                    Pessoa pessoa = _mapper.Map<Pessoa>(pessoaDto);
                    _pessoasService.Adicionar(pessoa);
                }
                _pessoasService.Salvar();
                isProcessing = false;
                return NoContent();
            }
            catch (Exception ex)
            {
                if (isProcessing)
                {
                    return BadRequest(ex.Message + $" \nNenhum registro foi cadastrado.");
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        private void ChecarArquivo(IFormFileCollection files)
        {
            if (files.Count > 1)
            {
                throw new ArgumentException("O limite máximo é de 1 arquivo.");
            }
            if (Path.GetExtension(files[0].FileName) != ".csv")
            {
                var a = Path.GetExtension(files[0].FileName);
                throw new ArgumentException("O formato do arquivo deve ser csv.");
            }
            if ((files[0].Length / 1000000000) > 1)
            {
                throw new ArgumentException("O tamanho máximo do arquivo é de 1GB.");
            }
            if (_csvService.ReadCSV<CadastrarPessoaDto>(files[0].OpenReadStream()).ToList().Count == 0)
            {
                throw new ArgumentException("O arquivo foi informado sem nenhum registro.");
            }
        }
        [HttpGet("{id}")]
        public IActionResult RecuperarPorId(long id)
        {
            try
            {
                var pessoaDto = _mapper.Map<ConsultarPessoaDto>(_pessoasService.RecuperaPorId(id));
                if (pessoaDto == null) return NotFound();
                return Ok(pessoaDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult Listar([FromQuery] int skip = 0, [FromQuery] int take = 50, [FromQuery] string? nome = null)
        {
            try
            {
                var pessoas = _pessoasService.Listar(skip, take);
                var pessoasDto = _mapper.Map<List<ConsultarPessoaDto>>(pessoas.ToList());

                if (nome is not null && nome.Length > 0)
                {
                    nome = nome.ToUpper();
                    pessoasDto = (from pessoa in pessoasDto
                                  where pessoa.Nome.ToUpper().StartsWith(nome)
                                  select pessoa).ToList();

                    pessoasDto.AddRange((from pessoa in pessoasDto
                                         where pessoa.Nome.ToUpper().Contains(nome) &&
                                    !pessoa.Nome.ToUpper().StartsWith(nome)
                                         select pessoa).ToList());
                }
                return Ok(pessoasDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult AtualizaPessoa(int id, [FromBody] AlterarPessoaDto pessoaDto)
        {
            var pessoa = _mapper.Map<Pessoa>(pessoaDto);
            try
            {
                _pessoasService.Atualizar(id, pessoa);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("{id}")]
        public IActionResult AtualizaPessoaParcial(int id, JsonPatchDocument<AlterarPessoaDto> patch)
        {
            var pessoa = _pessoasService.RecuperaPorId(id);
            if (pessoa == null) return NotFound();
            var pessoaParaAtualizar = _mapper.Map<AlterarPessoaDto>(pessoa);
            patch.ApplyTo(pessoaParaAtualizar, ModelState);
            if (!TryValidateModel(pessoaParaAtualizar))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(pessoaParaAtualizar, pessoa);
            _pessoasService.Salvar();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeletaPessoa(int id)
        {
            try
            {
                _pessoasService.Deleta(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
