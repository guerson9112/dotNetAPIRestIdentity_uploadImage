using System;
using ApiEcommerce.Models;

namespace ApiEcommerce.Repository.IRepository;

public interface IProductRepository
{
    ICollection<Product> GetProducts();
    ICollection<Product> GetProductsForCategory(int categoryId);
    ICollection<Product> SearchProducts(string searchTerm);

    Product? GetProduct(int id);
    bool ProductExists(int id);
    bool ProductExists(string name);
    bool BuyProduct(string name, int quantity);
    bool Createproduct(Product product);
    bool Updateproduct(Product product);
    bool Deleteproduct(Product product);
    bool Save();

}
