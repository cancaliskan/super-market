using Supermarket.DataAccess.Context;
using Supermarket.DataAccess.Contracts;

namespace Supermarket.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UnitOfWork(ApplicationDbContext applicationDbContext, IBasketRepository basketRepository, IProductBasketRepository productBasketRepository, IProductRepository productRepository, ISalesInformationRepository salesInformationRepository, IUserRepository userRepository)
        {
            _applicationDbContext = applicationDbContext;
            BasketRepository = basketRepository;
            ProductBasketRepository = productBasketRepository;
            ProductRepository = productRepository;
            SalesInformationRepository = salesInformationRepository;
            UserRepository = userRepository;
        }

        public IBasketRepository BasketRepository { get; }
        public IProductBasketRepository ProductBasketRepository { get; }
        public IProductRepository ProductRepository { get; }
        public ISalesInformationRepository SalesInformationRepository { get; }
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