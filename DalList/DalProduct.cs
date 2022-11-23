//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using DO;

namespace Dal;

public class DalProduct
{
    public  int Add(Product m_product)
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
        DataSource.productArray[DataSource.Confing.indexProduct] = m_product;
        DataSource.Confing.indexProduct++;
        return m_product.ID;
    }
    public  Product GetProduct(int id)
    {
        for (int i = 0; i < DataSource.Confing.indexProduct; i++)
        {
            if (DataSource.productArray[i].ID == id)
                return DataSource.productArray[i];
        }
        throw new Exception("the product doesn't exist in the array");
    }
    public  Product[] getList()
    {
        Product[] arr = new Product[DataSource.Confing.indexProduct];
        Array.Copy(DataSource.productArray, arr, DataSource.Confing.indexProduct);
        return arr;
    }
    public  void deleteProduct(int id)
    {
        for (int i = 0; i < DataSource.Confing.indexProduct; i++)
        {
            if (DataSource.productArray[i].ID == id)
            {
                //DataSource.productArray.remove(i);
                for (int j = i; j < DataSource.Confing.indexProduct; j++)//נבצע דריסה של האובייקט ונקדם את האובייקטים במערך
                {
                    DataSource.productArray[j] = DataSource.productArray[j + 1];
                }
                DataSource.Confing.indexProduct--;
                return;
            }
        }
        throw new Exception("the product doesn't exist in the array");
    }
    public  void updateProduct(Product m_product)
    {
        for (int i = 0; i < DataSource.productArray.Length; i++)
        {
            if (m_product.ID == DataSource.productArray[i].ID)
            {
                DataSource.productArray[i] = m_product;
                return;
            }
        }
        throw new Exception("the product doesn't exist in the array");
    }
}
