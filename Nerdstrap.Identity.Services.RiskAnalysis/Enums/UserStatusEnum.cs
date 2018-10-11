namespace Nerdstrap.Identity.Services.RiskAnalysis.Enums
{
    public enum UserStatusEnum
    {
        Default = 0,
        NotEnrolled = 100,
        UnVerified = 200,
        Verified = 300,
        Delete = 400,
        Lockout = 500,
        UnLocked = 600
    }
}
