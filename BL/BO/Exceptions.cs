using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO;
public class NegativeIdException : Exception
{
    public override string Message => "ERROR: Wrong ID!";
    public override string ToString()
    {
        return Message;
    }
}

public class NoProductNameException : Exception
{
    public override string Message => "ERROR: Empty product name!";
    public override string ToString()
    {
        return Message;
    }
}

public class NegativePriceException : Exception
{
    public override string Message => "ERROR: Wrong product price!";
    public override string ToString()
    {
        return Message;
    }
}

public class OutOfStockProductException : Exception
{
    public override string Message => "ERROR: Product is out of stock!";
    public override string ToString()
    {
        return Message;
    }
}

public class ProductExistsInOrdersException : Exception
{
    public override string Message => "ERROR: Product exists in orders - you can't delete it!";
    public override string ToString()
    {
        return Message;
    }
}

public class BODataAlreadyExistException : Exception
{
    public BODataAlreadyExistException(string? message) : base(message)
    {
    }

    public override string Message => "ERROR: Product already exist!";
    public override string ToString()
    {
        return Message;
    }
}

public class BODataNotExistException : Exception
{
    public BODataNotExistException(string? message) : base(message)
    {
    }

    public override string Message => "ERROR: Product does not exist!";
    public override string ToString()
    {
        return Message;
    }
}


public class OrderAlreadyShippedException : Exception
{
    public override string Message => "ERROR: Order have already been shipped!";
    public override string ToString()
    {
        return Message;
    }
}

public class OrderAlreadySuppliedException : Exception
{
    public override string Message => "ERROR: Order have already been supplied!";
    public override string ToString()
    {
        return Message;
    }
}

