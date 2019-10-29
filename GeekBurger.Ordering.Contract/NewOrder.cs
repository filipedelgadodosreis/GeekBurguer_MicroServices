using System;
using System.Collections.Generic;

namespace GeekBurger.Ordering.Contract
{
    public class Order
    {
        public Guid OrderId { get; private set; }

        public Guid StoreId { get; private set; }

        public string Total { get; private set; }

        public IEnumerable<Product> Products { get; private set; }

        public IEnumerable<Guid> ProductionIds { get; private set; }

    }

    public class Product
    {
        public Guid StoreId { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }    public class Item
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
    }
}
