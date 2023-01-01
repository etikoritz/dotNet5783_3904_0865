using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi;

public interface ICrud<T> where T : struct
{
    int Add(T item);
    void Delete(int id);
    void Update(T item);
    T? GetById(int id);
    T? Get(Func<T?, bool>? condition);
    IEnumerable<T?> GetList(Func<T?, bool>? filter = null);

    //IEnumerator<T> GetList();
}