using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using NUnit.Framework;
using AutoMapper;
using Moq;

using Supermarket.Business.Contracts;
using Supermarket.Common.Contracts;
using Supermarket.Domain.Entities;
using Supermarket.Web.Controllers;
using Supermarket.Web.Models;

namespace Supermarket.Web.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        private Mock<IProductService> _productService;
        private Mock<IMapper> _mapper;
        private HomeController _homeController;

        [SetUp]
        public void Setup()
        {
            _productService = new Mock<IProductService>();
            _mapper = new Mock<IMapper>();
            _homeController = new HomeController(_mapper.Object, _productService.Object);
        }

        [Test]
        public void Index_GetRecentlyAddedProducts_Failed()
        {
            // arrange
            var response = new Response<List<Product>>
            {
                IsSucceed = false,
                ErrorMessage = "Something went wrong"
            };
            _productService.Setup(x => x.GetRecentlyAddedProducts()).Returns(response);

            // act
            var result = _homeController.Index() as ViewResult;
            var errorMessage = result?.ViewData.Values.FirstOrDefault();

            // assert
            Assert.AreEqual(response.ErrorMessage, errorMessage);
        }

        [Test]
        public void Index_GetRecentlyAddedProducts_Success()
        {
            // arrange
            var productViewModel = new ProductViewModel()
            {
                Name = "Test Product",
                Description = "Test",
                CreatedDate = DateTime.Now,
                IsActive = true,
                Stock = 5,
                UnitPrice = 33,
                Type = "Type",
            };
            var productViewModelList = new List<ProductViewModel> { productViewModel };

            var product = new Product()
            {
                Name = "Test Product",
                Description = "Test",
                CreatedDate = DateTime.Now,
                IsActive = true,
                Stock = 5,
                UnitPrice = 33,
                Type = "Type",
            };
            var productList = new List<Product> { product };

            var response = new Response<List<Product>>
            {
                IsSucceed = true,
                Result = productList,
                ErrorMessage = null
            };
            _productService.Setup(x => x.GetRecentlyAddedProducts()).Returns(response);
            _mapper.Setup(m => m.Map<List<ProductViewModel>>(response.Result)).Returns(productViewModelList);

            // act
            var result = _homeController.Index() as ViewResult;
            var model = (List<ProductViewModel>)result?.ViewData.Model;

            // assert
            Assert.AreEqual(productList.Count, model.Count);
        }
    }
}