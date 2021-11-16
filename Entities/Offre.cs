using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSSGBOAdmin.Models.Entities
{
    public class Offre
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int NbrMois { get; set; }

        public int NbrUser { get; set; }

        public int NbrCPU { get; set; }

        public string Disk { get; set; }

        public string RAM { get; set; }

        public double Price { get; set; }

        public Offre() { }
    }
}
