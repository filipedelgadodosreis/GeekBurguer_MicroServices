using System;
using System.Collections.Generic;

namespace Ordering.Mongo.Dtos
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }

        public Guid StoreId { get; set; }

        public string Total { get; set; }

        public IEnumerable<ProductDto> Products { get; set; }

        public IEnumerable<Guid> ProductionIds { get; set; }

    }

    public class ProductDto
    {
        public Guid StoreId { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public IEnumerable<ItemDto> Items { get; set; }
    }    public class ItemDto
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
    }
}
