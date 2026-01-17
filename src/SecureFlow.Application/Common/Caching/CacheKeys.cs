namespace SecureFlow.Application.Common.Caching;

public static class CacheKeys
{
    public static string UserPermissions(int userId)
        => $"users:{userId}:permissions";

    public static string MenusByPermissions(string hash)
        => $"menus:{hash}";

    //Add more cache key generators as needed
}