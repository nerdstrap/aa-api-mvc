using Nerdstrap.Identity.Services.DirectoryAccess.Contracts;

namespace Nerdstrap.Identity.Services.DirectoryAccess
{
	public interface IDirectoryService
    {
        LookupUserResponse LookupUser(LookupUserRequest lookupUserRequest);
        AuthenticateResponse Authenticate(AuthenticateRequest authenticateRequest);
        GetUserResponse GetUser(GetUserRequest getUserRequest);
        ResetCredentialsResponse ResetCredentials(ResetCredentialsRequest resetCredentialsRequest);
        UnlockUserResponse UnlockUser(UnlockUserRequest unlockUserRequest);
        UnlockCredentialsResponse UnlockCredentials(UnlockCredentialsRequest unlockCredentialsRequest);
    }
}
