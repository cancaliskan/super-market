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
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResponseHelper<Product> _responseHelper;
        private readonly ResponseHelper<List<Product>> _listResponseHelper;
        private readonly ResponseHelper<bool> _booleanResponseHelper;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _responseHelper = new ResponseHelper<Product>();
            _listResponseHelper = new ResponseHelper<List<Product>>();
            _booleanResponseHelper = new ResponseHelper<bool>();
        }

        public Response<Product> GetById(Guid id)
        {
            try
            {
                if (id.IsEmptyGuid())
                {
                    return _responseHelper.FailResponse("Invalid product id");
                }

                var product = _unitOfWork.ProductRepository.GetById(id);
                if (product == null)
                {
                    return _responseHelper.FailResponse("Product could not found");
                }

                return _responseHelper.SuccessResponse(product, "Product returned successfully");
            }
            catch (Exception e)
            {
                return _responseHelper.FailResponse(e.ToString());
            }
        }

        public Response<List<Product>> GetAll()
        {
            try
            {
                var products = _unitOfWork.ProductRepository.GetAll();
                return _listResponseHelper.SuccessResponse(products.ToList(), "Products returned successfully");
            }
            catch (Exception e)
            {
                return _listResponseHelper.FailResponse(e.ToString());
            }
        }

        public Response<Product> Add(Product entity)
        {
            try
            {
                var result = ProductValidations(entity);
                if (result.IsSucceed)
                {
                    entity = _unitOfWork.ProductRepository.Add(entity);
                    _unitOfWork.Complete();

                    result.Result = entity;
                }

                return result;
            }
            catch (Exception e)
            {
                return _responseHelper.FailResponse(e.ToString());
            }
        }

        public Response<Product> Update(Product entity)
        {
            try
            {
                var existProduct = _unitOfWork.ProductRepository.GetById(entity.Id);
                if (existProduct == null)
                {
                    return _responseHelper.FailResponse("Product could not found");
                }

                var result = ProductValidations(entity);
                if (result.IsSucceed)
                {
                    existProduct.Name = entity.Name;
                    existProduct.Type = entity.Type;
                    existProduct.Stock = entity.Stock;
                    existProduct.UnitPrice = entity.UnitPrice;
                    existProduct.Description = entity.Description;
                    existProduct.Image = entity.Image;
                    result.Result = existProduct;

                    _unitOfWork.ProductRepository.Update(existProduct);
                    _unitOfWork.Complete();
                }

                return result;
            }
            catch (Exception e)
            {
                return _responseHelper.FailResponse(e.ToString());
            }
        }

        public Response<bool> Remove(Product entity)
        {
            try
            {
                var existProduct = _unitOfWork.ProductRepository.GetById(entity.Id);
                if (existProduct == null)
                {
                    return _booleanResponseHelper.FailResponse("Product could not found");
                }

                _unitOfWork.ProductRepository.Remove(entity);
                _unitOfWork.Complete();

                return _booleanResponseHelper.SuccessResponse("Product removed successfully");
            }
            catch (Exception e)
            {
                return _booleanResponseHelper.FailResponse(e.ToString());
            }
        }

        private Response<Product> ProductValidations(Product entity)
        {
            if (entity.Name.IsEmpty())
            {
                return _responseHelper.FailResponse("Product name is mandatory");
            }
            else if (entity.Stock < 1)
            {
                return _responseHelper.FailResponse("Stock must be greater than 0");
            }
            else if (entity.UnitPrice < 0)
            {
                return _responseHelper.FailResponse("UnitPrice must be greater than 0");
            }
            else if (entity.Image == null)
            {
                return _responseHelper.FailResponse("Image is mandatory");
            }
            else if (entity.Type.IsEmpty())
            {
                return _responseHelper.FailResponse("Type is mandatory");
            }
            else if (entity.Description.IsEmpty())
            {
                return _responseHelper.FailResponse("Description is mandatory");
            }

            return _responseHelper.SuccessResponse("");
        }
    }
}