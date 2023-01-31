using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;

namespace KindBee.DB.DAL
{
    public class CustomerDAL : IDataAccess<Customer>
    {
        KindBeeDBContext context;
        public CustomerDAL(KindBeeDBContext kindBeeDBContext) {
            context = kindBeeDBContext;
        }
        public void Add(Customer item)
        {
            context.Customers.Add(item);
        }

        public Customer Delete(int id)
        {
            var t = Get(id);
            context.Customers.Remove(t);
            return t;
        }

        public IEnumerable<Customer> Get()
        {
            return context.Customers;
        }

        public Customer Get(int id)
        {
            return context.Customers.Find(id);
        }

        public void Update(Customer item)
        {
            context.Customers.Update(item);
            context.SaveChanges();
        }
    }
}
