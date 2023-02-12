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
    public override string Message => "ERROR: the price is not valid";
    public override string ToString()
    {
        return Message;
    }
}


public class OutOfStockProductSException : Exception
{
    public OutOfStockProductSException(string? message) : base(message)
    {
    }

    //public override string Message => "ERROR: Product is out of stock!";
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

/// <summary>
/// //////////
/// </summary>
public class NoCustomerNameException : Exception
{
    public override string Message => "ERROR: Empty customer name!";
    public override string ToString()
    {
        return Message;
    }
}
public class NoCustomerAddressException : Exception
{
    public override string Message => "ERROR: Empty Address name!";
    public override string ToString()
    {
        return Message;
    }
}
public class NoCustomerEmailException : Exception
{
    public override string Message => "ERROR: Empty email address!";
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

public class InvalidChoisException : Exception
{
    public override string Message => "ERROR: please enter valid choise";
    public override string ToString()
    {
        return Message;
    }
}

public class IlegalDataException : Exception
{
    //public override string Message;
    public IlegalDataException(string? message) : base(message)
    {
    }
    public override string ToString()
    {
        return Message;
    }
}

