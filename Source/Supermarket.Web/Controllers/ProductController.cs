using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supermarket.Business.Contracts;
using Supermarket.Web.Models;
using System.Collections.Generic;
using Supermarket.Common.Helpers;
using Supermarket.Domain.Entities;

namespace Supermarket.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public ProductController(IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
            _productService = productService;
        }

        [HttpGet]
        public IActionResult List(string message)
        {
            var response = _productService.GetAll();
            var model = _mapper.Map<List<ProductViewModel>>(response.Result);

            if (message.IsNotEmpty())
            {
                ViewBag.Message = message;
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(ProductViewModel model)
        {
            var entity = _mapper.Map<Product>(model);
            var response = _productService.Add(entity);
            if (response.IsSucceed)
            {
                return RedirectToAction("Detail", new { response.Result.Id });
            }

            ViewBag.Warning = response.ErrorMessage;
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var guid = id.ToGuid();
            var response = _productService.GetById(guid);
            if (response.IsSucceed)
            {
                var model = _mapper.Map<ProductViewModel>(response.Result);
                return View(model);
            }

            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel model)
        {
            var entity = _mapper.Map<Product>(model);
            var response = _productService.Update(entity);
            if (response.IsSucceed)
            {
                return RedirectToAction("Detail", new { response.Result.Id });
            }

            ViewBag.Warning = response.ErrorMessage;
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(string productId)
        {
            var guid = productId.ToGuid();
            var product = _productService.GetById(guid).Result;
            var response = _productService.Remove(product);
            if (response.IsSucceed)
            {
                return RedirectToAction("List",new {message = response.SuccessMessage});
            }

            return RedirectToAction("List", new { Message = response.ErrorMessage });
        }

        [HttpGet]
        public IActionResult Detail(string id)
        {
            var guid = id.ToGuid();
            var response = _productService.GetById(guid);
            if (response.IsSucceed)
            {
                var model = _mapper.Map<ProductViewModel>(response.Result);
                return View(model);
            }

            return RedirectToAction("List", new { Message = response.ErrorMessage });
        }
    }
}