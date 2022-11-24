using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public struct Enum
{
    public enum Category
    {
        Laptop, DesktopComputer, Tablet, Cellphone, Headphones
    }
    public enum OrderStatus
    {
        Confirmed, Shipped, Delivered
    }
}
