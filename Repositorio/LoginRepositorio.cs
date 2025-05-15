using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorio;
using System.Data;



namespace ProjetoEcommerce.Repositorio
{
    public class LoginRepositorio(IConfiguration configuration)
    {

        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");

        public tbUsuarios ObterUsuario(string email)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from usuario where Id=@codigo ", conexao);
                cmd.Parameters.AddWithValue("@email", MySqlDbType.VarChar).Value = email;


                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;
                tbUsuarios usuario = new tbUsuarios();

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dr.Read())
                {
                    usuario = new tbUsuarios
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Nome = dr["Nome"].ToString(),
                        Email = dr["Email"].ToString(),
                        Senha = dr["Senha"].ToString(),
                    };


                }

                return usuario;
            }

        }
    }
}
