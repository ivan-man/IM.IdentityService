namespace IM.IdentityService.DataAccess.Repositories;

public interface IIdentityBaseRepository 
{
    ServiceDbContext Context { get; }

    Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        string userId = default,
        CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        int? userId = default,
        CancellationToken cancellationToken = default);
}
