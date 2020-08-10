using System;
using Supermarket.DataAccess.Contracts;

namespace Supermarket.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBasketRepository BasketRepository { get; }
        IProductBasketRepository ProductBasketRepository { get; }
        IProductRepository ProductRepository { get; }
        ISalesInformationRepository SalesInformationRepository { get; }
        IUserRepository UserRepository { get; }

        int Complete();
    }
}