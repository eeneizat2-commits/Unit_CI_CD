
using Domain;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Repositories;

public class DataSecurityRepository : IDataSecurityRepository
{
    private readonly AppDbContext _context;

    public DataSecurityRepository(AppDbContext context)
    {
        _context = context;
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

        return await _context.DataSecurities
            .AsNoTracking()
            .Where(x => ids.Contains(x.RoleId))
            .ToListAsync(cancellationToken);
    }
}
