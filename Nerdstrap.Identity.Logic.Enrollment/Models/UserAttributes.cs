using System;
using System.Collections.Generic;

namespace Nerdstrap.Identity.Logic.Enrollment.Models
{
    public class UserAttributes
    {
        public string DisplayName { get; set; }
        public bool Locked { get; set; }
        public bool WirelessUser { get; set; }
        public bool ExternalPartner { get; set; }
    }
}
