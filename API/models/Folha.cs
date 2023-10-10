using System;

namespace API.Models
{
    public class Folha
    {   
        public int FolhaId { get; set; }
        public double? valor { get; set; } 
        public double? quantidade { get; set; } 
        public int mes { get; set; }
        public int ano { get; set; } 

        public double? salarioBruto { get; set; } 

        public double? impostoIrrf { get; set; } 

        public double? impostoInss { get; set; } 

        public double? impostoFgts { get; set; }

        public double? salarioLiquido { get; set; } 


        public Funcionario? Funcionario { get; set; }
        public int FuncionarioId { get; set; }
    }
}

