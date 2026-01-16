namespace SecureFlow.Shared.Authorization;
 

public record Permission(
    string Action,
    string Resource,
    string Description)
{
    public string Name => $"Permissions.{Resource}.{Action}";
}

public static class Permissions
{
    public static readonly Permission ViewUsers =
        new(Actions.View, Resources.Users, "View users");

    public static readonly Permission CreateUsers =
        new(Actions.Create, Resources.Users, "Create users");

    public static readonly Permission UpdateUsers =
        new(Actions.Update, Resources.Users, "Update users");

    public static readonly Permission DeleteUsers =
        new(Actions.Delete, Resources.Users, "Delete users");

    public static readonly Permission UploadEvidence =
        new(Actions.Upload, Resources.Evidences, "Upload evidence");

    public static readonly Permission DownloadEvidence =
    new(Actions.Download, Resources.Evidences, "Download evidence");

    public static IEnumerable<Permission> All =>
        new[]
        {
            ViewUsers,
            CreateUsers,
            UpdateUsers,
            DeleteUsers,
            UploadEvidence,
            DownloadEvidence,

        };
}