using System;
using System.Collections.Generic;
using System.Linq;
using Supermarket.Business.Contracts;
using Supermarket.Common.Contracts;
using Supermarket.Common.Helpers;
using Supermarket.DataAccess.UnitOfWork;
using Supermarket.Domain.Entities;

namespace Supermarket.Business.Services
{
    public class BasketService : IBasketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResponseHelper<Basket> _responseHelper;
        private readonly ResponseHelper<List<Product>> _listResponseHelper;
        private readonly ResponseHelper<bool> _booleanResponseHelper;

        public BasketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _responseHelper = new ResponseHelper<Basket>();
            _listResponseHelper = new ResponseHelper<List<Product>>();
            _booleanResponseHelper = new ResponseHelper<bool>();
        }

        public Response<List<Product>> GetDetail(User user)
        {
            try
            {
                var basket = _unitOfWork.BasketRepository.GetBasketByUser(user);
                var products = _unitOfWork.ProductBasketRepository.GetProductsByBasket(basket);
                return _listResponseHelper.SuccessResponse(products, "Products return successfully");
            }
            catch (Exception e)
            {
                return _listResponseHelper.FailResponse(e.ToString());
            }
        }

        public Response<bool> Remove(User user, Guid productId)
        {
            try
            {
                var basket = _unitOfWork.BasketRepository.GetBasketByUser(user);
                var productBasket = _unitOfWork.ProductBasketRepository.Find(basket.Id, productId);
                if (productBasket == null)
                {
                    return _booleanResponseHelper.FailResponse("Something went wrong");
                }

                var product = _unitOfWork.ProductRepository.GetById(productId);
                product.Stock++;

                _unitOfWork.ProductRepository.Update(product);
                _unitOfWork.ProductBasketRepository.Remove(productBasket);
                _unitOfWork.Complete();

                return _booleanResponseHelper.SuccessResponse("Product successfully removed from basket");
            }
            catch (Exception e)
            {
                return _booleanResponseHelper.FailResponse(e.ToString());
            }
        }
    }
}