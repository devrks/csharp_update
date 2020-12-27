using DataManager;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class PersonsService : IRepository<Person, int>
    {
        public Person Get(int id)
        {
            return GetAll().SingleOrDefault(x => x.BusinessEntityID == id);
        }

        public IEnumerable<Person> GetAll()
        {
            using (var entities = new Entities())
            {
                return entities.People;
            }
        }
    }
}