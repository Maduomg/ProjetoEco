using ProjetoEcommerce.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ProjetoEcommerce.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly LoginRepositorio _loginRepositorio;

        public UsuarioController(LoginRepositorio loginRepositorio)
        {
            _loginRepositorio = loginRepositorio;
        }
        public IActionResult login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string senha)
        {
            var usuario = _loginRepositorio.ObterUsuario(email);

            if (usuario != null && usuario.Senha == senha)
            {
                return RedirectToAction("Index", "Produto");
            }

            ModelState.AddModelError("", "Email ou senha inválidos. ");

            return View();
        }
    }
}
