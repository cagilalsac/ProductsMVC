using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Domain
{
    public class Store : Entity
    {
        [Required, StringLength(200)]
        public string Name { get; set; }

        public bool IsVirtual { get; set; }



        // for products-stores many to many relationship
        public List<ProductStore> ProductStores { get; set; } = new List<ProductStore>(); // navigation property for retrieving related ProductStore
                                                                                          // entities data of the Store entity data in queries,
                                                                                          // initialized for preventing null reference exception

        // Since we won't update the relational product data (ProductStores) through Store entity, we don't need the ProductIds property here.
        //[NotMapped] // no column in the Stores table will be created for this property since NotMapped attribute is defined
        //public List<int> ProductIds // helps to easily manage the ProductStores relational entities by Product Id values
        //{
        //    // returns the Product Id values of the Store entity
        //    get => ProductStores.Select(productStoreEntity => productStoreEntity.ProductId).ToList();

        //    // sets the ProductStores relational entities of the Store entity by the assigned Product Id values
        //    set => ProductStores = value.Select(productId => new ProductStore() { ProductId = productId }).ToList();
        //}
    }
}
