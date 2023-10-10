using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API
{
    [ApiController]
    [Route("api/folha")]
    public class FolhaController : ControllerBase
    {
        private readonly AppDataContext _ctx;

        public FolhaController(AppDataContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        [Route("listar")]
        public IActionResult Listar()
        {
            try
            {
                List<Folha> folhas = _ctx.Folha
                    .Include(f => f.Funcionario) // Carrega os dados do Funcionário
                    .ToList();

                if (folhas.Count == 0)
                {
                    return NotFound();
                }

                return Ok(folhas);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        
        [HttpPost]
        [Route("buscar/{cpf}/{mes}/{ano}")]
        public IActionResult ListarPorParametro(string cpf, int mes, int ano)
        {
            try
            {
                var folha = _ctx.Folha
                    .Include(f => f.Funcionario) // Carrega os dados do Funcionário
                    .SingleOrDefault(f => f.Funcionario.CPF == cpf && f.mes == mes && f.ano == ano);

                if (folha == null)
                {
                    return NotFound();
                }

                return Ok(folha);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("cadastrar")]
        public IActionResult Cadastrar([FromBody] Folha folha)
        {
            try
            {

                Funcionario? funcionario = _ctx.Funcionario.FirstOrDefault(x => x.FuncionarioId == folha.FuncionarioId);
            if(funcionario != null) {
                double salarioBruto = (double)(folha.valor * folha.quantidade);
                double irrf = 0;

                if (salarioBruto > 1903.99f && salarioBruto <= 2826.65f)
                {
                    irrf = (salarioBruto * 0.075) - 142.80;
                }
                else if (salarioBruto > 2826.65f && salarioBruto <= 3751.05f)
                {
                    irrf = salarioBruto * 0.15 - 354.80;
                }
                else if (salarioBruto > 3751.05f && salarioBruto <= 4664.68f)
                {
                    irrf = salarioBruto * 0.225 - 636.13;
                }
                else if (salarioBruto > 4664.68f)
                {
                    irrf = salarioBruto * 0.275 - 869.36;
                }

                double descontoINSS = 0;

                if (salarioBruto <= 1693.72)
                {
                    descontoINSS = salarioBruto * 0.08;
                }
                else if (salarioBruto >= 1693.73 && salarioBruto <= 2822.90)
                {
                    descontoINSS = salarioBruto * 0.09;
                }
                else if (salarioBruto >= 2822.91 && salarioBruto <= 5645.80)
                {
                    descontoINSS = salarioBruto * 0.12;
                }
                else if (salarioBruto >= 5645.81)
                {
                    descontoINSS = 5645.81 * 0.12;
                }

                double fgts = salarioBruto * 0.08;

                double salarioLiquido = salarioBruto - descontoINSS - irrf;

                folha.salarioBruto = salarioBruto;
                folha.impostoIrrf = irrf;
                folha.impostoInss = descontoINSS;
                folha.impostoFgts = fgts;
                folha.Funcionario = funcionario;
                folha.salarioLiquido = salarioLiquido;
                _ctx.Folha.Add(folha);
                _ctx.SaveChanges();

                return Created("", folha);
                }else {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
