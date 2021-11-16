using DSSGBOAdmin.Models.DAL;
using DSSGBOAdmin.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSSGBOAdmin.Models.BLL
{
    public class BLL_User
    {
        public static bool CheckEmailUnicity(string Email, long IdOrganization)
        {
            return DAL_User.CheckEmailUnicity(Email, IdOrganization);
        }
        public static bool CheckNameUnicity(string Name, long IdOrganization)
        {
            return DAL_User.CheckNameUnicity(Name, IdOrganization);
        }
        // UserRequestID => User Connecté
        // PrefixOrg => OrganizationSystemPrefix
        public static long Add(User user, bool IsAdminRequest, long IdCurrentUser,string NameCurrentUser, string PrefixOrg, string ContentRootPath)
        {
            if (IsAdminRequest == false)
            {
                //  GET Current Contract Actif
                Contract CurrentContract = BLL_Contract.GetCurrentContractByOrganization(user.IdOrganization);

                // Check Number User Of Current Contract Organization
                if (BLL_Contract.CheckValidityContractUser(CurrentContract) == false)
                {
                    throw new Exception("Le nombre d'utilisateur du contrat est atteint.");
                }

            }
            long IdNewUser = DAL_User.Add(user);
            try
            {
                if (IdNewUser != 0 && IsAdminRequest == false)
                {
                    var CurrentUser = new
                    {
                        Name = user.Name,
                        Email = user.Email,
                        Role = user.Role,
                        AccountCreationDate = user.AccountCreationDate.ToString("dd/MM/yyyy"),
                    };
                    BLL_HistoryLog.SaveLog(IdCurrentUser, NameCurrentUser, PrefixOrg, new UserActionsHistory()
                    {
                        ActionName = "Ajout Utilisateur",
                        NewVaue = CurrentUser,
                        OldValue = { },
                        DateAction = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                    }, ContentRootPath);

                }
            }
            catch { }

            return IdNewUser;

        }
        //public static long Add(User user)
        //{
        //    return DAL_User.Add(user);
        //}
        //public static void Update(long id, User user)
        //{
        //    DAL_User.Update(id, user);
        //}
        public static void Update(long id, User user, bool IsAdminRequest, long IdCurrentUser, string NameCurrentUser, string PrefixOrg, string ContentRootPath)
        {
            User OldUserValue = BLL_User.SelectById(id);
            DAL_User.Update(id, user);
            try
            {
                if (IsAdminRequest == false)
                {
                    var CurrentUser = new
                    {
                        Name = user.Name,
                        Email = user.Email,
                        Role = user.Role,
                        AccountCreationDate = OldUserValue.AccountCreationDate.ToString("dd/MM/yyyy"),
                    };
                    var OldUser = new
                    {
                        Name = OldUserValue.Name,
                        Email = OldUserValue.Email,
                        Role = OldUserValue.Role,
                        AccountCreationDate = OldUserValue.AccountCreationDate.ToString("dd/MM/yyyy"),
                    };
                    BLL_HistoryLog.SaveLog(IdCurrentUser, NameCurrentUser, PrefixOrg, new UserActionsHistory()
                    {
                        ActionName = "Update Utilisateur",
                        NewVaue = CurrentUser,
                        OldValue = OldUser,
                        DateAction = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                    }, ContentRootPath);

                }
            }
            catch { }

        }
       
        public static void Delete(long id, bool IsAdminRequest, long IdCurrentUser, string NameCurrentUser, string PrefixOrg, string ContentRootPath)
        {

            User user = BLL_User.SelectById(id);
            DAL_User.Delete(id);
            try
            {
                if (IsAdminRequest == false)
                {
                    var CurrentUser = new
                    {
                        Name = user.Name,
                        Email = user.Email,
                        Role = user.Role,
                        AccountCreationDate = user.AccountCreationDate.ToString("dd/MM/yyyy"),
                    };
                    BLL_HistoryLog.SaveLog(IdCurrentUser, NameCurrentUser, PrefixOrg, new UserActionsHistory()
                    {
                        ActionName = "Delete Utilisateur",
                        NewVaue = { },
                        OldValue = CurrentUser,
                        DateAction = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                    }, ContentRootPath);
                }
            }
            catch { }


        }
        public static User SelectById(long id)
        {
            return DAL_User.SelectById(id);
        }
        public static List<User> SelectAll(long IdOrganization)
        {
            return DAL_User.SelectAll(IdOrganization);
        }
        // RQ: Lors de connection il faut verifier le contrat c'est a dire la date. 
        public static List<User> TestConnexion(string UserName, string Password,out string message)
        {
            return DAL_User.TestConnexion(UserName,Password, out message);
        }
        // RQ: Lors de RechercherCompteUser il faut verifier le contrat c'est a dire la date. 
        public static List<User> RechercherCompteUser(string Email, out string message)
        {
            return DAL_User.RechercherCompteUser(Email, out message);
        }
        public static int CountUserByOrganization(long IdOrganization)
        {
            return DAL_User.CountUserByOrganization(IdOrganization);
        }
    }
}
