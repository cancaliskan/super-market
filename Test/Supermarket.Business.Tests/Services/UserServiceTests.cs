using System;

using Moq;
using NUnit.Framework;

using Supermarket.Business.Services;
using Supermarket.Business.Tests.Helpers;
using Supermarket.Common.Contracts;
using Supermarket.Common.Helpers;
using Supermarket.DataAccess.UnitOfWork;
using Supermarket.Domain.Entities;

namespace Supermarket.Business.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private UserService _userService;
        private AssertHelper<User> _assertHelper;

        [SetUp]
        public void Setup()
        {
            _assertHelper = new AssertHelper<User>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _userService = new UserService(_unitOfWork.Object);
        }

        [Test]
        public void GetById_InvalidId_Failed()
        {
            // arrange
            var response = new Response<User>()
            {
                SuccessMessage = "Invalid user id",
                IsSucceed = false
            };

            // act
            var result = _userService.GetById(Guid.Empty);

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void GetById_UserNotFound_Failed()
        {
            // arrange
            var response = new Response<User>()
            {
                SuccessMessage = "User could not found",
                IsSucceed = false
            };
            _unitOfWork.Setup(x => x.UserRepository.GetById(It.IsAny<Guid>())).Returns((User)null);

            // act
            var result = _userService.GetById(Guid.NewGuid());

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void GetById_ValidRequest_Success()
        {
            // arrange
            var response = new Response<User>()
            {
                SuccessMessage = "Returned user successfully",
                IsSucceed = true
            };
            _unitOfWork.Setup(x => x.UserRepository.GetById(It.IsAny<Guid>())).Returns(new User());

            // act
            var result = _userService.GetById(Guid.NewGuid());

            // assert
            _assertHelper.Assertion(response, result);
        }

        [TestCase("can")]
        [TestCase("cancaliskan")]
        [TestCase("cancaliskan@test")]
        [TestCase("cancaliskan@")]
        public void Login_InvalidEmail_Failed(string email)
        {
            // arrange
            var response = new Response<User>()
            {
                IsSucceed = false,
                ErrorMessage = "Invalid email address"
            };

            // act
            var result = _userService.Login(email, string.Empty);

            // assert
            _assertHelper.Assertion(response, result);
        }

        [TestCase("Test")]
        [TestCase("Test+-")]
        [TestCase("test")]
        [TestCase("test+-password")]
        public void Login_InvalidPassword_Failed(string password)
        {
            // arrange
            var response = new Response<User>()
            {
                IsSucceed = false,
                ErrorMessage = "Invalid password"
            };

            // act
            var result = _userService.Login("cancaliskan@windowslive.com", password);

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void Login_UserNotFound_Failed()
        {
            // arrange
            var response = new Response<User>()
            {
                IsSucceed = false,
                ErrorMessage = "User could not found"
            };

            _unitOfWork.Setup(x => x.UserRepository.GetByEmail(It.IsAny<string>())).Returns((User)null);

            // act
            var result = _userService.Login("cancaliskan@windowslive.com", "Test+-1234*");

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void Login_WrongPassword_Failed()
        {
            // arrange
            var response = new Response<User>()
            {
                IsSucceed = false,
                ErrorMessage = "Wrong password"
            };

            var user = new User()
            {
                Password = CryptoHelper.Encrypt("Test+-1234*")
            };

            _unitOfWork.Setup(x => x.UserRepository.GetByEmail(It.IsAny<string>())).Returns(user);

            // act
            var result = _userService.Login("cancaliskan@windowslive.com", "Test+-5464*");

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void Login_ValidRequest_Success()
        {
            // arrange
            var response = new Response<User>()
            {
                IsSucceed = true,
                ErrorMessage = "Returned user successfully"
            };

            var password = "Test+-1234*";
            var user = new User()
            {
                Password = CryptoHelper.Encrypt(password)
            };
            _unitOfWork.Setup(x => x.UserRepository.GetByEmail(It.IsAny<string>())).Returns(user);

            // act
            var result = _userService.Login("cancaliskan@windowslive.com", password);

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void Add_InvalidName_Failed()
        {
            // arrange
            var response = new Response<User>()
            {
                IsSucceed = false,
                ErrorMessage = "Name is mandatory"
            };

            var user = new User();

            // act
            var result = _userService.Add(user);

            // assert
            _assertHelper.Assertion(result, response);
        }

        [Test]
        public void Add_InvalidLastName_Failed()
        {
            // arrange
            var response = new Response<User>()
            {
                IsSucceed = false,
                ErrorMessage = "Last Name is mandatory"
            };

            var user = new User()
            {
                Name = "Can"
            };

            // act
            var result = _userService.Add(user);

            // assert
            _assertHelper.Assertion(result, response);
        }

        [Test]
        public void Add_InvalidEmail_Failed()
        {
            // arrange
            var response = new Response<User>()
            {
                IsSucceed = false,
                ErrorMessage = "Email must be valid"
            };

            var user = new User()
            {
                Name = "Can",
                LastName = "Çalışkan"
            };

            // act
            var result = _userService.Add(user);

            // assert
            _assertHelper.Assertion(result, response);
        }

        [Test]
        public void Add_InvalidAddress_Failed()
        {
            // arrange
            var response = new Response<User>()
            {
                IsSucceed = false,
                ErrorMessage = "Address is mandatory"
            };

            var user = new User()
            {
                Name = "Can",
                LastName = "Çalışkan",
                Email = "cancaliskan@windowslive.com"
            };

            // act
            var result = _userService.Add(user);

            // assert
            _assertHelper.Assertion(result, response);
        }

        [Test]
        public void Add_InvalidPassword_Failed()
        {
            // arrange
            var response = new Response<User>()
            {
                IsSucceed = false,
                ErrorMessage = "Password must have 1 big, 1 small, 1 number and be minimum 8 character"
            };

            var user = new User()
            {
                Name = "Can",
                LastName = "Çalışkan",
                Email = "cancaliskan@windowslive.com",
                Address = "Karşıyaka"
            };

            // act
            var result = _userService.Add(user);

            // assert
            _assertHelper.Assertion(result, response);
        }

        [Test]
        public void Add_ValidRequest_Success()
        {
            // arrange
            var response = new Response<User>()
            {
                IsSucceed = true,
                SuccessMessage = "User added successfully"
            };

            var user = new User()
            {
                Name = "Can",
                LastName = "Çalışkan",
                Email = "cancaliskan@windowslive.com",
                Address = "Karşıyaka",
                Password = "Test+-1234*"
            };

            // act
            var result = _userService.Add(user);

            // assert
            _assertHelper.Assertion(result, result);
        }

        [TestCase("can")]
        [TestCase("cancaliskan@")]
        [TestCase("can@caliskan")]
        [TestCase("")]
        public void GetUserByEmail_InvalidEmail_Failed(string email)
        {
            // arrange
            var response = new Response<User>()
            {
                ErrorMessage = "Invalid email address",
                IsSucceed = false
            };

            // act
            var result = _userService.GetUserByEmail(email);

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void GetUserByEmail_UserNotFound_Failed()
        {
            // arrange
            var response = new Response<User>()
            {
                ErrorMessage = "User could not found",
                IsSucceed = false
            };
            _unitOfWork.Setup(x => x.UserRepository.GetByEmail(It.IsAny<string>())).Returns((User)null);

            // act
            var result = _userService.GetUserByEmail("cancaliskan@windowslive.com");

            // assert
            _assertHelper.Assertion(response, result);
        }

        [Test]
        public void GetUserByEmail_ValidRequest_Success()
        {
            // arrange
            var response = new Response<User>()
            {
                ErrorMessage = "Returned user successfully",
                IsSucceed = true
            };
            _unitOfWork.Setup(x => x.UserRepository.GetByEmail(It.IsAny<string>())).Returns(new User());

            // act
            var result = _userService.GetUserByEmail("cancaliskan@windowslive.com");

            // assert
            _assertHelper.Assertion(response, result);
        }
    }
}