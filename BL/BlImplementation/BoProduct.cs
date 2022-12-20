using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;

namespace BlImplementation;
internal class BoProduct : IProduct
{
    private DalApi.IDal Dal = new Dal.DalList();
    public static int count = 0;

    /// <summary>
    /// Get list of products from DO
    /// </summary>
    /// <returns>Returns the list as a BO ProductForList</returns>
    public IEnumerable<BO.ProductForList> GetProductList()
    {
        ////creates random to inisialise amount in stock randomly to the initialise list
        //Random random = new Random();

        //get the products list from dal
        List<DO.Product> DalProductList = Dal.Product.GetList();

        //creates new BO products list
        List<BO.ProductForList> productsList = new();

        /////
        /////the folowing "if" take the list from DO and add to each product a variable of "InStock"
        /////updating it randomely and add it to the new list of "itemForList"
        /////(we have 10 objects in the inisialised list, so the loop runs 10 times)
        /////
        //if (count<10)
        //{
        //    foreach (DO.Product product in DalProductList)
        //    {

        //        //creates new ProductForList items from the dal products list
        //        BO.ProductForList p = new BO.ProductForList();
        //        p.ID = product.ID;
        //        p.Name = product.Name;
        //        p.Category = product.Category;
        //        p.Price = product.Price;
        //        p.inStock = random.Next(5, 100);
        //        count++;
        //        productsList.Add(p);
        //    }
        //}

        ///
        ///for adding new product to the list (after inisialsition)
        ///
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
                DO.Product p = Dal.Product.GetById(ID);
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
                DO.Product p = Dal.Product.GetById(ID);

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
        if(Dal.OrderItem.GetList().Exists(x=>x.ID!=productID))
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
                Category = product.Category,
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
}
