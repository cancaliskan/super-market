using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supermarket.Business.Contracts;
using Supermarket.Common.Helpers;
using Supermarket.Web.Models;

namespace Supermarket.Web.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IBasketService _basketService;
        private readonly IUserService _userService;

        public BasketController(IMapper mapper, IBasketService basketService, IUserService userService)
        {
            _mapper = mapper;
            _basketService = basketService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Detail(string message)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var currentUserResponse = _userService.GetUserByEmail(userEmail);
            if (currentUserResponse.IsSucceed)
            {
                var response = _basketService.GetDetail(currentUserResponse.Result);
                if (response.IsSucceed)
                {
                    var model = new BasketViewModel
                    {
                        Products = _mapper.Map<List<ProductDetail>>(response.Result.Products),
                        Id = response.Result.Id
                    };
                    model.ProductCount = model.Products.Count;
                    model.TotalPrice = model.Products.Sum(x => x.UnitPrice);

                    model.Products = model.Products.GroupBy(p => p.Id)
                                          .Select(x => new ProductDetail()
                                          {
                                              Count = x.Count(),
                                              Id = x.First().Id,
                                              Name = x.First().Name,
                                              UnitPrice = x.First().UnitPrice,
                                              Total = x.Count() * x.First().UnitPrice
                                          }).ToList();

                    if (message.IsNotEmpty())
                    {
                        ViewBag.Message = message;
                    }

                    return View(model);
                }

                ViewBag.Message = response.ErrorMessage;
                return View();
            }

            ViewBag.Message = currentUserResponse.ErrorMessage;
            return View();
        }

        [HttpPost]
        public IActionResult Remove(string productId)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var currentUserResponse = _userService.GetUserByEmail(userEmail);
            if (currentUserResponse.IsSucceed)
            {
                var response = _basketService.Remove(currentUserResponse.Result, productId.ToGuid());
                if (response.IsSucceed)
                {
                    return RedirectToAction("Detail", new { message = response.SuccessMessage });
                }

                return RedirectToAction("Detail", new { message = response.ErrorMessage });
            }

            return RedirectToAction("Detail", new { message = currentUserResponse.ErrorMessage });
        }

        [HttpPost]
        public IActionResult CompleteOrder(string id)
        {
            var response = _basketService.CompleteOrder(id.ToGuid());
            if (response.IsSucceed)
            {
                return RedirectToAction("List", "Product", new { message = response.SuccessMessage });
            }

            return RedirectToAction("Detail", new { message = response.ErrorMessage });
        }
    }
}