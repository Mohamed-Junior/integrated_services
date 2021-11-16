using DSSGBOAdmin.Models.DAL;
using DSSGBOAdmin.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DSSGBOAdmin.Models.BLL
{
    public class BLL_IpAdresse
    {
        public static long AddIpAdresse(IpAdresse ipAdresse)
        {
            return DAL_IpAdresse.AddIpAdresse(ipAdresse);
        }

        public static void UpdateIpAdresse(long id, IpAdresse ipAdresse)
        {
            DAL_IpAdresse.UpdateIpAdresse(id, ipAdresse);
        }

        public static void DeleteIpAdresse(long id)
        {
            DAL_IpAdresse.DeleteIpAdresse(id);
        }

        public static List<IpAdresse> SelectIpAdresseByOrg(long IdOrganization)
        {
            return DAL_IpAdresse.SelectIpAdresseByOrg(IdOrganization);
        }

        public static string SelectAllIpAdresseIndex()
        {
            return DAL_IpAdresse.SelectAllIpAdresseIndex();
        }

        // Verif IP oranization request.
        // RQ: Lors de connection il faut verifier l'@IP de la requette de l'organization. 
        public static bool ValidateIpAdresse(long IdOrganization, string IpUser)
        {
            bool IsValid = false;
            if (DAL_IpAdresse.CountIpAdresseValidationByOrganization(IdOrganization, IpUser) > 0)
                IsValid = true;
            return IsValid;
            //return DAL_IpAdresse.CountIpAdresseValidationByOrganization(PrefixOrg, IpUser) > 0;
        }

        // GET IPV4 From Request.
        public static string GetIpRequest(IPAddress RemoteRequestIpAdresse)
        {
            var remoteIp = RemoteRequestIpAdresse;
            if (remoteIp.IsIPv4MappedToIPv6)
            {
                remoteIp = remoteIp.MapToIPv4();
            }
            return remoteIp.ToString();
        }
    }
}
