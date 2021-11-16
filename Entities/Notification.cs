using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSSGBOAdmin.Models.Entities
{
    public class Notification
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public int Status { get; set; }

        public string Date { get; set; }

        public Notification() { }
    }
}
