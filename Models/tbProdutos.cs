namespace ProjetoEcommerce.Models
{

    public class tbProdutos
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }

        public List<tbProdutos> ?Produtos { get; set; }
    }

}
