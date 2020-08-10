using System;
using System.Collections.Generic;

using Supermarket.Business.Contracts;
using Supermarket.Common.Contracts;
using Supermarket.Common.Helpers;
using Supermarket.DataAccess.UnitOfWork;
using Supermarket.Domain.Entities;

namespace Supermarket.Business.Services
{
    public class UserService : IUserService
    {
        private readonly ResponseHelper<User> _responseHelper;
        private readonly IUnitOfWork _unitOfWork;

        private readonly CryptoHelper _cryptoHelper;
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public UserService(ResponseHelper<User> responseHelper, IUnitOfWork unitOfWork, CryptoHelper cryptoHelper)
        {
            _responseHelper = responseHelper;
            _unitOfWork = unitOfWork;

            _cryptoHelper = cryptoHelper;
            _key = _cryptoHelper.GetKey();
            _iv = _cryptoHelper.GetIV();
        }

        public Response<User> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Response<IEnumerable<User>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Response<User> Add(User entity)
        {
            try
            {
                if (ModelValidation(entity, out var response)) return response;

                entity.Id = new Guid(GuidHelper.GetNewUid());
                entity.Password = _cryptoHelper.Encrypt(entity.Password, _key, _iv);
                entity.CreatedDate = DateTime.Now;
                entity.IsActive = true;

                _unitOfWork.UserRepository.Add(entity);
                _unitOfWork.Complete();

                return _responseHelper.SuccessResponse(entity, "User added successfully");
            }
            catch (Exception e)
            {
                return _responseHelper.FailResponse(e.ToString());
            }
        }

        public Response<User> Update(User entity)
        {
            throw new System.NotImplementedException();
        }

        public Response<bool> Remove(int id)
        {
            throw new System.NotImplementedException();
        }

        private bool ModelValidation(User entity, out Response<User> response)
        {
            if (entity.Name.IsEmpty())
            {
                response = _responseHelper.FailResponse("Name is mandatory");
                return true;
            }
            else if (entity.LastName.IsEmpty())
            {
                response = _responseHelper.FailResponse("Last Name is mandatory");
                return true;
            }
            else if (entity.Email.IsNotEmail())
            {
                response = _responseHelper.FailResponse("Email must be valid");
                return true;
            }
            else if (entity.Address.IsEmpty())
            {
                response = _responseHelper.FailResponse("Address is mandatory");
                return true;
            }

            response = null;
            return false;
        }
    }
}