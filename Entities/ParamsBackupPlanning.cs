using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminServices.Models.Entities
{
    public class ParamsBackupPlanning
    {
        public int NbrJourInterval { get; set; }
        public string UrlToSaveBackupFileToCoud { get; set; }

        public List<string> ListOrg { get; set; }
    }
}
