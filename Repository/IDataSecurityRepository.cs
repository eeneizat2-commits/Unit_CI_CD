
using Domain;

namespace Infrastructure.Repositories;

public interface IDataSecurityRepository
{
    Task<IReadOnlyList<DataSecurity>> GetByRoleIdsAsync(
        IReadOnlyCollection<int> roleIds,
        CancellationToken cancellationToken = default);
}
