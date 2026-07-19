using Sbdms.Ltr.Infra.Data;
using Sbdms.SharedLibrary.Common;

namespace Sbdms.Ltr.Infra.Persistence;

public class UnitOfWork(LtrAppDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);

    public void Dispose() => context.Dispose();
}
