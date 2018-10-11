namespace Nerdstrap.Identity.Services.DirectoryAccess.Enums
{
    public enum ResetCredentialsResultEnum
    {
        Default = 0,
        Success = 200,
        Fail = 300,
        PolicyViolation = 301,
        DoesNotExist = 400,
        Error = 500
    }
}
