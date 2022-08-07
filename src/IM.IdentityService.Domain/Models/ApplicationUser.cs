using System.ComponentModel.DataAnnotations;
using IM.Common.Models.Domain;
using IM.IdentityService.Common.Enums;
using Microsoft.AspNetCore.Identity;

namespace IM.IdentityService.Domain.Models;

public class ApplicationUser : IdentityUser<Guid>, IBaseEntity<Guid>
{
    /// <summary>
    /// <inheritdoc cref="IdentityUser.UserName"/>
    /// </summary>
    public ConfirmationType DefaultConfirmationType { get; set; }

    /// <summary>
    /// <inheritdoc cref="IdentityUser.UserName"/>
    /// </summary>
    [MaxLength(64)]
    public override string UserName { get; set; }

    /// <summary>
    /// <inheritdoc cref="IdentityUser{string}.NormalizedUserName"/>
    /// </summary>
    [ProtectedPersonalData]
    [MaxLength(64)]
    public override string NormalizedUserName { get; set; }

    [ProtectedPersonalData]
    [MaxLength(128)]
    public string? FirstName { get; set; }

    [ProtectedPersonalData]
    [MaxLength(128)]
    public string? MiddleName { get; set; }

    [ProtectedPersonalData]
    [MaxLength(128)]
    public string? LastName { get; set; }

    /// <summary>
    /// <inheritdoc cref="IdentityUser{string}.Email"/>
    /// </summary>
    [ProtectedPersonalData]
    [MaxLength(255)]
    public override string? Email { get; set; }

    /// <summary>
    /// <inheritdoc cref="IdentityUser{string}.NormalizedEmail"/>
    /// </summary>
    [ProtectedPersonalData]
    [MaxLength(250)]
    public override string? NormalizedEmail { get; set; }

    [MaxLength(2)] public string? CountryIso2 { get; set; }
    [MaxLength(3)] public string? CountryIso3 { get; set; }

    public bool? IsDisabled { get; set; }

    [ProtectedPersonalData] public DateTime? DateOfBirth { get; set; }

    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }

    public List<Application> Applications { get; set; }
}
