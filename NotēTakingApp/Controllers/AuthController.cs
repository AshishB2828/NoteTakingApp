using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteTakingApp.Models.DTO;
using NoteTakingApp.Repository.interfaces;

namespace NoteTakingApp.Controllers
{
    public class AuthController : Controller
    {

        private readonly IAuthRepository _authRepository;


        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegistartionData();
            return View(model);
        }

        [HttpPost]
        public IActionResult Register(RegistartionData registartionData)
        {
            if (!ModelState.IsValid)
            {
                return View(registartionData);
            }
            registartionData.UserName = registartionData.Email;
            var result = _authRepository.Registration(registartionData);
            if (result.IsSuccess)
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                TempData["Msg"] = result.ResponseMessage;
                return RedirectToAction(nameof(Register));
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginData();
            return View(model);
        }

        [HttpPost]
        public IActionResult Login(LoginData loginData)
        {
            if (!ModelState.IsValid)
            {
                return View(loginData);
            }

            var result = _authRepository.Login(loginData);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index", "Notes");
            }
            else
            {
                TempData["Msg"] = result.ResponseMessage;
                return RedirectToAction(nameof(Login));
            }
        }

        [Authorize]
        public IActionResult Logout()
        {
            _authRepository.Logout();
            return RedirectToAction(nameof(Login));
        }

    }
}
