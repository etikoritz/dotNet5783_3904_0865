using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using Dal;
using DalApi;

namespace BlApi;

public interface IProduct
{
    /// <summary>
    /// Get Product list from the Dal
    /// --Manager and Buyer screen--
    /// </summary>
    /// <returns>list of products (BO)</returns>
    public IEnumerable<ProductForList> GetProductList();

    /// <summary>
    /// Get product's details by its ID
    /// --Manager screen--
    /// </summary>
    /// <param name="ID"></param>
    /// <returns>Product details</returns>
    public Product GetProductDetails(int ID);

    /// <summary>
    /// Get product's details by its ID and by the Cart of the current purchase
    /// --Buyer screen--
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="cart"></param>
    /// <returns>ProductItem</returns>
    public ProductItem GetProductDetails(int ID, Cart cart);

    /// <summary>
    /// Add new product to the Dal
    /// --Manager screen--
    /// </summary>
    /// <param name="product"></param>
    public void Add(Product product);

    /// <summary>
    /// Update pruduct details
    /// --Manager screen--
    /// </summary>
    /// <param name="product"></param>
    public void Update(Product product);

    /// <summary>
    /// Delete product
    /// --Manager screen--
    /// </summary>
    /// <param name="product"></param>
    public void Delete(Product product);
}
