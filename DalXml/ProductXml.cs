using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal;

internal class ProductXml : IProduct
{
    XElement productRoot;
    string PPath = @"ProductXml.xml";
    const string s_product = "Product"; //linq to XML

    public ProductXml()
    {
        if (!File.Exists(s_product))
            CreateFiles();
        else
            LoadData();
    }

    /// <summary>
    /// Saves the products to xml list
    /// </summary>
    /// <param name="productList"></param>
    public void SaveProductList(List<Product> productList)
    {
        productRoot = new XElement("products",
                                from p in productList
                                select new XElement("product",
                                new XElement("id", p.ID),
                                new XElement("name", p.Name),
                                new XElement("Price", p.Price),
                                new XElement("Category", p.Category),
                                new XElement("InStock", p.InStock))
                                );
        productRoot.Save(PPath);
    }

    private void CreateFiles()
    {
        productRoot = new XElement("Products");
        productRoot.Save(s_product);
    }

    private void LoadData()
    {
        try
        {
            productRoot = XElement.Load(s_product);
        }
        catch
        {
            Console.WriteLine("File upload problem");
        }
    }  

    static DO.Product? CreateProductFromXElement(XElement s)
    {
        return new DO.Product()
        {
            //ID = s.ToIntNullable ("ID") ?? throw new FormatException("id"),
            ID = Convert.ToInt32(s.Parent.Element("ID").Value),
            Name = s.Parent.Element("Name").Value,
           // Category = Enums.Category(s.Parent.Element("Category").Value),
            InStock = Convert.ToInt32(s.Parent.Element("InStock").Value),
            //?? throw new FormatException("InStock"),
            Price = Convert.ToDouble(s.Parent.Element("Price").Value)
        };
    }


    public List<DO.Product> GetProductsList()
    {
        LoadData();
        List<Product> products;
        try
        {
            products = (from p in productRoot.Elements()
                        select new DO.Product()
                        {
                            ID = Convert.ToInt32(p.Element("ID").Value),
                            Name = p.Element("name").Value,
                            Category = (Enums.Category)p.ToEnumNullable<Enums.Category>("Category").Value,
                            InStock = Convert.ToInt32(p.Element("InStock").Value),
                            Price = Convert.ToDouble(p.Element("Price"))
                        }).ToList();
        }
        catch
        {
            products = null;
        }
        return products;
    }

    public Product GetProduct(int id)
    {
        LoadData();
        Product? product;
        try
        {
            product = (from p in productRoot.Elements()
                       where Convert.ToInt32(p.Element("ID").Value) == id
                       select new Product()
                       {
                           ID = Convert.ToInt32(p.Element("ID").Value),
                           Name = p.Element("name").Value,
                           Category = (Enums.Category)p.ToEnumNullable<Enums.Category>("Category").Value,
                           InStock = Convert.ToInt32(p.Element("InStock").Value),
                           Price = Convert.ToDouble(p.Element("Price"))

                       }).FirstOrDefault();
        }
        catch
        {
            product = null;
        }
        return (Product)product;
    }

    /// <summary>
    /// Get list of all products with XElement
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Product?> GetList(Func<Product?, bool>? filter = null)
    {
        XElement? productRootElem = XMLTools.LoadListFromXMLElement(s_product);
        if (filter != null)
        {
            return from s in productRootElem.Elements()
                   let doProd = CreateProductFromXElement(s)
                   where filter(doProd)
                   select (Product?)doProd;
        }
        else
            return from s in productRootElem.Elements()
                   select CreateProductFromXElement(s);
    }

    /// <summary>
    /// Get product by ID with XElement
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public Product? GetById(int id)
    {
        XElement? productRootElem = XMLTools.LoadListFromXMLElement(s_product);
        return (from s in productRootElem?.Elements()
                where s.ToIntNullable("ID") == id
                select (DO.Product?)CreateProductFromXElement(s)).FirstOrDefault()
                ?? throw new Exception("Missing ID");
    }

    /// <summary>
    /// Add new product with XElement
    /// </summary>
    /// <param name="doProduct"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public int Add(Product doProduct)
    {
        XElement? productRootElem = XMLTools.LoadListFromXMLElement(s_product);
        XElement? prod = (from pr in productRootElem?.Elements()
                          where pr.Parent.ToIntNullable("ID").Value == doProduct.ID
                          select pr).FirstOrDefault();
        if (prod != null)
            throw new Exception("ID already exist");

        XElement productElem = new XElement("Product",
                                    new XElement("ID", doProduct.ID),
                                    new XElement("Name", doProduct.Name),
                                    new XElement("InStock", doProduct.InStock),
                                    new XElement("Price", doProduct.Price),
                                    new XElement("Category", doProduct.Category));
        productRootElem.Add(productElem);
        XMLTools.SaveListToXMLElement(productRootElem, s_product);
        return doProduct.ID;
    }

    /// <summary>
    /// Delete product by ID with XElement
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(int id)
    {
        XElement productRootElem = XMLTools.LoadListFromXMLElement(s_product);
        XElement? prod = (from pr in productRootElem?.Elements()
                          where (int?)pr.Parent.Element("ID") == id
                          select pr).FirstOrDefault() ?? throw new Exception("Missing ID");

        prod.Remove(); // remove prod from productRootElem
        XMLTools.SaveListToXMLElement(productRootElem, s_product);
    }

    /// <summary>
    /// Update product
    /// </summary>
    /// <param name="item"></param>
    public void Update(Product item)
    {
        Delete(item.ID);
        Add(item);
    }

    public IEnumerable<Product?> DoGetProductListBySort(Func<Product?, bool>? condition)
    {
        throw new NotImplementedException();
    }

    public Product? Get(Func<Product?, bool>? condition)
    {
        throw new NotImplementedException();
    }
}