namespace KindBee.DB.Interfaces
{
    public interface IDataAccess<T>
    {
        IEnumerable<T> Get();
        T Get(int id);
        void Add(T item);
        void Update(T item);
        T Delete(int id);
       // void RemoveAll();
        public KindBeeDBContext context { get; set; }
    }
}
