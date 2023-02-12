using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;

namespace KindBee.DB.DAL
{
    public class BasketDAL : IDataAccess<Basket>
    {
        KindBeeDBContext context;
        public BasketDAL(KindBeeDBContext kindBeeDBContext) {
            context = kindBeeDBContext;
        }
        public void Add(Basket item)
        {
            context.Baskets.Add(item);
            context.SaveChanges();
        }

        public Basket Delete(int id)
        {
            var t = Get(id);
            if (t != null)
            {
                context.Baskets.Remove(t);
                context.SaveChanges();
            }
            return t;
        }

        public IEnumerable<Basket> Get()
        {
            return context.Baskets;
        }

        public Basket Get(int id)
        {
            return context.Baskets.Find(id);
        }

        public void Update(Basket item)
        {
            context.Baskets.Update(item);
            context.SaveChanges();
        }
    }
}
