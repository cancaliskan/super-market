using System;
using System.Collections.Generic;
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
    public class SalesInformationServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private SalesInformationService _salesInformationService;
        private AssertHelper<bool> _assertHelperBoolean;
        private AssertHelper<SalesInformation> _assertHelper;
        private AssertHelper<List<SalesInformation>> _assertHelperList;

        [SetUp]
        public void Setup()
        {
            _assertHelperBoolean = new AssertHelper<bool>();
            _assertHelper = new AssertHelper<SalesInformation>();
            _assertHelperList = new AssertHelper<List<SalesInformation>>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _salesInformationService = new SalesInformationService(_unitOfWork.Object);
        }

        [Test]
        public void Orders_ValidRequest_Success()
        {
            // arrange
            var response = new Response<List<SalesInformation>>()
            {
                IsSucceed = true,
                SuccessMessage = "Orders return successfully"
            };
            _unitOfWork.Setup(x => x.SalesInformationRepository.GetSalesInformationByUser(It.IsAny<User>()))
                .Returns(new List<SalesInformation>());

            // act
            var result = _salesInformationService.Orders(new User());
            ;
            // assert
            _assertHelperList.Assertion(response, result);
        }

        [Test]
        public void Detail_ValidRequest_Success()
        {
            // arrange
            var response = new Response<SalesInformation>()
            {
                IsSucceed = true,
                SuccessMessage = "Orders return successfully"
            };
            _unitOfWork.Setup(x => x.SalesInformationRepository.GetSalesInformation(It.IsAny<Guid>()))
                .Returns(new SalesInformation());

            // act
            var result = _salesInformationService.Detail(Guid.NewGuid());
            ;
            // assert
            _assertHelper.Assertion(response, result);

        }
    }
}