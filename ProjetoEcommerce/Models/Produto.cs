namespace ProjetoEcommerce.Models
{

    public class Produto
    {
        public int CodProd { get; set; } 
        public string? NomeProd { get; set; }
        public string? DescProd { get; set; }
        public int? QuantProd { get; set; }
        public Double? PrecoProd { get; set; }
        public List<Produto>? ListaProduto { get; set; }

    }
}
