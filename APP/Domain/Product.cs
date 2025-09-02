using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Domain
{
    public class Product : Entity
    {
        [Required, StringLength(150)]
        public string Name { get; set; } // Reference Type: null can't be assigned since Required is used

        // Way 1: decimal value type
        //public float UnitPrice { get; set; } // Value Type: null can't be assigned, default value is 0.0F if no assignment
        // Way 2: decimal value type
        //public double UnitPrice { get; set; } // Value Type: null can't be assigned, default value is 0.0 if no assignment
        // Way 3: decimal value type
        public decimal UnitPrice { get; set; } // Value Type: null can't be assigned, default value is 0.0M if no assignment

        public int? StockAmount { get; set; } // Value Type: null can be assigned since ? is used, default value is null if no assignment

        public DateTime? ExpirationDate { get; set; } // Value Type: null can be assigned since ? is used, default value is null if no assignment



        // for category-products one to many relationship
        public int CategoryId { get; set; } // Value Type: null can't be assigned, default value is 0 if no assignment

        public Category Category { get; set; } // navigation property for retrieving related Category entity data of the Product
                                               // entity data in queries,
                                               // Reference Type: null can be assigned, default reference is null if no assignment



        // for products-stores many to many relationship
        public List<ProductStore> ProductStores { get; set; } = new List<ProductStore>(); // navigation property for retrieving related ProductStore
                                                                                          // entities data of the Product entity data in queries,
                                                                                          // initialized for preventing null reference exception

        [NotMapped] // no column in the Products table will be created for this property since NotMapped attribute is defined
        public List<int> StoreIds // helps to easily manage the ProductStores relational entities by Store Id values
        {
            // returns the Store Id values of the Product entity
            get => ProductStores.Select(productStoreEntity => productStoreEntity.StoreId).ToList();

            // sets the ProductStores relational entities of the Product entity by the assigned Store Id values
            set => ProductStores = value.Select(storeId => new ProductStore() { StoreId = storeId }).ToList();
        }
    }
}
