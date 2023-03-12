using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace KindBee.DB
{
    public interface DBInterface : IInfrastructure<IServiceProvider>,
        IDbContextDependencies,
        IDbSetCache,
        IDbContextPoolable
    {

    }
}
