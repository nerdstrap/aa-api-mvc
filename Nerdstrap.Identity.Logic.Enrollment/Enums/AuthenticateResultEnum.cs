namespace Nerdstrap.Identity.Logic.Enrollment.Enums
{
    public enum AuthenticateResultEnum
	{
        Default = 0,
        Allow = 200,
        Enroll = 201,
        Challenge = 202,
        Review = 203,
        Deny = 300,
        Locked = 301,
        Expired = 302,
        NotAuthorized = 303,
        None = 400,
        DoesNotExist = 404,
        Error = 500
    }
}
