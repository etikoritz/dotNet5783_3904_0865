using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using DalApi;

namespace BlImplementation;
internal class BoProduct :  BlApi.IProduct
{
    private DalApi.IDal Dal = new Dal.DalList();
    public static int count = 0;

    /// <summary>
    /// Get list of products from DO
    /// </summary>
    /// <returns>Returns the list as a BO ProductForList</returns>
    public IEnumerable<BO.ProductForList> GetProductList()
    {

        //get the products list from dal
        List<DO.Product?> DalProductList =(List<DO.Product?>)Dal.Product.GetList();
        


        //creates new BO products list
        List<BO.ProductForList> productsList = new();

        

        
        ///putting product from the DO list into the BO list
        foreach (DO.Product product in DalProductList)
        {
            //creates new ProductForList items from the dal products list
            BO.ProductForList p = new()
            {
                ID = product.ID,
                Name = product.Name,
                Category = product.Category,
                Price = product.Price,
                inStock = product.InStock
            };
            productsList.Add(p);
        }
        return productsList;
    }

    /// <summary>
    /// Get product details from DO by its ID
    /// </summary>
    /// <param name="ID"></param>
    /// <returns>BO product</returns>
    /// <exception cref="BO.BODataAlreadyExistException"></exception>
    /// <exception cref="BO.NegativeIdException"></exception>
    public BO.Product GetProductDetails(int ID)
    {
        if(ID > 0)
        {
            try
            {
                DO.Product p = (DO.Product)Dal.Product.GetById(ID);
                BO.Product product = new()
                {
                    ID = p.ID,
                    Name = p.Name,
                    Category = p.Category,
                    Price = p.Price,
                    InStock= p.InStock
                };
                return product;
            }
            catch (DO.DataNotExistException ex)
            {
                throw new BO.BODataNotExistException(ex.Message);
            }
        }
        else
            throw new BO.NegativeIdException();
    }

    /// <summary>
    /// Get product details from DO by its ID and cart from buyer
    /// </summary>
    /// <param name="ID"></param>
    /// <returns>BO product</returns>
    /// <exception cref="BO.BODataAlreadyExistException"></exception>
    /// <exception cref="BO.NegativeIdException"></exception>
    public BO.ProductItem GetProductDetails(int ID, BO.Cart cart)
    {
        if (ID > 0)
        {
            try
            {
                DO.Product p = (DO.Product)Dal.Product.GetById(ID);

                BO.ProductItem productItem = new()
                {
                    ID = p.ID,
                    Name = p.Name,
                    Category = p.Category,
                    Price = p.Price,
                    InStock = p.InStock,
                    Amount = cart.Items.Find(x => x.ProductID == ID).Amount
                };                
                return productItem;
            }
            catch (DO.DataNotExistException ex)
            {
                throw new BO.BODataNotExistException(ex.Message);
            }
        }
        else
            throw new BO.NegativeIdException();
    }
   
    /// <summary>
    /// Add new product (manager screen)
    /// </summary>
    /// <param name="product"></param>
    /// <exception cref="BO.NegativeIdException"></exception>
    /// <exception cref="BO.NoProductNameException"></exception>
    /// <exception cref="BO.NegativePriceException"></exception>
    /// <exception cref="BO.OutOfStockProductException"></exception>
    /// <exception cref="BO.BODataAlreadyExistException"></exception>
    public void Add(BO.Product product)
    {
        //check product propriety
        if (product.ID <= 0) 
            throw new BO.NegativeIdException();
        if (product.Name == "") 
            throw new BO.NoProductNameException();
        if (product.Price <= 0) 
            throw new BO.NegativePriceException();
        if(product.InStock<=0)
            throw new BO.OutOfStockProductException();
        else
        {
            DO.Product newProduct = new()
            {
                ID = product.ID,
                Name = product.Name,
                Category=product.Category,
                Price = product.Price,
                InStock=product.InStock,
            };
            try
            {
                Dal.Product.Add(newProduct);
            }
            catch(DO.DataAlreadyExistException ex)
            {
                throw new BO.BODataAlreadyExistException(ex.Message);
            }
        }
    }

    /// <summary>
        /// Delete existing product (manager screen)
        /// </summary>
        /// <param name="product"></param>
        /// <exception cref="NotImplementedException"></exception>
    public void Delete(int productID)
    {
        if(((List<DO.Product>)Dal.OrderItem.GetList()).Exists(x=>x.ID == productID))
        {
            try
            {
                Dal.Product.Delete(productID);
            }
            catch (DO.DataNotExistException ex)
            {
                throw new BO.BODataNotExistException(ex.Message);
            }
        }
        else
            throw new BO.ProductExistsInOrdersException();
    }

    /// <summary>
    /// Update product data (manager screen)
    /// </summary>
    /// <param name="product"></param>
    /// <exception cref="BO.NegativeIdException"></exception>
    /// <exception cref="BO.NoProductNameException"></exception>
    /// <exception cref="BO.NegativePriceException"></exception>
    /// <exception cref="BO.OutOfStockProductException"></exception>
    /// <exception cref="BO.BODataAlreadyExistException"></exception>
    /// <exception cref="NotImplementedException"></exception>
    public void Update(BO.Product product)
    {
        //check product propriety
        if (product.ID <= 0)
            throw new BO.NegativeIdException();
        if (product.Name == "")
            throw new BO.NoProductNameException();
        if (product.Price <= 0)
            throw new BO.NegativePriceException();
        if (product.InStock <= 0)
            throw new BO.OutOfStockProductException();
        else
        {
            DO.Product UpdatedProduct = new()
            {
                Category = (DO.Enums.Category)product.Category,
                ID = product.ID,
                Name = product.Name,
                Price = product.Price,
                InStock = product.InStock
            };

            try
            {
                Dal.Product.Update(UpdatedProduct);
            }
            catch (DO.DataNotExistException ex)
            {
                throw new BO.BODataAlreadyExistException(ex.Message);
            }
        }
    }





    public IEnumerable<BO.ProductForList> GetProductListBySort(BO.Enum.Category category)
    {
        //List<BO.ProductForList> productsList = new List<BO.ProductForList>();
        //var productsDO = Dal.Product.GetList(product => product?.Category == (DO.Enums.Category)category);
        //foreach (var prod in productsDO)
        //{

        //    productsList.Add(new BO.ProductForList()
        //    {
        //        ID = prod?.ID ?? 0,
        //        Name = prod?.Name ?? "",
        //        Price = prod?.Price ?? 0,
        //        inStock = prod?.InStock ?? 0,
        //        Category = (DO.Enums.Category)(BO.Enum.Category)prod?.Category,
        //    });
        //}
        //return productsList;



        ////get the products list from dal
        List<DO.Product?> DalProductList = (List<DO.Product?>)Dal.Product.GetList();



        //creates new BO products list
        List<BO.ProductForList> productsList = new();




        ///putting product from the DO list into the BO list
        foreach (DO.Product product in DalProductList)
        {
            if (product.Category == (DO.Enums.Category)category)
            {
                BO.ProductForList p = new()
                {
                    ID = product.ID,
                    Name = product.Name,
                    Category = product.Category,
                    Price = product.Price,
                    inStock = product.InStock
                };
                productsList.Add(p);
            }
            //creates new ProductForList items from the dal products list

        }
        return productsList;
    }
}


