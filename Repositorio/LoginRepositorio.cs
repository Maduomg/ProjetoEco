using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using System.Data;



namespace ProjetoEcommerce.Repositorio
{
    public class LoginRepositorio(IConfiguration configuration)
    {

        private readonly string _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");

        public tbUsuarios ObterUsuario(string email)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new ("SELECT * FROM tbUsuarios WHERE Email = @email", conexao);

                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;

                using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    tbUsuarios usuario = null;

            

                    if (dr.Read())
                    {
                        usuario = new tbUsuarios
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Nome = dr["Nome"].ToString(),
                            Email = dr["Email"].ToString(),
                            Senha = dr["Senha"].ToString()
                        };


                    }

                    return usuario;
                }

            }

        }
    }
}
