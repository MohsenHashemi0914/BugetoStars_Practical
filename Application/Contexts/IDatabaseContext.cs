using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Contexts
{
    public interface IDatabaseContext
    {
        #region SaveChanges

        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

        #endregion
    }
}