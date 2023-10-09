using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API;

[ApiController]
[Route("api/usuario")]
public class UsuarioController : ControllerBase
{
    private readonly AppDataContext _ctx;
    public UsuarioController(AppDataContext ctx)
    {
        _ctx = ctx;
    }
    [HttpGet]
    [Route("listar")]

    public IActionResult Listar()
    {
        try
        {
            List<Usuario> usuarios = _ctx.Usuarios.ToList();
            return usuarios.Count == 0 ? NotFound() : Ok(usuarios);


        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    [HttpGet]
    [Route("buscar/{id}")]

    public IActionResult Buscar([FromRoute] int id)
    {
        try
        {
            Usuario? usuarioCadastrado = _ctx.Usuarios.FirstOrDefault(x => x.UsuarioId == id);
            if (usuarioCadastrado != null)
            {
                return Ok(usuarioCadastrado);
            }
            return NotFound();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    [Route("deletar/{id}")]
    public IActionResult Deletar([FromRoute] int id)
    {
        try
        {
            Usuario? usuarioCadastrado = _ctx.Usuarios.Find(id);
            if (usuarioCadastrado != null)
            {
                _ctx.Usuarios.Remove(usuarioCadastrado);
                _ctx.SaveChanges();
                return Ok();
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
    public IActionResult Cadastrar([FromBody] Usuario usuario)
    {
        try
        {
            _ctx.Add(usuario);
            _ctx.SaveChanges();
            
            return Created("", usuario);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

        [HttpPut]
        [Route("atualizar/{id}")]
        public IActionResult Atualizar(int id, [FromBody] Usuario usuarioAtualizado)
        {
            try
            {
                Usuario usuarioExistente = _ctx.Usuarios.Find(id) ?? throw new InvalidOperationException($"Usuario com id {id} não encontrado");

                if (usuarioExistente != null)
                {
                    usuarioExistente.Nome = usuarioAtualizado.Nome;
                    usuarioExistente.Endereco = usuarioAtualizado.Endereco;
                    usuarioExistente.Telefone = usuarioExistente.Telefone;
                    usuarioExistente.Ativo = usuarioAtualizado.Ativo;

                    _ctx.SaveChanges();
                    
                    return Ok(usuarioExistente);
                }

                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        


}







