using System;
using Moq;
using NUnit.Framework;
using Supermarket.Business.Services;
using Supermarket.Business.Tests.Helpers;
using Supermarket.Common.Contracts;
using Supermarket.DataAccess.UnitOfWork;
using Supermarket.Domain.Entities;

namespace Supermarket.Business.Tests.Services
{
    [TestFixture]
    public class ProductBasketServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private ProductBasketService _productBasketService;
        private AssertHelper<bool> _assertHelperBoolean;

        [SetUp]
        public void Setup()
        {
            _assertHelperBoolean = new AssertHelper<bool>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _productBasketService = new ProductBasketService(_unitOfWork.Object);
        }

        [Test]
        public void AddToBasket_ProductRepository_Failed()
        {
            // arrange
            var response = new Response<bool>()
            {
                ErrorMessage = "Product could not found",
                IsSucceed = false
            };
            _unitOfWork.Setup(x => x.ProductRepository.GetById(It.IsAny<Guid>())).Returns((Product)null);

            // act
            var result = _productBasketService.AddToBasket(new User(), Guid.NewGuid());

            // assert
            _assertHelperBoolean.Assertion(response, result);
        }

        [Test]
        public void AddToBasket_ProductStock_Failed()
        {
            // arrange
            var response = new Response<bool>()
            {
                ErrorMessage = "Product is not in stock",
                IsSucceed = false
            };

            var product = new Product()
            {
                Stock = 0
            };

            _unitOfWork.Setup(x => x.ProductRepository.GetById(It.IsAny<Guid>())).Returns(product);

            // act
            var result = _productBasketService.AddToBasket(new User(), Guid.NewGuid());

            // assert
            _assertHelperBoolean.Assertion(response, result);
        }


        [Test]
        public void AddToBasket_ProductRepository_Success()
        {
            // arrange
            var response = new Response<bool>()
            {
                SuccessMessage = "Product added to basket successfully",
                IsSucceed = false
            };

            var product = new Product()
            {
                Stock = 3
            };

            _unitOfWork.Setup(x => x.ProductRepository.GetById(It.IsAny<Guid>())).Returns(product);

            // act
            var result = _productBasketService.AddToBasket(new User(), Guid.NewGuid());

            // assert
            _assertHelperBoolean.Assertion(response, result);
        }
    }
}