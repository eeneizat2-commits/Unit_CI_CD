using Domain;
using Infrastructure.Repositories;

namespace Application;

public class DataSecurityService : IDataSecurityService
{
    private readonly IDataSecurityRepository _repository;

    public DataSecurityService(IDataSecurityRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<DataSecurity>> GetByRoleIdsAsync(
        IReadOnlyCollection<int> roleIds,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(roleIds);

        var ids = roleIds
            .Where(id => id > 0)
            .Distinct()
            .ToArray();

        if (ids.Length == 0)
            return Array.Empty<DataSecurity>();

        var records = await _repository.GetByRoleIdsAsync(
            ids,
            cancellationToken);

        return records
            .Where(record => record.IsActive)
            .ToArray();
    }
}