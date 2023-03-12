using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;
using KindBee.Models;
using Microsoft.EntityFrameworkCore;

namespace KindBee.DB.DAL
{
    public class ProductDAL : IDataAccess<Product>
    {
        KindBeeDBContext context;
        public ProductDAL(KindBeeDBContext kindBeeDBContext) {
            context = kindBeeDBContext;
        }
        public void Add(Product item)
        {
            context.Products.Add(item);
            context.SaveChanges();
        }

        public Product Delete(int id)
        {
            var t = Get(id);
            if(t!=null)
            {
                context.Products.Remove(t);
                context.SaveChanges();
            }
            return t;
        }

        public IEnumerable<Product> Get()
        {
            return context.Products.AsNoTracking();
        }

        public Product Get(int id)
        {
            // Entry<Product>(t).State = EntityState.Detached;
           // t.State = EntityState.Detached;

            return context.Products.AsNoTracking<Product>().Where(t=>t.Id==id).First();
        }

        public void Update(Product item)
        {
            context.Entry(item).State= EntityState.Detached;
            //context.SaveChanges();
            context.Set<Product>().Update(item);
            context.SaveChanges();
        }
    }
}
