using System;
using System.Collections.Generic;

using Moq;
using NUnit.Framework;

using Supermarket.Business.Services;
using Supermarket.Business.Tests.Helpers;
using Supermarket.Common.Contracts;
using Supermarket.DataAccess.UnitOfWork;
using Supermarket.Domain.Entities;

namespace Supermarket.Business.Tests
{
    [TestFixture]
    public class BasketServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private BasketService _basketService;
        private AssertHelper<Basket> _assertHelper;
        private AssertHelper<bool> _assertHelperBoolean;

        [SetUp]
        public void Setup()
        {
            _assertHelper = new AssertHelper<Basket>();
            _assertHelperBoolean = new AssertHelper<bool>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _basketService = new BasketService(_unitOfWork.Object);
        }

        [Test]
        public void GetDetail_Success()
        {
            // arrange
            var response = new Response<Basket>()
            {
                SuccessMessage = "Products return successfully",
                IsSucceed = true
            };
            _unitOfWork.Setup(x => x.BasketRepository.GetBasketByUser(It.IsAny<User>())).Returns(new Basket());
            _unitOfWork.Setup(x => x.ProductBasketRepository.GetProductsByBasket(It.IsAny<Basket>())).Returns(new List<Product>());

            // act
            var result = _basketService.GetDetail(new User());

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void Remove_Failed()
        {
            // arrange
            var response = new Response<bool>()
            {
                ErrorMessage = "Something went wrong",
                IsSucceed = false
            };
            _unitOfWork.Setup(x => x.BasketRepository.GetBasketByUser(It.IsAny<User>())).Returns(new Basket());
            _unitOfWork.Setup(x => x.ProductBasketRepository.Find(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns((ProductBasket)null);

            // act
            var result = _basketService.Remove(new User(), Guid.NewGuid());

            // assert
            _assertHelperBoolean.Assertion(response, result);
        }

        [Test]
        public void Remove_Success()
        {
            // arrange
            var response = new Response<bool>()
            {
                ErrorMessage = "Product successfully removed from basket",
                IsSucceed = true
            };
            _unitOfWork.Setup(x => x.BasketRepository.GetBasketByUser(It.IsAny<User>())).Returns(new Basket());
            _unitOfWork.Setup(x => x.ProductBasketRepository.Find(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new ProductBasket());
            _unitOfWork.Setup(x => x.ProductRepository.GetById(It.IsAny<Guid>())).Returns(new Product());

            // act
            var result = _basketService.Remove(new User(), Guid.NewGuid());

            // assert
            _assertHelperBoolean.Assertion(response, result);
        }

        [Test]
        public void CompleteOrder_Failed()
        {
            // arrange
            var response = new Response<bool>()
            {
                ErrorMessage = "Basket could not found",
                IsSucceed = false
            };
            _unitOfWork.Setup(x => x.BasketRepository.GetById(It.IsAny<Guid>())).Returns((Basket)null);

            // act
            var result = _basketService.CompleteOrder(Guid.NewGuid());

            // assert
            _assertHelperBoolean.Assertion(response, result);
        }

        [Test]
        public void CompleteOrder_Success()
        {
            // arrange
            var response = new Response<bool>()
            {
                SuccessMessage = "Order completed successfully",
                IsSucceed = true
            };
            var basket =new Basket()
            {
                IsActive = true,
                CreatedDate = DateTime.Now,
                UserId = Guid.NewGuid()
            };

            _unitOfWork.Setup(x => x.BasketRepository.GetById(It.IsAny<Guid>())).Returns(basket);
            _unitOfWork.Setup(x => x.ProductBasketRepository.GetProductsByBasket(It.IsAny<Basket>())).Returns(new List<Product>());
            _unitOfWork.Setup(x => x.SalesInformationRepository.Add(It.IsAny<SalesInformation>())).Returns(new SalesInformation());
            _unitOfWork.Setup(x => x.OrderProductInformationRepository.AddRange(It.IsAny<List<OrderProductInformation>>())).Returns(new List<OrderProductInformation>());

            // act
            var result = _basketService.CompleteOrder(Guid.NewGuid());

            // assert
            _assertHelperBoolean.Assertion(response, result);
        }
    }
}