using System;

namespace Supermarket.Domain.Entities
{
    public sealed class SalesInformation : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User{ get; set; }
    }
}