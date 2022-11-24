using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace DO;
[Serializable]
public class DataNotExistException : Exception
{
    public int Data { get; private set; }
    public DataNotExistException() : base() { }
    public DataNotExistException(string message) : base(message) { }
    public DataNotExistException(string message, Exception inner) : base(message, inner) { }
    protected DataNotExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    // special constructor for our custom exception
    public DataNotExistException(int capacity, string message) : base(message) =>
    this.Data = Data;
    override public string ToString() =>
    "DataNotFoundException: " + Data + " does not exist\n" + Message;
}

public class DataAlreadyExistException : Exception
{
    public int Data { get; private set; }
    public DataAlreadyExistException() : base() { }
    public DataAlreadyExistException(string message) : base(message) { }
    public DataAlreadyExistException(string message, Exception inner) : base(message, inner) { }
    protected DataAlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    // special constructor for our custom exception
    public DataAlreadyExistException(int capacity, string message) : base(message) =>
    this.Data = Data;
    override public string ToString() =>
    "DataAlreadyExistException: " + Data + " already exist\n" + Message;
}
