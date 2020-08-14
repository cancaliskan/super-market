using System;
using System.Collections.Generic;
using Supermarket.Business.Contracts;
using Supermarket.Common.Contracts;
using Supermarket.Common.Helpers;
using Supermarket.DataAccess.UnitOfWork;
using Supermarket.Domain.Entities;

namespace Supermarket.Business.Services
{
    public class SalesInformationService : ISalesInformationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResponseHelper<SalesInformation> _responseHelper;
        private readonly ResponseHelper<List<SalesInformation>> _listResponseHelper;
        private readonly ResponseHelper<bool> _booleanResponseHelper;

        public SalesInformationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _responseHelper = new ResponseHelper<SalesInformation>();
            _listResponseHelper = new ResponseHelper<List<SalesInformation>>();
            _booleanResponseHelper = new ResponseHelper<bool>();
        }

        public Response<List<SalesInformation>> Orders(User user)
        {
            try
            {
                var salesInformationList = _unitOfWork.SalesInformationRepository.GetSalesInformationByUser(user);
                return _listResponseHelper.SuccessResponse(salesInformationList, "Orders return successfully");
            }
            catch (Exception e)
            {
                return _listResponseHelper.FailResponse(e.ToString());
            }
        }

        public Response<SalesInformation> Detail(Guid id)
        {
            try
            {
                var salesInformation = _unitOfWork.SalesInformationRepository.GetSalesInformation(id);
                return _responseHelper.SuccessResponse(salesInformation, "Sales information return successfully");
            }
            catch (Exception e)
            {
                return _responseHelper.FailResponse(e.ToString());
            }
        }
    }
}