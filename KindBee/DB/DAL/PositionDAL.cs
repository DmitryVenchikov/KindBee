using KindBee.DB.DBModels;
using KindBee.DB.Interfaces;

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
            context.Positions.Remove(t);
            return t;
        }

        public IEnumerable<Position> Get()
        {
            return context.Positions;
        }

        public Position Get(int id)
        {
            return context.Positions.Find(id);
        }

        public void Update(Position item)
        {
            context.Positions.Update(item);
            context.SaveChanges();
        }
    }
}
