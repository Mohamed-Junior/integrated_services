using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminServices.Models.Entities
{
    public class ParamsBackupOrg
    {
        public long IdOrganization { get; set; }
        public string PrefixOrganization { get; set; }
        public string NameOrganization { get; set; }
    }
}
