using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;


namespace API
{
    [ApiController]
    [Route("api/emprestimo")]
    public class EmprestimoController : ControllerBase
    {
        private readonly AppDataContext _ctx;

        public EmprestimoController(AppDataContext ctx)
        {
            _ctx = ctx;
        }


        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            try
            {
                List<Emprestimo> emprestimos = _ctx.Emprestimos.ToList();
                return emprestimos.Count == 0 ? NotFound() : Ok(emprestimos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("buscar/{id}")]
        public IActionResult Buscar(int id)
        {
            try
            {
                Emprestimo? emprestimo = _ctx.Emprestimos.FirstOrDefault(x => x.EmprestimoId == id);
                if (emprestimo != null)
                {
                    return Ok(emprestimo);
                }
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost]
        [Route("cadastrar")]
        public IActionResult Cadastrar([FromBody] Emprestimo emprestimo)
        {
            try
            {
                Livro? livroEncontrado = _ctx.Livros.FirstOrDefault(x => x.LivroId == emprestimo.LivroId);

                if (livroEncontrado != null && livroEncontrado.Estoque > 0)
                {
                    int estoque = livroEncontrado.Estoque - 1;

                    livroEncontrado.Autor = livroEncontrado.Autor;
                    livroEncontrado.TotalPaginas = livroEncontrado.TotalPaginas;
                    livroEncontrado.Titulo = livroEncontrado.Titulo;
                    livroEncontrado.Descricao = livroEncontrado.Descricao;
                    livroEncontrado.Estoque = estoque;

                    _ctx.SaveChanges();

                    Usuario? usuario = _ctx.Usuarios.FirstOrDefault(x => x.UsuarioId == emprestimo.UsuarioId);


                    if (usuario != null && usuario.Ativo == 1)
                    {
                        usuario.Nome = usuario.Nome;
                        usuario.Endereco = usuario.Endereco;
                        usuario.Telefone = usuario.Telefone;
                        usuario.Ativo = 0;

                        _ctx.SaveChanges();

                        emprestimo.DataEmprestimo = DateTime.Now;
                        emprestimo.DataFinal = DateTime.Now.AddDays(7);

                        _ctx.Emprestimos.Add(emprestimo);
                        _ctx.SaveChanges();

                        return Created("", emprestimo);
                    }
                    else
                    {
                        return BadRequest("Usuário se encontra com emprestimo ativo!.");
                    }
                }
                return BadRequest("Livro não encontrado ou sem estoque disponível.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("devolucao")]
        public IActionResult Devolucao([FromBody] Emprestimo emprestimo)
        {
            try
            {
                Livro? livroEncontrado = _ctx.Livros.FirstOrDefault(x => x.LivroId == emprestimo.LivroId);

                if (livroEncontrado != null)
                {
                    int estoque = livroEncontrado.Estoque + 1;

                    livroEncontrado.Autor = livroEncontrado.Autor;
                    livroEncontrado.TotalPaginas = livroEncontrado.TotalPaginas;
                    livroEncontrado.Titulo = livroEncontrado.Titulo;
                    livroEncontrado.Descricao = livroEncontrado.Descricao;
                    livroEncontrado.Estoque = estoque;

                    _ctx.SaveChanges();

                    Usuario? usuario = _ctx.Usuarios.FirstOrDefault(x => x.UsuarioId == emprestimo.UsuarioId);

                    usuario.Nome = usuario.Nome;
                    usuario.Endereco = usuario.Endereco;
                    usuario.Telefone = usuario.Telefone;
                    usuario.Ativo = 1;

                    _ctx.SaveChanges();
                    return Ok("Livro devolvido com sucesso.");
                }else {
                    return BadRequest("Livro inválido.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete]
        [Route("deletar/{id}")]
        public IActionResult Deletar(int id)
        {
            try
            {
                Emprestimo? emprestimoEncontrado = _ctx.Emprestimos.Find(id);
                if (emprestimoEncontrado != null)
                {
                    _ctx.Emprestimos.Remove(emprestimoEncontrado);
                    _ctx.SaveChanges();
                    return Ok("Livro removido com sucesso!");
                }
                return NotFound("Não encontrado");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
