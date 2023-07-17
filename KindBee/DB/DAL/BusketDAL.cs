using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KindBee.DB.DAL
{
    public class BasketDAL : IDataAccess<Basket>
    {
        public KindBeeDBContext context { get; set; }
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
            //return context.Baskets.AsNoTracking().ToList();
            return context.Baskets.ToList();
        }

        public Basket Get(int id)
        {
            //return context.Baskets.AsNoTracking<Basket>().Where(t=>t.Id==id).First();
            return context.Baskets.Where(t => t.Id == id).First();
        }

        public void Update(Basket item)
        {
            context.Baskets.Update(item);
            context.SaveChanges();
        }

    }
}
