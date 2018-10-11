using System.DirectoryServices.Protocols;

namespace Nerdstrap.Identity.Services.DirectoryAccess.Proxies
{
    public interface LdapInterface
    {
        DirectoryConnection GetDirectoryConnection(string[] directoryServerArray, string userName, string credentials);
        SearchResponse SearchForUser(string userId, string[] attributeList, string usersDistinguishedName, DirectoryConnection directoryConnection);
        SearchResponse SearchForPasswordPolicy(string userId, string[] attributeList, string passwordPolicyDistinguishedName, DirectoryConnection adminDirectoryConnection);
        SearchResponse SearchForGroups(string groupDistinguishedName, string userId, DirectoryConnection adminDirectoryConnection);
        ModifyResponse SendUnlockGdsModifyRequest(string userDistinguishedName, DirectoryConnection adminDirectoryConnection);
        ModifyResponse SendUserPasswordModifyRequest(string userDistinguishedName, string directoryAttributeValue, DirectoryConnection adminLdapConnection);
        ModifyResponse SendPasswordPublishModifyRequest(string userDistinguishedName, DirectoryConnection adminLdapConnection, string passwordPublishStatusAttributeValue);
        ModifyResponse SendUnlockPublishModifyRequest(string userDistinguishedName, DirectoryConnection adminLdapConnection, string unlockPublishStatusAttributeValue);
        ExtendedResponse SendResetPasswordExtendedRequest(byte[] resetPasswordRequestValue, DirectoryConnection userDirectoryConnection);
    }
}
