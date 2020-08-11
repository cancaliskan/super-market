﻿using System;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResponseHelper<User> _responseHelper;

        private readonly byte[] _key;
        private readonly byte[] _iv;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _responseHelper = new ResponseHelper<User>();
        }

        public Response<User> GetById(Guid id)
        {
            try
            {
                if (id.IsEmptyGuid())
                {
                    return _responseHelper.FailResponse("Invalid user id");
                }

                var user = _unitOfWork.UserRepository.GetById(id);
                if (user == null)
                {
                    return _responseHelper.FailResponse("User could not found");
                }

                return _responseHelper.SuccessResponse(user, "Returned user successfully");

            }
            catch (Exception e)
            {
                return _responseHelper.FailResponse(e.ToString());
            }
        }

        public Response<User> Login(string email, string password)
        {
            try
            {
                if (email.IsNotEmail())
                {
                    return _responseHelper.FailResponse("Invalid email address");
                }
                else if (password.IsNotValidPassword())
                {
                    return _responseHelper.FailResponse("Invalid password");
                }

                var user = _unitOfWork.UserRepository.GetByEmail(email);
                if (user == null)
                {
                    return _responseHelper.FailResponse("User could not found");
                }

                if (password != CryptoHelper.Decrypt(user.Password))
                {
                    return _responseHelper.FailResponse("Invalid password");
                }

                return _responseHelper.SuccessResponse(user, "Returned user successfully");

            }
            catch (Exception e)
            {
                return _responseHelper.FailResponse(e.ToString());
            }
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

                entity.Password = CryptoHelper.Encrypt(entity.Password);

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

        public Response<bool> Remove(Guid id)
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