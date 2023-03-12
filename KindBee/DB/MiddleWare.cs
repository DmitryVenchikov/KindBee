namespace KindBee.DB
{
    public class MiddleWare
    {
        private readonly RequestDelegate _next;
        readonly  KindBeeDBContext _context;
        public MiddleWare(RequestDelegate next, KindBeeDBContext context)
        {
            _next = next;
            _context = context;
        }

        public async Task Invoke(HttpContext context)
        {
            
        }
    }
}
