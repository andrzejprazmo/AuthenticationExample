﻿namespace WebApp.Domain.Configuration;

public class JwtSettings
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public int TokenExpire { get; set; }
    public int RefreshTokenExpire { get; set; }
}
