using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;

namespace KindBee.DB.DAL
{
    public class BusketDAL : IDataAccess<Basket>
    {
        KindBeeDBContext context;
        public BusketDAL(KindBeeDBContext kindBeeDBContext) {
            context = kindBeeDBContext;
        }
        public void Add(Basket item)
        {
            context.Baskets.Add(item);
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
