﻿using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DO.Enums;

namespace BO;

public class Product
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public Enum.Category Category { get; set; }
    public int InStock { get; set; }

    public override string ToString() => $@"
        Product ID: {ID}
        Product Name: {Name}
        category: {Category}
    	Price: {Price}
    	Amount in stock: {InStock}";
}
