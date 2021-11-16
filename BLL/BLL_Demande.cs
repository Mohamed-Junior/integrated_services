using DSSGBOAdmin.Models.DAL;
using DSSGBOAdmin.Models.Entities;
using MyUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DSSGBOAdmin.Models.BLL
{
    public class BLL_Demande
    {

        // New Version Methods Today 21/10/2021 Demands
        public static string Add(Demande demande, string url)
        {

            demande.RegDemandDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm");
            demande.RegDemandDecision = "attends";
            demande.StatusActivationEmail = "attends";
            //demande.RegDecisionComments = " ";
            //demande.RegDemandDecisionDate = DateTime.MinValue.ToShortDateString();
            DAL_Demande.AddDemande(demande);
            var mailRepository = new MailRepository("smtp.gmail.com", 465, true, "dssgbo56@gmail.com", "dssgbo56");
            string message = mailRepository.SendEmailConfirmation(demande.Email, demande.Name, url);
            return message;
        }

        private static long CreateNewOrganizationFromDemande(long id, Demande demande, string OrganizationSystemPrefix)
        {

            Organization newOrganization = new Organization(
                                                0, demande.Name, null, demande.Name.Substring(0, 3) + id, null,
                                                demande.Affiliation, null, demande.FieldOfActivity, demande.Adress,
                                                demande.PostalCode, demande.City, demande.Country, demande.Email, demande.Phone,
                                                demande.PersonToContact, demande.ContactMail, demande.ContactPhone, demande.ContactPosition,
                                                demande.Email, " ", " ", " ", "inactive", "essai", OrganizationSystemPrefix);

            return BLL_Organization.Add(newOrganization);
        }

        private static long CreateNewUserAdminFromDemande(long newOrgId, Demande demande)
        {

            User AdminOrg = new User(0, newOrgId, "Admin" + demande.Name.ToUpper(), demande.Email,
                                   "12345678", "Administrateur", DateTime.Today,
                                    null, null, demande.Email, null);

            return BLL_User.Add(AdminOrg, true, 0, "", "", "");
        }

        private static void AcceptNewDemand(long id, Demande demande, string PrefixOrg)
        {
            long newOrgId = 0;
            long Iduser = 0;
            bool OrgCreated = false,
                 UserCreated = false,
                 DbCreated = false;

            try
            {
                newOrgId = CreateNewOrganizationFromDemande(id, demande, PrefixOrg);
                OrgCreated = true;

                if (newOrgId == 0)
                    throw new Exception($"Erreur Lors de la creation de l'organisation {demande.Name}");


                Iduser = CreateNewUserAdminFromDemande(newOrgId, demande);
                UserCreated = true;

                if (Iduser == 0)
                    throw new Exception($"Erreur Lors de la creation du nouveau administrateur de l'organisation{demande.Name}");

                string responseCreationDB = CreateDatabaseIfNotExists(PrefixOrg + "DB");

                if (responseCreationDB == null)
                    DbCreated = true;
                else
                    throw new Exception($"Erreur Creation de la base de données : {responseCreationDB}");

                // CreateFolderFromNewOrganization(PrefixOrg);

                DAL_Demande.UpdateDemande(id, demande);
            }
            catch (Exception ex)
            {
                if (DbCreated)
                    DeleteDatabaseIfExists(PrefixOrg + "DB");

                if (UserCreated)
                    BLL_User.Delete(Iduser, true, 0, "", "", "");

                if (OrgCreated)
                    BLL_Organization.Delete(newOrgId);

                throw ex;
            }
        }

        public static void Update(long id, Demande demande, string OrganizationSystemPrefix)
        {

            Demande checkDemandeExist = BLL_Demande.SelectById(id);
            if (checkDemandeExist != null && checkDemandeExist.ID > 0)
            {
                demande.RegDemandDecision = demande.RegDemandDecision.Trim().ToLower();
                demande.RegDemandDecisionDate = DateTime.Now.ToShortDateString();

                if (demande.RegDemandDecision.Equals("refuse"))
                    DAL_Demande.UpdateDemande(id, demande);

                else if (demande.RegDemandDecision.Equals("accepte"))
                {
                    AcceptNewDemand(id, demande, OrganizationSystemPrefix);
                }
                else
                    throw new Exception("Decision non correcte, essayez encore");
            }
            else
            {
                throw new Exception("Demande n'existe pas dans la base de données");
            }
        }

        public static string CreateDatabaseIfNotExists(string OrganizationSystemPrefix)
        {
            return DAL_Demande.CreateDatabaseIfNotExists(OrganizationSystemPrefix);
        }

        public static string DeleteDatabaseIfExists(string OrganizationSystemPrefix)
        {
            return DAL_Demande.DeleteDatabaseIfExists(OrganizationSystemPrefix);
        }

        public static Demande SelectById(long id)
        {
            return DAL_Demande.selectByField("Id", "" + id);
        }

        public static List<Demande> SelectAll()
        {
            return DAL_Demande.selectAll();
        }

        public static KeyValuePair<string, string> SelectByToken(string Token)
        {
            return DAL_Demande.SelectByToken(Token);
        }

        public static void UpdateDemandeStatusActivationEmail(string Token, string StatusActivationEmail)
        {
            DAL_Demande.UpdateDemandeStatusActivationEmail(Token, StatusActivationEmail);
        }

        public static void UpdateDemandeToken(string NewToken, string OldToken)
        {
            DAL_Demande.UpdateDemandeToken(NewToken, OldToken);
        }
    }
}
