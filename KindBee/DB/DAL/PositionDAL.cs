using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace KindBee.DB.DAL
{
    public class PositionDAL : IDataAccess<Position>
    {
        KindBeeDBContext context;
        public PositionDAL(KindBeeDBContext kindBeeDBContext) {
            context = kindBeeDBContext;
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
                context.Positions.Remove(t);
                context.SaveChanges();
            }
            return t;
        }

        public IEnumerable<Position> Get()
        {
            return context.Positions.AsNoTracking().ToList();
        }

        public Position Get(int id)
        {
            return context.Positions.AsNoTracking<Position>().Where(t => t.Id == id).First();
        }

        public void Update(Position item)
        {
            context.Positions.Update(item);
            context.SaveChanges();
        }
    }
}
