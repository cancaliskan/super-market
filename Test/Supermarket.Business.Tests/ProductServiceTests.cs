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
    public class ProductServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private ProductService _productService;
        private AssertHelper<bool> _assertHelperBoolean;
        private AssertHelper<Product> _assertHelper;
        private AssertHelper<List<Product>> _assertHelperList;

        [SetUp]
        public void Setup()
        {
            _assertHelperBoolean = new AssertHelper<bool>();
            _assertHelper = new AssertHelper<Product>();
            _assertHelperList = new AssertHelper<List<Product>>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _productService = new ProductService(_unitOfWork.Object);
        }

        [Test]
        public void GetById_InvalidId_Failed()
        {
            // arrange
            var response = new Response<Product>()
            {
                IsSucceed = false,
                ErrorMessage = "Invalid product id"
            };

            // act
            var result = _productService.GetById(Guid.Empty);

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void GetById_InvalidProduct_Failed()
        {
            // arrange
            var response = new Response<Product>()
            {
                IsSucceed = false,
                ErrorMessage = "Product could not found"
            };
            _unitOfWork.Setup(x => x.ProductRepository.GetById(It.IsAny<Guid>())).Returns((Product)null);

            // act
            var result = _productService.GetById(Guid.Empty);

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void GetById_ValidRequest_Success()
        {
            // arrange
            var response = new Response<Product>()
            {
                IsSucceed = true,
                ErrorMessage = "Product returned successfully"
            };
            _unitOfWork.Setup(x => x.ProductRepository.GetById(It.IsAny<Guid>())).Returns(new Product());

            // act
            var result = _productService.GetById(Guid.NewGuid());

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void GetAll_ValidRequest_Success()
        {
            // arrange
            var response = new Response<List<Product>>()
            {
                IsSucceed = true,
                ErrorMessage = "Products returned successfully"
            };
            _unitOfWork.Setup(x => x.ProductRepository.GetAll()).Returns(new List<Product>());

            // act
            var result = _productService.GetAll();

            // assert
            _assertHelperList.Assertion(response, result);
        }

        [Test]
        public void GetRecentlyAddedProducts_ValidRequest_Success()
        {
            // arrange
            var response = new Response<List<Product>>()
            {
                IsSucceed = true,
                ErrorMessage = "Products returned successfully"
            };
            _unitOfWork.Setup(x => x.ProductRepository.GetRecentlyAddedProducts()).Returns(new List<Product>());

            // act
            var result = _productService.GetRecentlyAddedProducts();

            // assert
            _assertHelperList.Assertion(response, result);
        }

        [Test]
        public void Add_InvalidName_Failed()
        {
            // arrange
            var product = new Product();
            var response = new Response<Product>()
            {
                ErrorMessage = "Product name is mandatory",
                IsSucceed = false
            };

            // act
            var result = _productService.Add(product);

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void Add_InvalidStock_Failed()
        {
            // arrange
            var product = new Product()
            {
                Name = "Test"
            };
            var response = new Response<Product>()
            {
                ErrorMessage = "Stock must be greater than 0",
                IsSucceed = false
            };

            // act
            var result = _productService.Add(product);

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void Add_InvalidUnitePrice_Failed()
        {
            // arrange
            var product = new Product()
            {
                Name = "Test",
                Stock = 4
            };
            var response = new Response<Product>()
            {
                ErrorMessage = "UnitPrice must be greater than 0",
                IsSucceed = false
            };

            // act
            var result = _productService.Add(product);

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void Add_InvalidType_Failed()
        {
            // arrange
            var product = new Product()
            {
                Name = "Test",
                Stock = 4,
                UnitPrice = 44
            };
            var response = new Response<Product>()
            {
                ErrorMessage = "Type is mandatory",
                IsSucceed = false
            };

            // act
            var result = _productService.Add(product);

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void Add_InvalidDescription_Failed()
        {
            // arrange
            var product = new Product()
            {
                Name = "Test",
                Stock = 4,
                UnitPrice = 44,
                Type = "Type"
            };
            var response = new Response<Product>()
            {
                ErrorMessage = "Description is mandatory",
                IsSucceed = false
            };

            // act
            var result = _productService.Add(product);

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void Add_ValidRequest_Success()
        {
            // arrange
            var product = new Product()
            {
                Name = "Test",
                Stock = 4,
                UnitPrice = 44,
                Type = "Type",
                Description = "Test"
            };
            var response = new Response<Product>()
            {
                IsSucceed = true,
            };
            _unitOfWork.Setup(x => x.ProductRepository.Add(It.IsAny<Product>())).Returns(new Product());

            // act
            var result = _productService.Add(product);

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void Update_InvalidId_Failed()
        {
            // arrange
            var response = new Response<Product>()
            {
                IsSucceed = false,
                ErrorMessage = "Product could not found"
            };
            _unitOfWork.Setup(x => x.ProductRepository.GetById(It.IsAny<Guid>())).Returns((Product)null);

            // act
            var result = _productService.Update(new Product());

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void Update_InvalidName_Failed()
        {
            // arrange
            var product = new Product();
            var response = new Response<Product>()
            {
                IsSucceed = false,
                ErrorMessage = "Product name is mandatory"
            };
            _unitOfWork.Setup(x => x.ProductRepository.GetById(It.IsAny<Guid>())).Returns(new Product());

            // act
            var result = _productService.Update(product);

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void Update_InvalidStock_Failed()
        {
            // arrange
            var product = new Product()
            {
                Name = "Test"
            };
            var response = new Response<Product>()
            {
                IsSucceed = false,
                ErrorMessage = "Stock must be greater than 0"
            };
            _unitOfWork.Setup(x => x.ProductRepository.GetById(It.IsAny<Guid>())).Returns(new Product());

            // act
            var result = _productService.Update(product);

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void Update_InvalidUnitPrice_Failed()
        {
            // arrange
            var product = new Product()
            {
                Name = "Test",
                Stock = 4
            };
            var response = new Response<Product>()
            {
                IsSucceed = false,
                ErrorMessage = "UnitPrice must be greater than 0"
            };
            _unitOfWork.Setup(x => x.ProductRepository.GetById(It.IsAny<Guid>())).Returns(new Product());

            // act
            var result = _productService.Update(product);

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void Update_InvalidType_Failed()
        {
            // arrange
            var product = new Product()
            {
                Name = "Test",
                Stock = 4,
                UnitPrice = 44
            };
            var response = new Response<Product>()
            {
                IsSucceed = false,
                ErrorMessage = "Type is mandatory"
            };
            _unitOfWork.Setup(x => x.ProductRepository.GetById(It.IsAny<Guid>())).Returns(new Product());

            // act
            var result = _productService.Update(product);

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void Update_InvalidDescription_Failed()
        {
            // arrange
            var product = new Product()
            {
                Name = "Test",
                Stock = 4,
                UnitPrice = 44,
                Type = "Test"
            };
            var response = new Response<Product>()
            {
                IsSucceed = false,
                ErrorMessage = "Description is mandatory"
            };
            _unitOfWork.Setup(x => x.ProductRepository.GetById(It.IsAny<Guid>())).Returns(new Product());

            // act
            var result = _productService.Update(product);

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void Update_ValidRequest_Success()
        {
            // arrange
            var product = new Product()
            {
                Name = "Test",
                Stock = 4,
                UnitPrice = 44,
                Type = "Test",
                Description = "Test"
            };
            var response = new Response<Product>()
            {
                IsSucceed = true,
            };
            _unitOfWork.Setup(x => x.ProductRepository.GetById(It.IsAny<Guid>())).Returns(new Product());

            // act
            var result = _productService.Update(product);

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void Remove_InvalidId_Failed()
        {
            // arrange
            var response = new Response<bool>()
            {
                IsSucceed = false,
                ErrorMessage = "Product could not found"
            };
            _unitOfWork.Setup(x => x.ProductRepository.GetById(It.IsAny<Guid>())).Returns((Product)null);

            // act
            var result = _productService.Remove(new Product());

            // assert
            _assertHelperBoolean.Assertion(response, result);
        }

        [Test]
        public void Remove_ValidRequest_Success()
        {
            // arrange
            var response = new Response<bool>()
            {
                IsSucceed = true,
                ErrorMessage = "Product removed successfully"
            };
            _unitOfWork.Setup(x => x.ProductRepository.GetById(It.IsAny<Guid>())).Returns(new Product());

            // act
            var result = _productService.Remove(new Product());

            // assert
            _assertHelperBoolean.Assertion(response, result);
        }
    }
}