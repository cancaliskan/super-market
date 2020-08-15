using NUnit.Framework;

using Supermarket.Common.Contracts;

namespace Supermarket.Business.Tests.Helpers
{
    public class AssertHelper<T>
    {
        public void Assertion(Response<T> response, Response<T> result)
        {
            Assert.AreEqual(response.IsSucceed, result.IsSucceed);
        }
    }
}