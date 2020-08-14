using Supermarket.DataAccess.Context;
using Supermarket.DataAccess.Contracts;
using Supermarket.DataAccess.Repositories;

namespace Supermarket.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            BasketRepository = new BasketRepository(_applicationDbContext);
            ProductBasketRepository = new ProductBasketRepository(_applicationDbContext);
            ProductRepository = new ProductRepository(_applicationDbContext);
            SalesInformationRepository = new SalesInformationRepository(_applicationDbContext);
            OrderProductInformationRepository = new OrderProductInformationRepository(_applicationDbContext);
            UserRepository = new UserRepository(_applicationDbContext);
        }

        public IBasketRepository BasketRepository { get; }
        public IProductBasketRepository ProductBasketRepository { get; }
        public IProductRepository ProductRepository { get; }
        public ISalesInformationRepository SalesInformationRepository { get; }
        public IOrderProductInformationRepository OrderProductInformationRepository { get; }
        public IUserRepository UserRepository { get; }

        public int Complete()
        {
            return _applicationDbContext.SaveChanges();
        }

        public void Dispose()
        {
            _applicationDbContext?.Dispose();
        }
    }
}