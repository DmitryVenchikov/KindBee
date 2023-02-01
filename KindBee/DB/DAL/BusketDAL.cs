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
            context.Baskets.Remove(t);
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
