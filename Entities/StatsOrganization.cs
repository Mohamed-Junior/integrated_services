using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSSGBOAdmin.Models.Entities
{
    public class StatsOrganization
    {
        public long IdOrganization { get; set; }
        public string DiskUsage { get; set; }
        public string FolderNumbers { get; set; }
        public string UserNumbers { get; set; }
        public string MessageErreurFolder { get; set; }
        public string MessageErreurNumberUser { get; set; }
        public int[] NumberFileByMonth { get; set; }
        public StatsOrganization() { }
    }
}
