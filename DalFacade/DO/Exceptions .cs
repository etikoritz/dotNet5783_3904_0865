using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DO;
public class DataNotExistException : Exception
{
    public override string Message => "ERROR: Doesnt exist";
    public override string ToString()
    {
        return Message;
    }
}

public class DataAlreadyExistException : Exception
{
    public override string Message => "ERROR: Alredy exist";
    public override string ToString()
    {
        return Message;
    }
}

[Serializable]

public class DalConfigException : Exception
{
    public DalConfigException(string msg) : base(msg) { }
    public DalConfigException(string msg, Exception ex) : base(msg, ex) { }
}

