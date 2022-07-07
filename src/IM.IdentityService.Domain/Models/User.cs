// using System.ComponentModel.DataAnnotations;
// using Microsoft.AspNetCore.Identity;
//
// namespace IM.IdentityService.Domain.Models;
//
// public class User : IdentityUser<int>, IEntity<int>
// {
//     /// <summary>
//     /// <inheritdoc cref="IdentityUser.UserName"/>
//     /// </summary>
//     [MaxLength(64)]
//     public override string UserName { get; set; }
//
//     /// <summary>
//     /// <inheritdoc cref="IdentityUser{string}.NormalizedUserName"/>
//     /// </summary>
//     [ProtectedPersonalData]
//     [MaxLength(64)]
//     public override string NormalizedUserName { get; set; }
//     
//     [ProtectedPersonalData]
//     [MaxLength(125)]
//     public string? FirstName { get; set; }
//
//     [ProtectedPersonalData]
//     [MaxLength(125)]
//     public string? MiddleName { get; set; }
//
//     [ProtectedPersonalData]
//     [MaxLength(125)]
//     public string? LastName { get; set; }
//     
//     /// <summary>
//     /// <inheritdoc cref="IdentityUser{string}.Email"/>
//     /// </summary>
//     [ProtectedPersonalData]
//     [MaxLength(255)]
//     public override string? Email { get; set; }
//
//     /// <summary>
//     /// <inheritdoc cref="IdentityUser{string}.NormalizedEmail"/>
//     /// </summary>
//     [ProtectedPersonalData]
//     [MaxLength(255)]
//     public override string? NormalizedEmail { get; set; }
//     
//     [ProtectedPersonalData]
//     public DateTime? DateOfBirth { get; set; }
//
//     public DateTime Created { get; set; }
//     public DateTime? Updated { get; set; }
//     
//     public List<ApplicationUser>? ApplicationUsers { get; init; }
// }
