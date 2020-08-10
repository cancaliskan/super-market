using System.Collections.Generic;

using Supermarket.Common.Contracts;

namespace Supermarket.Common.Helpers
{
    public class ResponseHelper<T>
    {
        public Response<T> FailResponse(string errorMessage)
        {
            var response = new Response<T>
            {
                IsSucceed = false,
                ErrorMessage = errorMessage
            };
            return response;
        }

        public Response<T> SuccessResponse(T result, string successMessage)
        {
            var response = new Response<T>
            {
                IsSucceed = true,
                SuccessMessage = successMessage,
                Result = result
            };

            return response;
        }
    }
}