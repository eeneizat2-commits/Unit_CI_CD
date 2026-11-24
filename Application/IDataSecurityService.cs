using Domain;

namespace Application;

public interface IDataSecurityService
{
    Task<IReadOnlyList<DataSecurity>> GetByRoleIdsAsync(
        IReadOnlyCollection<int> roleIds,
        CancellationToken cancellationToken = default);
}