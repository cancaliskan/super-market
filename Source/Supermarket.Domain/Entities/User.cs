﻿using System;
using System.Collections.Generic;

namespace Supermarket.Domain.Entities
{
    public sealed class User : BaseEntity
    {
        public User()
        {
            SalesInformation = new List<SalesInformation>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public Basket Basket { get; set; }

        public ICollection<SalesInformation> SalesInformation { get; set; }
    }
}