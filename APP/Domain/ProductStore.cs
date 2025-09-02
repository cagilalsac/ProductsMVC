using CORE.APP.Domain;

namespace APP.Domain
{
    // for products-stores many to many relationship
    public class ProductStore : Entity
    {
        public int ProductId { get; set; } // foreign key that references to the Products table's Id primary key

        public Product Product { get; set; } // navigation property for retrieving related Product entity data
                                             // of the ProductStore entity data in queries

        public int StoreId { get; set; } // foreign key that references to the Stores table's Id primary key

        public Store Store { get; set; } // navigation property for retrieving related Store entity data
                                         // of the ProductStore entity data in queries
    }
}
