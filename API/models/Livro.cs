namespace API.Models;

public class Livro
{

        public int LivroId { get; set; } // Id do livro (pode ser gerado automaticamente pelo banco de dados)
        public string? Autor { get; set; } // Autor do livro
        public int TotalPaginas { get; set; } // Número total de páginas do livro
        public string? Titulo  { get; set; } // Título do livro
        public string? Descricao { get; set; } // Descrição do livro

        public int Estoque { get; set; } // Descrição do livro

}
