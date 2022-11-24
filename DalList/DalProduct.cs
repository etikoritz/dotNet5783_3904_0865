//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using DO;

namespace Dal;

public class DalProduct
{
    public  int AddProduct(Product m_product)
    {
        int temp = DataSource.Confing.get_ID_Product;
        m_product.ID = temp;
        //for (int i = 0; i < DataSource.Confing.I_Product; i++)//logical fonctions not allowed, where to put it?
        //{
        //    if (DataSource.productArray[i].ID == temp)
        //    {
        //        temp = DataSource.Confing.get_ID_Product;
        //        i = 0;
        //    }
        //}
        DataSource.productList.Add(m_product);
        //DataSource.Confing.indexProduct++;
        return m_product.ID;
    }
    public  Product GetProduct(int id)
    {
        for (int i = 0; i < DataSource.productList.Count; i++)
        {
            if (DataSource.productList[i].ID == id)
                return DataSource.productList[i];
        }
        throw new Exception("the product doesn't exist in the array");
    }
    public  List<Product> getList()
    {
        List<Product> arr = new List<Product>();
        //Array.Copy(DataSource.productList, arr, DataSource.Confing.indexProduct);
        arr = new List<Product>(DataSource.productList);
        return arr;
    }
    public  void deleteProduct(int id)
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
        throw new Exception("the product doesn't exist in the array");
    }
    public  void updateProduct(Product m_product)
    {
        for (int i = 0; i < DataSource.productList.Count; i++)
        {
            if (m_product.ID == DataSource.productList[i].ID)
            {
                DataSource.productList[i] = m_product;
                return;
            }
        }
        throw new Exception("the product doesn't exist in the array");
    }
}
