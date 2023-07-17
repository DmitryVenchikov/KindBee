using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace KindBee.DB.DAL
{
    public class OrderDAL : IDataAccess<Order>
    {
        public KindBeeDBContext context { get; set; }
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
            if (t != null)
            {
                context.Orders.Remove(t);
                context.SaveChanges();
            }
            return t;
        }

        public IEnumerable<Order> Get()
        {
            //return context.Orders.AsNoTracking().ToList();
            return context.Orders.ToList();
        }

        public Order Get(int id)
        {
            //return context.Orders.AsNoTracking<Order>().Where(t => t.Id == id).First();
            return context.Orders.Where(t => t.Id == id).First();
        }

        public void Update(Order item)
        {
            context.Orders.Update(item);
            context.SaveChanges();
        }
    }
}
