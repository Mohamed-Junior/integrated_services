using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AdminServices.Models.Entities
{
    public class Backups
    {
        public long     Id                       { get; set; }
        public long     IdOrganization           { get; set; }
        public string   OrganizationSystemPrefix { get; set; }
        public string   NameOrganization         { get; set; }
        public int      IntervalJour             { get; set; }
        public string   Status                   { get; set; }
        public string   Message                  { get; set; }
        public string   DatePlanification        { get; set; }
        public string   DateExecution            { get; set; }
        public string   Size            { get; set; }
    }
}
