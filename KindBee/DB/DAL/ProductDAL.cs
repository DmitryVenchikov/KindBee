using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;

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
            return context.Products;
        }

        public Product Get(int id)
        {
            return context.Products.Find(id);
        }

        public void Update(Product item)
        {
            context.Products.Update(item);
            context.SaveChanges();
        }
    }
}
