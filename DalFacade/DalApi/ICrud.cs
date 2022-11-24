using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;


namespace DalApi;

public interface ICrud<T>
{
    int Add(T item);
    void Delete(int id);
    void Update(T item);
    void GetById(int id);
}
