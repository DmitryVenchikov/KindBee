using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;

namespace KindBee.DB.DAL
{
    public class OrderDAL : IDataAccess<Order>
    {
        KindBeeDBContext context;
        public OrderDAL(KindBeeDBContext kindBeeDBContext) {
            context = kindBeeDBContext;
        }
        public void Add(Order item)
        {
            context.Orders.Add(item);
            context.SaveChanges();
        }

        public Order Delete(int id)
        {
            var t = Get(id);
            context.Orders.Remove(t);
            return t;
        }

        public IEnumerable<Order> Get()
        {
            return context.Orders;
        }

        public Order Get(int id)
        {
            return context.Orders.Find(id);
        }

        public void Update(Order item)
        {
            context.Orders.Update(item);
            context.SaveChanges();
        }
    }
}
