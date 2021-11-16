using DSSGBOAdmin.Models.DAL;
using DSSGBOAdmin.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSSGBOAdmin.Models.BLL
{
    public class BLL_Contract
    {
        public static long Add(Contract contract)
        {
            return DAL_Contract.Add(contract);
        }

        public static void Update(long Id, Contract ContractUpdated)
        {
            ContractUpdated.DateEnd = Convert.ToDateTime(ContractUpdated.DateStart).AddMonths(ContractUpdated.NbrMois).ToShortDateString();
            DAL_Contract.Update(Id, ContractUpdated);
        }
        // Admin GBO
        public static void TerminerContract(long Id, long IdOrganization, string TypeOrganization, string NewStatus)
        {
            bool IsStatusUpdateOrg = false;
            try
            {
                //on rend l'organisation inactive lorsqu'on termine le contract
                BLL_Organization.UpdateStatusOrganization(IdOrganization, "inactive", TypeOrganization);
                IsStatusUpdateOrg = true;
                DAL_Contract.UpdateStatus(Id, NewStatus);

            }
            catch (Exception ex)
            {
                if (IsStatusUpdateOrg) // si il y a eu un problème lors de DAL_Contract.UpdateStatus alors on reactive l'organisation
                {
                    try
                    {
                        BLL_Organization.UpdateStatusOrganization(IdOrganization, "active", TypeOrganization);
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                }
                throw new Exception(ex.Message);
            }
        }

        public static void TerminerContractsByOrganization(long IdOrganization)
        {
            DAL_Contract.TerminerContractsByOrganization(IdOrganization);
        }

        public static void ActiveLastContractsByOrganization(long IdOrganization)
        {

            DAL_Contract.ActiveLastContractsByOrganization(IdOrganization);

        }

        public static void Delete(long id)
        {

            DAL_Contract.Delete(id);

        }

        public static Contract SelectById(long id)
        {

            Contract contract = DAL_Contract.SelectById(id);
            if (contract == null || (contract != null && contract.Id == 0))
            {
                throw new Exception("Ce contract n'existe pas");
            }

            return contract;


        }

        public static Contract GetCurrentContractByOrganization(long IdOrganization)
        {

            Contract contract = DAL_Contract.SelectActiveContratByOrganization(IdOrganization);
            if (contract != null && contract.Id > 0)
            {
                return contract;
            }
            throw new Exception("Pas de contract active pour cette organisation");


        }

        public static List<Contract> SelectByOrganization(long idOrg)
        {

            return DAL_Contract.SelectByOrganization(idOrg);


        }

        public static List<Contract> SelectAll()
        {

            return DAL_Contract.SelectAll();

        }
        // valid date conctarct 
        //public static bool IsContractOrganizationValide(Contract CurrentContract)
        //{

        //    if (CurrentContract != null && CurrentContract.Id > 0)
        //        return CheckValidityContractDate(CurrentContract);

        //    return false;
        //}
        // valid date & NbUser conctarct 
        //public static bool IsContractOrganizationUserValide(Contract CurrentContract)
        //{

        //    if (CurrentContract != null && CurrentContract.Id > 0 && IsContractOrganizationValide(CurrentContract))
        //        return CheckValidityContractUser(CurrentContract);

        //    return false;
        //}

        // Test validit date Contract 
        public static bool CheckValidityContractDate(Contract CurrentContract)
        {
            try
            {
                DateTime EndContractDate = DateTime.Parse(CurrentContract.DateEnd);
                double DaysContractLeft = EndContractDate.Subtract(DateTime.Now).TotalDays;

                if (DaysContractLeft >= 0)
                    return true;
            }
            catch { }

            return false;
        }
        // valid Number User conctarct 
        public static bool CheckValidityContractUser(Contract CurrentContract)
        {

            try
            {
                int ContractTotalNumberUser = CurrentContract.NbrUser;
                int CurrentNumberUser = BLL_User.CountUserByOrganization(CurrentContract.IdOrganization);

                if ((ContractTotalNumberUser - CurrentNumberUser) > 0)
                    return true;

            }
            catch { }

            return false;
        }
    }
}
