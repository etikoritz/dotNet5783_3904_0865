using DO;
using DalApi;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.XPath;
//using DalXml;
namespace Dal;

internal class DalProduct: IProduct
{
    //const string productPath = "Product";
    //static XElement config = DalXml(XmlTools.LoadConfig());
    public int Add(Product m_product)
    {
        //List<DO.Product?> listProduct = XmlTools.LoadListFromXMLSerializer<DO.Product>(productPath);
        if (m_product.ID == 0)
        {
            int temp = DataSource.Confing.get_ID_Product;
            m_product.ID = temp;
        }
        if (DataSource.productList.Exists(product => product?.ID == m_product.ID))
        {
            throw new DataAlreadyExistException();
        }
        DataSource.productList.Add(m_product);
        return m_product.ID;
    }

    public Product? GetById(int id)
    { 
        DO.Product? product = DataSource.productList.FirstOrDefault(p=> p.Value.ID == id);
        if (product == null)
            throw new DataNotExistException();
        return product ?? new();
    }

    /// <summary>
    /// Get product list
    /// </summary>
    /// <returns>List of all products</returns>
    public IEnumerable<DO.Product?> GetList(Func<DO.Product?, bool>? filter = null)
    {
        return DataSource.productList.ConvertAll(product => product);
    }

    public void Delete(int id)
    {
        for (int i = 0; i < DataSource.productList.Count; i++)
        {
            //  if (DataSource.productList[i]?.ID == id)
            if (DataSource.productList[i]?.ID == id)
            {
                DataSource.productList.Remove(DataSource.productList[i]);
                return;
            }
        }
        throw new DataNotExistException();
    }
    public void Update(Product m_product)
    {
        for (int i = 0; i < DataSource.productList.Count; i++)
        {
            if (m_product.ID == DataSource.productList[i]?.ID)
            {
                DataSource.productList[i] = m_product;
                return;
            }
        }
        throw new DataNotExistException();
    }

    public IEnumerable<DO.Product?> DoGetProductListBySort(Func<Product?, bool> condition)
    {
        return DataSource.productList.FindAll(x => condition(x));
    }



    public Product? Get(Func<Product?, bool>? condition)
    {

        DO.Product? product = DataSource.productList.FirstOrDefault(p => condition(p));
        if (product == null)
            throw new DataNotExistException();
        return product ?? new();
    }
}
