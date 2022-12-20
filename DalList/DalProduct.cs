using DO;
using DalApi;
using System.Security.Cryptography;

namespace Dal;

internal class DalProduct:IProduct
{
    public int Add(Product m_product)
    {
        if(m_product.ID == 0)
        {
            int temp = DataSource.Confing.get_ID_Product;
            m_product.ID = temp;
        }

        
        if (DataSource.productList.Exists(product => product.ID == m_product.ID))
        {
            throw new DataAlreadyExistException();
        }
        DataSource.productList.Add(m_product);
        return m_product.ID;
    }
    public Product GetById(int id)
    {
        for (int i = 0; i < DataSource.productList.Count; i++)
        {
            if (DataSource.productList[i].ID == id)
                return DataSource.productList[i];
        }
        throw new DataNotExistException();
    }

    /// <summary>
    /// Get product list
    /// </summary>
    /// <returns>List of all products</returns>
    public List<Product> GetList()
    {
        List<Product> arr = new List<Product>();
        //Array.Copy(DataSource.productList, arr, DataSource.Confing.indexProduct);
        arr = new List<Product>(DataSource.productList);
        return arr;
    }

    public void Delete(int id)
    {
        for (int i = 0; i < DataSource.productList.Count; i++)
        {
            if (DataSource.productList[i].ID == id)
            {
                DataSource.productList.Remove(DataSource.productList[i]);
                //for (int j = i; j < DataSource.productList.Count; j++)//נבצע דריסה של האובייקט ונקדם את האובייקטים במערך
                //{
                //    DataSource.productList[j] = DataSource.productList[j + 1];
                //}
                //DataSource.Confing.indexProduct--;
                return;
            }
        }
        throw new DataNotExistException();
    }
    public void Update(Product m_product)
    {
        for (int i = 0; i < DataSource.productList.Count; i++)
        {
            if (m_product.ID == DataSource.productList[i].ID)
            {
                DataSource.productList[i] = m_product;
                return;
            }
        }
        throw new DataNotExistException();
    }
}
