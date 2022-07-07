using System.ComponentModel.DataAnnotations;

namespace IM.IdentityService.Domain.Models;

public interface IEntity<TId> : IEntity
{
    [Key] TId Id { get; set; }
}

public interface IEntity
{
    DateTime Created { get; set; }

    DateTime? Updated { get; set; }
}
