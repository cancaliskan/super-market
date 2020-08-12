using Supermarket.Domain.Entities;

namespace Supermarket.DataAccess.Contracts
{
    public interface IBasketRepository : IRepository<Basket>
    {
        Basket GetBasketByUser(User user);
    }
}