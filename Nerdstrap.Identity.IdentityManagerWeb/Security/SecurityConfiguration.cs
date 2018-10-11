using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;

namespace Nerdstrap.Identity.IdentityManagerWeb.Security
{
    public static class SecurityConfiguration
    {
        public static string SigningKey = ConfigurationManager.AppSettings["jwt.signingKey"];
        public static string TokenIssuer = ConfigurationManager.AppSettings["jwt.tokenIssuer"];
        public static string TokenAudience = ConfigurationManager.AppSettings["jwt.tokenAudience"];
        public static int TokenLifetimeInMinutes = int.Parse(ConfigurationManager.AppSettings["jwt.tokenLifetimeInMinutes"]);

        public static SecurityKey SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey));
        public static SigningCredentials SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

        //public static string EncryptionSigningKey = ConfigurationManager.AppSettings["jwt.encryptionSigningKey"];
        //public static SecurityKey EncryptingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EncryptionSigningKey));
        //public static EncryptingCredentials EncryptingCredentials = new EncryptingCredentials(EncryptingKey, SecurityAlgorithms.HmacSha256, SecurityAlgorithms.Aes256CbcHmacSha512);
    }
}
