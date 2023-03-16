using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace KindBee.DB.DAL
{
    public class PositionDAL : IDataAccess<Position>
    {
        KindBeeDBContext context;
        public PositionDAL(KindBeeDBContext kindBeeDBContext) {
            context = KindBeeDBContext.GetContext();
        }

        public void Add(Position item)
        {
            context.Positions.Add(item);
            context.SaveChanges();
        }

        public Position Delete(int id)
        {
            var t = Get(id);
            if (t != null)
            {
                try
                {
                    context.Positions.Remove(t);
                    context.SaveChanges();
                }
                catch(Exception ex)
                {
                    var t1 = ex;
                }
            }
            return t;
        }

        public IEnumerable<Position> Get()
        {
            //return context.Positions.AsNoTracking().ToList();
            return context.Positions.ToList();
        }

        public Position Get(int id)
        {
            //return context.Positions.AsNoTracking<Position>().Where(t => t.Id == id).First();
            return context.Positions.AsNoTracking<Position>().Where(t => t.Id == id).First();
        }

        public void Update(Position item)
        {
            context.Positions.Update(item);
            context.SaveChanges();
        }
    }
}
