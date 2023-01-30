using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

internal class ProductXml : IProduct
{
    public int Add(Product item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Product?> DoGetProductListBySort(Func<Product?, bool>? condition)
    {
        throw new NotImplementedException();
    }

    public Product? Get(Func<Product?, bool>? condition)
    {
        throw new NotImplementedException();
    }

    public Product? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Product?> GetList(Func<Product?, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Product item)
    {
        throw new NotImplementedException();
    }
}
