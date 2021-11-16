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

        public static IpAdresse SelectIpAdresseById(long id)
        {
            return DAL_IpAdresse.SelectIpAdresseById(id);
        }

        public static List<IpAdresse> SelectIpAdresseByPrefixOrg(long IdOrganization)
        {
            return DAL_IpAdresse.SelectIpAdresseByPrefixOrg(IdOrganization);
        }

        public static List<IpAdresse> SelectAllIpAdresse()
        {
            return DAL_IpAdresse.SelectAllIpAdresse();
        }

        //public static List<string> SelectAllIpAdresseValidation(string PrefixOrg)
        //{
        //    return DAL_IpAdresse.SelectAllIpAdresseValidation(PrefixOrg);
        //}

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
