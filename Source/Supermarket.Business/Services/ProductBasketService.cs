﻿using System;

using Supermarket.Business.Contracts;
using Supermarket.Common.Contracts;
using Supermarket.Common.Helpers;
using Supermarket.DataAccess.UnitOfWork;
using Supermarket.Domain.Entities;

namespace Supermarket.Business.Services
{
    public class ProductBasketService : IProductBasketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResponseHelper<bool> _booleanResponseHelper;

        public ProductBasketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _booleanResponseHelper = new ResponseHelper<bool>();
        }

        public Response<bool> AddToBasket(User user, Guid productId)
        {
            try
            {
                var product = _unitOfWork.ProductRepository.GetById(productId);
                if (product == null)
                {
                    return _booleanResponseHelper.FailResponse("Product could not found");
                }

                if (product.Stock < 1)
                {
                    return _booleanResponseHelper.FailResponse("Product is not in stock");
                }

                var basket = _unitOfWork.BasketRepository.GetBasketByUser(user);
                var entity = new ProductBasket()
                {
                    BasketId = basket.Id,
                    Basket = basket,
                    ProductId = productId,
                    Product = product
                };

                _unitOfWork.ProductBasketRepository.Add(entity);

                product.Stock--;
                _unitOfWork.ProductRepository.Update(product);
                _unitOfWork.Complete();
                return _booleanResponseHelper.SuccessResponse("Product added to basket successfully");
            }
            catch (Exception e)
            {
                return _booleanResponseHelper.FailResponse(e.ToString());
            }
        }
    }
}