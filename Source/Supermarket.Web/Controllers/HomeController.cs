﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using Supermarket.Business.Contracts;
using Supermarket.Web.Models;

namespace Supermarket.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public HomeController(IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var response = _productService.GetRecentlyAddedProducts();
            if (response.IsSucceed)
            {
                var model = _mapper.Map<List<ProductViewModel>>(response.Result);
                return View(model);
            }

            ViewBag.Message = response.ErrorMessage;
            return View();
        }
    }
}