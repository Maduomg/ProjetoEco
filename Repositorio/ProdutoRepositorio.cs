using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Data;


namespace ProjetoEcommerce.Repositorio
{
    public class ProdutoRepositorio(IConfiguration configuration)
    {

        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");


        public void Cadastrar(tbProdutos produto)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("insert into tbProdutos (Nome, Descricao, Preco, Quantidade) values (@Nome, @Descricao, @Preco, @Quantidade)", conexao);
                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = produto.Nome;
                cmd.Parameters.Add("@Descricao", MySqlDbType.VarChar).Value = produto.Descricao;
                cmd.Parameters.Add("@Preco", MySqlDbType.Decimal).Value = produto.Preco;
                cmd.Parameters.Add("@Quantidade", MySqlDbType.Int32).Value = produto.Quantidade;
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
        public bool Atualizar(tbProdutos produto)
        {
            try
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                { conexao.Open();
                    MySqlCommand cmd = new MySqlCommand("Update tbProdutos set Nome=@Nome, Descricao=@Descricao, Preco=@Preco, Quantidade=@Quantidade " + "where Id=@id", conexao);
                    cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = produto.Id;
                    cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = produto.Nome;
                    cmd.Parameters.Add("@Descricao", MySqlDbType.VarChar).Value = produto.Descricao;
                    cmd.Parameters.Add("@Preco", MySqlDbType.Decimal).Value = produto.Preco;
                    cmd.Parameters.Add("@Quantidade", MySqlDbType.Int32).Value = produto.Quantidade;
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao atualizar produto: {ex.Message}");
                return false;

            }
        }
        public IEnumerable<tbProdutos> TodosProdutos()
        {
            List<tbProdutos> Produtolist = new List<tbProdutos>();

    
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from tbProdutos", conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conexao.Close();
                
                foreach (DataRow dr in dt.Rows)
                {
                    Produtolist.Add(
                                new tbProdutos
                                {
                                    Id = Convert.ToInt32(dr["Id"]), 
                                    Nome = ((string)dr["Nome"]), 
                                    Descricao = ((string)dr["Descricao"]), 
                                    Preco = ((decimal)dr["Preco"]), 
                                    Quantidade = ((int)dr["Quantidade"]),
                                });
                }

                return Produtolist;
            }
        }
        public tbProdutos ObterProduto(int id)
        {

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
     
                MySqlCommand cmd = new MySqlCommand("SELECT * from tbProdutos where Id=id", conexao);


                cmd.Parameters.AddWithValue("@id", id);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                tbProdutos produto = new tbProdutos();

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    produto.Id = Convert.ToInt32(dr["Id"]);
                    produto.Nome = (string)(dr["Nome"]); 
                    produto.Descricao = (string)(dr["Descricao"]); 
                    produto.Preco = (decimal)(dr["Preco"]); 
                    produto.Quantidade = (int)(dr["Quantidade"]);
                }
                return produto;
            }
        }
        public void Excluir(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("delete from tbProdutos where Id=@id", conexao);
                cmd.Parameters.AddWithValue("@id", Id);

                int i = cmd.ExecuteNonQuery();

                conexao.Close(); 
            }
        }
    }
}