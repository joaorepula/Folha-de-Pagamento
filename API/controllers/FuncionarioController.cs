using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;


namespace API
{
    [ApiController]
    [Route("api/funcionario")]
    public class FuncionarioController : ControllerBase
    {
        private readonly AppDataContext _ctx;

        public FuncionarioController(AppDataContext ctx)
        {
            _ctx = ctx;
        }
        

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            try
            {
                List<Funcionario> funcionarios = _ctx.Funcionario.ToList();
                return funcionarios.Count == 0 ? NotFound() : Ok(funcionarios);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("cadastrar")]
        public IActionResult Cadastrar([FromBody] Funcionario funcionario)
        {
            try
            {
                _ctx.Funcionario.Add(funcionario);
                _ctx.SaveChanges();
                return Created("", funcionario);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
