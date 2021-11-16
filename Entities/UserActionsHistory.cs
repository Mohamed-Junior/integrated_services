using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSSGBOAdmin.Models.Entities
{
    public class UserActionsHistory
    {
        public string ActionName { get; set; }
        public object OldValue { get; set; }
        public object NewVaue { get; set; }
        public string DateAction { get; set; }
    }
}
