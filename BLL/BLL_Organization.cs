using DSSGBOAdmin.Models.DAL;
using DSSGBOAdmin.Models.Entities;
using DSSGBOAdmin.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DSSGBOAdmin.Models.BLL
{
    public class BLL_Organization
    {
        public static long Add(Organization organization)
        {
            return DAL_Organization.Add(organization);
        }

        public static void Update(long Id, Organization organization, bool IsAdminRequest, long IdCurrentUser, string NameCurrentUser, string PrefixOrg, string ContentRootPath)
        {
            Organization OldOrganization = BLL_Organization.SelectById(Id);
            DAL_Organization.Update(Id, organization);
            try
            {
                if (IsAdminRequest == false)
                {
                    var CurrentOrganization = new
                    {
                        NameFr = organization.NameFr,
                        NameAr = organization.NameAr,
                        Acronym = organization.Acronym,
                        OrganisationLogo = organization.OrganisationLogo,
                        Affiliation = organization.Affiliation,
                        AffiliationLogo = organization.AffiliationLogo,
                        FieldOfActivity = organization.FieldOfActivity,
                        Adress = organization.Adress,
                        PostalCode = organization.PostalCode,
                        City = organization.City,
                        Country = organization.Country,
                        Email = organization.Email,
                        Phone = organization.Phone,
                        PersonToContact = organization.PersonToContact,
                        ContactMail = organization.ContactMail,
                        ContactPhone = organization.ContactPhone,
                        ContactPosition = organization.ContactPosition,
                        ParDiffusionEmail = organization.ParDiffusionEmail,
                        ParDiffusionEmailPW = organization.ParDiffusionEmailPW,
                        ParIngoingMailChar = organization.ParIngoingMailChar,
                        ParOutgoingMailChar = organization.ParOutgoingMailChar,
                    };
                    var OldOrg = new
                    {
                        NameFr = OldOrganization.NameFr,
                        NameAr = OldOrganization.NameAr,
                        Acronym = OldOrganization.Acronym,
                        OrganisationLogo = OldOrganization.OrganisationLogo,
                        Affiliation = OldOrganization.Affiliation,
                        AffiliationLogo = OldOrganization.AffiliationLogo,
                        FieldOfActivity = OldOrganization.FieldOfActivity,
                        Adress = OldOrganization.Adress,
                        PostalCode = OldOrganization.PostalCode,
                        City = OldOrganization.City,
                        Country = OldOrganization.Country,
                        Email = OldOrganization.Email,
                        Phone = OldOrganization.Phone,
                        PersonToContact = OldOrganization.PersonToContact,
                        ContactMail = OldOrganization.ContactMail,
                        ContactPhone = OldOrganization.ContactPhone,
                        ContactPosition = OldOrganization.ContactPosition,
                        ParDiffusionEmail = OldOrganization.ParDiffusionEmail,
                        ParDiffusionEmailPW = OldOrganization.ParDiffusionEmailPW,
                        ParIngoingMailChar = OldOrganization.ParIngoingMailChar,
                        ParOutgoingMailChar = OldOrganization.ParOutgoingMailChar,
                    };
                    BLL_HistoryLog.SaveLog(IdCurrentUser, NameCurrentUser, PrefixOrg, new UserActionsHistory()
                    {
                        ActionName = "Update Organisation",
                        NewVaue = CurrentOrganization,
                        OldValue = OldOrg,
                        DateAction = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                    }, ContentRootPath);

                }

            }
            catch { }
        }

        public static void Delete(long id)
        {
            DAL_Organization.Delete(id);
        }

        public static Organization SelectById(long id)
        {
            return DAL_Organization.SelectById(id);
        }
        
        public static List<Organization> SelectAll()
        {
            return DAL_Organization.SelectAll();
        }

        // Update Status & Type of organziation using Super Admin
        public static void UpdateStatusOrganization(long Id, string StatusOrg, string NewTypeOrg)
        {
            DAL_Organization.UpdateStatusOrganization(Id, StatusOrg, NewTypeOrg);
        }

        // Statistique by Organization
        public static StatsOrganization GetStatsOrganization(long Id)
        {
            var Disk_usage = "";
            var User_Numbers = "";
            var Folder_Numbers = "";
            var NumberFileByMonth = new int[5];
            var MessageErreurFolder = "";
            var MessageErreurNumberUser = "";
            try
            {
                var org = DAL_Organization.SelectById(Id);
                var dirOrganization = Path.Combine(MyHelpers.DirOrganization, $"Courriers_{org.OrganizationSystemPrefix}");
                var space = MyHelpers.GetDirectorySpace(dirOrganization);
                NumberFileByMonth = MyHelpers.GetNumberFileByMonth(dirOrganization);
                Disk_usage = string.Format("{0:N3}", ((double)space / (double)1000000));
                Folder_Numbers = "" + MyHelpers.FileNumbers(dirOrganization);
            }
            catch (Exception e)
            {
                MessageErreurFolder = e.Message;
            }

            try
            {
                User_Numbers = BLL_User.CountUserByOrganization(Id).ToString();
            }
            catch (Exception e)
            {
                MessageErreurNumberUser = e.Message;
            }

            StatsOrganization statsOrganization = new StatsOrganization()
            {
                IdOrganization = Id,
                DiskUsage = Disk_usage,
                FolderNumbers = Folder_Numbers,
                MessageErreurFolder = MessageErreurFolder,
                UserNumbers = User_Numbers,
                MessageErreurNumberUser = MessageErreurNumberUser,
                NumberFileByMonth = NumberFileByMonth,
            };

            return statsOrganization;

        }
        
        // Active Organization by Create New Contract
        public static Contract ActiveOrganization(long Id, Contract contract)
        {
            long idAddedContract = 0;
            try
            {
                idAddedContract = BLL_Contract.Add(contract);
                BLL_Organization.UpdateStatusOrganization(Id, "active", "client");
                contract.Id = idAddedContract;
                return contract;
            }
            catch (Exception e)
            {
                if (idAddedContract != 0)
                    BLL_Contract.Delete(idAddedContract);

                throw e;
            }

        }

        // Desactive Organization by Terminate ALL Contracts
        public static void DesactiveOrganization(long IdOrganization, string OldTypeOrganization)
        {
            bool hasTermineContract = false;
            try
            {
                BLL_Contract.TerminerContractsByOrganization(IdOrganization);
                hasTermineContract = true;
                BLL_Organization.UpdateStatusOrganization(IdOrganization, "inactive", OldTypeOrganization);
            }
            catch (Exception e)
            {
                if (hasTermineContract == true)
                    BLL_Contract.ActiveLastContractsByOrganization(IdOrganization);

                throw e;
            }

        }
    }
}
