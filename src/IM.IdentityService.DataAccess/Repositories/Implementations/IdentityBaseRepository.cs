namespace IM.IdentityService.DataAccess.Repositories.Implementations;

public class IdentityBaseRepository : IIdentityBaseRepository
{
    protected readonly ServiceDbContext _context;

    protected IdentityBaseRepository(ServiceDbContext context)
    {
        _context = context;
    }

    public ServiceDbContext Context => _context;

    public async Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        string userId = default,
        CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        int? userId = default,
        CancellationToken cancellationToken = default)
    {
        return await SaveChangesAsync(acceptAllChangesOnSuccess, userId?.ToString(), cancellationToken);
    }
}
