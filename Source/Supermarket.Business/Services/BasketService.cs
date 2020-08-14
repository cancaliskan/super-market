using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly ResponseHelper<Basket> _listResponseHelper;
        private readonly ResponseHelper<bool> _booleanResponseHelper;

        public BasketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _responseHelper = new ResponseHelper<Basket>();
            _listResponseHelper = new ResponseHelper<Basket>();
            _booleanResponseHelper = new ResponseHelper<bool>();
        }

        public Response<Basket> GetDetail(User user)
        {
            try
            {
                var basket = _unitOfWork.BasketRepository.GetBasketByUser(user);
                var products = _unitOfWork.ProductBasketRepository.GetProductsByBasket(basket);
                basket.Products = products;
                return _listResponseHelper.SuccessResponse(basket, "Products return successfully");
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

        public Response<bool> CompleteOrder(Guid basketId)
        {
            try
            {
                var basket = _unitOfWork.BasketRepository.GetById(basketId);
                if (basket == null)
                {
                    return _booleanResponseHelper.FailResponse("Basket could not found");
                }
                basket.Products = _unitOfWork.ProductBasketRepository.GetProductsByBasket(basket);

                var salesInformation = new SalesInformation()
                {
                    TotalItem = basket.Products.Count,
                    TotalPrice = basket.Products.Sum(x => x.UnitPrice),

                    UserId = basket.UserId,
                    User = basket.User
                };

                salesInformation = _unitOfWork.SalesInformationRepository.Add(salesInformation);
                _unitOfWork.Complete();

                var orders = new List<OrderProductInformation>();
                Parallel.ForEach(basket.Products.Distinct(), product =>
                {
                    var orderProductInformation = new OrderProductInformation()
                    {
                        ProductId = product.Id,
                        Product = product,

                        SalesInformationId = salesInformation.Id,
                        SalesInformation = salesInformation,

                        Count = basket.Products.Count(x => x.Id == product.Id),
                        UnitPrice = product.UnitPrice
                    };
                    orderProductInformation.TotalPrice = orderProductInformation.Count * orderProductInformation.UnitPrice;

                    orders.Add(orderProductInformation);
                });

                salesInformation.Orders = orders;
                _unitOfWork.SalesInformationRepository.Update(salesInformation);
                var a =_unitOfWork.OrderProductInformationRepository.AddRange(orders);
                _unitOfWork.BasketRepository.CompleteOrder(basketId);
                _unitOfWork.Complete();

                return _booleanResponseHelper.SuccessResponse("Order completed successfully");
            }
            catch (Exception e)
            {
                return _booleanResponseHelper.FailResponse(e.ToString());
            }
        }
    }
}