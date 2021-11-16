using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSSGBOAdmin.Models.Entities
{
    public class UserHistoryLog
    {
        public long UserIdCurrent { get; set; }
        public string UserNameCurrent { get; set; }
        public string PrefixOrganization { get; set; }
        public List<UserActionsHistory> AllActionHistories { get; set; }
    }
}
