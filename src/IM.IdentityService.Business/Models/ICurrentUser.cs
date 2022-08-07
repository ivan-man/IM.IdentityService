﻿namespace IM.IdentityService.Business.Models;

public interface ICurrentUser
{
    public Guid Id { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public string Jti { get; init; }
    public string[] Roles { get; init; }
}
