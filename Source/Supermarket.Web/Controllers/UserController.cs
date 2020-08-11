using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Supermarket.Business.Contracts;
using Supermarket.Common.Helpers;
using Supermarket.Domain.Entities;
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
            var response = _userService.GetByEmail(model.Email);
            if (response.IsSucceed)
            {
                if (model.Password == CryptoHelper.Decrypt(response.Result.Password))
                {
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect password");
                    return View(model);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}