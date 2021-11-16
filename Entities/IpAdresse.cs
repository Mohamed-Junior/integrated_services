using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSSGBOAdmin.Models.Entities
{
    public class IpAdresse
    {
        public long Id { get; set; }

        public string NameOrg { get; set; }

        public long IdOrganization { get; set; }

        public string IpValue { get; set; }

        public IpAdresse() { }
    }
}
