using MySql.Data.MySqlClient;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorio;
using System.Data;



namespace ProjetoEcommerce.Repositorio
{
       // Define a classe responsável por interagir com os dados de clientes no banco de dados
        public class LoginRepositorio(IConfiguration configuration)
        {
            // Declara uma variável privada somente leitura para armazenar a string de conexão com o MySQL
            private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");


            // Método para cadastrar um novo cliente no banco de dados
            public void Cadastrar(Usuario usuario)
            {
                // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    // Abre a conexão com o banco de dados MySQL
                    conexao.Open();
                    // Cria um novo comando SQL para inserir dados na tabela 'cliente'
                    MySqlCommand cmd = new MySqlCommand("insert into usuario (Nome,Email,Senha) values (@nome, @email, @senha)", conexao); // @: PARAMETRO
                                                                                                                                                 // Adiciona um parâmetro para o nome, definindo seu tipo e valor
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = usuario.Nome;
                    // Adiciona um parâmetro para o telefone, definindo seu tipo e valor
                    cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = usuario.Email;
                    // Adiciona um parâmetro para o email, definindo seu tipo e valor
                    cmd.Parameters.Add("@senha", MySqlDbType.VarChar).Value = usuario.Senha;
                    // Executa o comando SQL de inserção e retorna o número de linhas afetadas
                    cmd.ExecuteNonQuery();
                    // Fecha explicitamente a conexão com o banco de dados (embora o 'using' já faça isso)
                    conexao.Close();
                }
            }

        // Método para Editar (atualizar) os dados de um cliente existente no banco de dados
        public bool Atualizar(Usuario usuario)
        {
            try
            {
                // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    // Abre a conexão com o banco de dados MySQL
                    conexao.Open();
                    // Cria um novo comando SQL para atualizar dados na tabela 'cliente' com base no código
                    MySqlCommand cmd = new MySqlCommand("Update cliente set Nome=@nome, Email=@email, Senha=@senha " + " where Cod=@codigo ", conexao);
                    // Adiciona um parâmetro para o código do cliente a ser atualizado, definindo seu tipo e valor
                    cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = usuario.Id;
                    // Adiciona um parâmetro para o novo nome, definindo seu tipo e valor
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = usuario.Nome;
                    // Adiciona um parâmetro para o novo telefone, definindo seu tipo e valor
                    cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = usuario.Email;
                    // Adiciona um parâmetro para o novo email, definindo seu tipo e valor
                    cmd.Parameters.Add("@senha", MySqlDbType.VarChar).Value = usuario.Senha;
                    // Executa o comando SQL de atualização e retorna o número de linhas afetadas
                    //executa e verifica se a alteração foi realizada
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0; // Retorna true se ao menos uma linha foi atualizada
                   
                }
            }
            catch (MySqlException ex)
            {
                // Logar a exceção (usar um framework de logging como NLog ou Serilog)
                Console.WriteLine($"Erro ao atualizar usuario: {ex.Message}");
                return false; // Retorna false em caso de erro
               
            }
        }

            // Método para listar todos os clientes do banco de dados
            public IEnumerable<Usuario> TodosUsuario()
            {
                // Cria uma nova lista para armazenar os objetos Cliente
                List<Usuario> Usuariolist = new List<Usuario>();

                // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    // Abre a conexão com o banco de dados MySQL
                    conexao.Open();
                    // Cria um novo comando SQL para selecionar todos os registros da tabela 'cliente'
                    MySqlCommand cmd = new MySqlCommand("SELECT * from usuario", conexao);

                    // Cria um adaptador de dados para preencher um DataTable com os resultados da consulta
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    // Cria um novo DataTable
                    DataTable dt = new DataTable();
                    // metodo fill- Preenche o DataTable com os dados retornados pela consulta
                    da.Fill(dt);
                    // Fecha explicitamente a conexão com o banco de dados 
                    conexao.Close();

                    // interage sobre cada linha (DataRow) do DataTable
                    foreach (DataRow dr in dt.Rows)
                    {
                        // Cria um novo objeto Cliente e preenche suas propriedades com os valores da linha atual
                        Usuariolist.Add(
                                    new Usuario
                                    {
                                        Id = Convert.ToInt32(dr["id"]), // Converte o valor da coluna "codigo" para inteiro
                                        Nome = ((string)dr["Nome"]), // Converte o valor da coluna "nome" para string
                                        Email = ((string)dr["Email"]), // Converte o valor da coluna "telefone" para string
                                        Senha = ((string)dr["Senha"]), // Converte o valor da coluna "email" para string
                                    });
                    }
                    // Retorna a lista de todos os clientes
                    return Usuariolist;
                }
            }

            // Método para buscar um cliente específico pelo seu código (Codigo)
            public Usuario ObterUsuario(int Codigo)
            {
                // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    // Abre a conexão com o banco de dados MySQL
                    conexao.Open();
                    // Cria um novo comando SQL para selecionar um registro da tabela 'cliente' com base no código
                    MySqlCommand cmd = new MySqlCommand("SELECT * from usuario where Id=@codigo ", conexao);

                    // Adiciona um parâmetro para o código a ser buscado, definindo seu tipo e valor
                    cmd.Parameters.AddWithValue("@codigo", Codigo);

                    // Cria um adaptador de dados (não utilizado diretamente para ExecuteReader)
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                    // Declara um leitor de dados do MySQL
                    MySqlDataReader dr;
                // Cria um novo objeto Cliente para armazenar os resultados
                 Usuario usuario = new Usuario();

                    /* Executa o comando SQL e retorna um objeto MySqlDataReader para ler os resultados
                    CommandBehavior.CloseConnection garante que a conexão seja fechada quando o DataReader for fechado*/

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    // Lê os resultados linha por linha
                    while (dr.Read())
                    {
                        // Preenche as propriedades do objeto Cliente com os valores da linha atual
                        usuario.Id = Convert.ToInt32(dr["Id"]);//propriedade Codigo e convertendo para int
                        usuario.Nome = (string)(dr["Nome"]); // propriedade Nome e passando string
                        usuario.Email = (string)(dr["Email"]); //propriedade telefone e passando string
                        usuario.Senha = (string)(dr["Senha"]); //propriedade email e passando string
                    }
                    // Retorna o objeto Cliente encontrado (ou um objeto com valores padrão se não encontrado)
                    return usuario;
                }
            }


            // Método para excluir um cliente do banco de dados pelo seu código (ID)
            public void Excluir(int Id)
            {
                // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    // Abre a conexão com o banco de dados MySQL
                    conexao.Open();

                    // Cria um novo comando SQL para deletar um registro da tabela 'cliente' com base no código
                    MySqlCommand cmd = new MySqlCommand("delete from usuario where Id=@codigo", conexao);

                    // Adiciona um parâmetro para o código a ser excluído, definindo seu tipo e valor
                    cmd.Parameters.AddWithValue("@codigo", Id);

                    // Executa o comando SQL de exclusão e retorna o número de linhas afetadas
                    int i = cmd.ExecuteNonQuery();

                    conexao.Close(); // Fecha  a conexão com o banco de dados
                }
            }
        }
}
