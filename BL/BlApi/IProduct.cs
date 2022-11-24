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
    public IEnumerable<ProductForList> GetProductList();
    public Product GetProductDetails(int ID);
    public ProductItem GetProductDetails(int ID, Cart cart);
    public void Add(Product product);
    public void Update(Product product);
    public void Delete(Product product);
}
