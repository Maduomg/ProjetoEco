using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorio;

namespace ProjetoEcommerce.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly ProdutoRepositorio _produtoRepositorio;

        public ProdutoController(ProdutoRepositorio produtoRepositorio)
        {
     
            _produtoRepositorio = produtoRepositorio;
        }

        public IActionResult Index()
        {
            /* Retorna a View padrão associada a esta Action,
             passando como modelo a lista de todos os clientes obtida do repositório.*/
            return View(_produtoRepositorio.TodosProdutos());
        }


        /* Action para exibir o formulário de cadastro de cliente (via Requisição GET)*/
        public IActionResult CadastrarProduto()
        {
            //retorna a Página
            return View();
        }

        // Action que recebe e processa os dados que serão enviados pelo formulário de cadastro de cliente (via Requisição POST)
        [HttpPost]
        public IActionResult CadastrarProduto(tbProdutos produto)
        {

            /* O parâmetro 'cliente' recebe os dados enviados pelo formulário,
             que são automaticamente mapeados para as propriedades da classe Cliente.
             Chama o método no repositório para cadastrar o novo cliente no sistema.*/
            _produtoRepositorio.Cadastrar(produto);

            //redireciona para pagina Index 'nameof(Index)' garante que o nome da Action seja usado corretamente,
            return RedirectToAction(nameof(Index));
        }

        /* Action para exibir o formulário de edição de um cliente específico (via Requisição GET)
         Este método recebe o 'id' do cliente a ser editado como parâmetro.*/
        public IActionResult EditarProduto(int id)
        {
            // Obtém o cliente específico do repositório usando o ID fornecido.
            var produto = _produtoRepositorio.ObterProduto(id);

            // Verifica se o cliente foi encontrado. É uma boa prática tratar casos onde o ID é inválido.
            if (produto == null)
            {
                // Você pode retornar um NotFound (código de status 404) ou outra resposta apropriada.
                return NotFound();
            }

            // Retorna a View associada a esta Action (EditarCliente.cshtml),
            return View(produto);
        }


        // Carrega a liista de Cliente que envia a alteração(post)

        [HttpPost]
        [ValidateAntiForgeryToken] // Essencial para segurança contra ataques CSRF
        /*[Bind] para especificar explicitamente quais propriedades do objeto Cliente podem ser vinculadas a partir dos dados do formulário.
        Isso é uma boa prática de segurança para evitar o overposting (onde um usuário malicioso pode enviar dados para propriedades
        que você não pretendia que fossem alteradas)*/
        public IActionResult EditarProduto(int id, [Bind("Id, Nome, Descricao, Preco, Quantidade")] tbProdutos produto)
        {
            // Verifica se o ID fornecido na rota corresponde ao ID do cliente no modelo.
            if (id != produto.Id)
            {
                return BadRequest(); // Retorna um erro 400 se os IDs não corresponderem.
            }
            if (ModelState.IsValid)
            {
                //try /catch = tratamento de erros 
                try
                {
                    // Verifica se o cliente com o Codigo fornecido existe no repositório.
                    if (_produtoRepositorio.Atualizar(produto))
                    {
                        //redireciona para a pagina index quando alterar
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {
                    // Adiciona um erro ao ModelState para exibir na View.
                    ModelState.AddModelError("", "Ocorreu um erro ao Editar.");
                    // Retorna a View com o modelo para exibir a mensagem de erro e os dados do formulário.
                    return View(produto);
                }
            }
            // Se o ModelState não for válido, retorna a View com os erros de validação.
            return View(produto);
        }


        public IActionResult ExcluirProduto(int id)
        {
            // Obtém o cliente específico do repositório usando o Codigo fornecido.
            _produtoRepositorio.Excluir(id);
            // Retorna a View de confirmação de exclusão, passando o cliente como modelo.
            return RedirectToAction(nameof(Index));
        }
    }
}
