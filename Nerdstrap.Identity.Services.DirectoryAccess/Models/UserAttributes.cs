using System;
using System.Collections.Generic;

namespace Nerdstrap.Identity.Services.DirectoryAccess.Models
{
    public class UserAttributes
    {
        public string DistinguishedName { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }

        public bool Locked { get; set; }
        public bool Expired { get; set; }

        public string PasswordPolicyDistinguishedName { get; set; }
        public DateTime? PasswordChangedTime { get; set; }

        public bool WirelessUser { get; set; }
        public bool ExternalPartner { get; set; }
    }
}
