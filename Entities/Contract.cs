using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSSGBOAdmin.Models.Entities
{
    public class Contract
    {

        public long   Id                { get; set; }
        public long   IdOrganization     { get; set; }
        public string NameOrganization   { get; set; }
        public string StatusOrganisation { get; set; }
        public string TypeOrganization   { get; set; }
        public string Disk               { get; set; }
        public string DateStart          { get; set; }
        //public string DateEnd            { get; set; }
        public string Status             { get; set; }
        public int    NbrUser            { get; set; }
        public int    NbrMois            { get; set; }

        public Contract() { }
        
        public Contract(Contract contract) {
            Id                 = contract.Id;
            IdOrganization     = contract.IdOrganization;
            NameOrganization   = contract.NameOrganization;
            StatusOrganisation = contract.StatusOrganisation;
            TypeOrganization   = contract.TypeOrganization;
            NbrMois            = contract.NbrMois;
            NbrUser            = contract.NbrUser;
            Disk               = contract.Disk;
            DateStart          = contract.DateStart;
            //DateEnd            = contract.DateEnd;
            Status             = contract.Status;
        }

    }
}
