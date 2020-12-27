using System.Collections.Generic;

namespace DataManager
{
    public interface IRepository<T1, T2> where T1 : class
    {
        IEnumerable<T1> GetAll();

        T1 Get(T2 id);

        //IEnumerable<T1> Find(Func<T1, Boolean> predicate);
        //void Create(T1 item);
        //void Update(T1 item);
        //void Delete(T2 id);
    }
}