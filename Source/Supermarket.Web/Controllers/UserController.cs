using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using Supermarket.Business.Contracts;
using Supermarket.Web.Models;

namespace Supermarket.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;


        public UserController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel model)
        {
            var response = _userService.Login(model.Email, model.Password);
            if (response.IsSucceed)
            {
                var userClaims = new List<Claim>() {
                        new Claim(ClaimTypes.Name, response.Result.Name),
                        new Claim(ClaimTypes.Email, response.Result.Email)
                    };

                var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");

                var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                await HttpContext.SignInAsync(userPrincipal);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Warning = response.ErrorMessage;
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}