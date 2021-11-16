using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSSGBOAdmin.Models.BLL;
using DSSGBOAdmin.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DSSGBOAdmin.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : Controller
    {

        [HttpGet("")]
        public JsonResult GetAllContracts()
        {
            try
            {
                List<Contract> AllContracts = BLL_Contract.SelectAll();
                return Json(new { success = true, message = "Success", data = AllContracts });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        [HttpGet("{ID}")]
        public JsonResult GetContractById(long ID)
        {
            try
            {
                Contract contract = BLL_Contract.SelectById(ID);
                return Json(new { success = true, message = "Success", data = contract });

            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        [HttpGet("organizations/{IdOrganization}/active")] //Changer l'url pour obtenir le contract active de l'organization
        public JsonResult GetCurrentContractByOrganization(long IdOrganization)
        {
            try
            {
                Contract contract = BLL_Contract.GetCurrentContractByOrganization(IdOrganization);
                return Json(new { success = true, message = "Success", data = contract });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        [HttpGet("organizations/{idOrganization}")] //Changer l'url pour obtenir tous les contracts de l'organization
        public JsonResult GetAllContractsByOrganization(long idOrganization)
        {
            try
            {
                List<Contract> AllContracts = BLL_Contract.SelectByOrganization(idOrganization);
                return Json(new { success = true, message = "Success", data = AllContracts });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        [HttpPost("")]
        public JsonResult AddNewContract(Contract contract)
        {
            try
            {
                long idContract = BLL_Contract.Add(contract);
                return Json(new { success = true, message = "Ajouté avec success", data = idContract });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        [HttpPost("{ID}")]
        public JsonResult UpdateContracts(long ID, Contract contract)
        {
            try
            {
                BLL_Contract.Update(ID, contract);
                return Json(new { success = true, message = "Modifié avec success" });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        [HttpPost("{ID}/delete")]
        public JsonResult DeleteContracts(long ID)
        {
            try
            {
                BLL_Contract.Delete(ID);
                return Json(new { success = true, message = "Supprimé avec success" });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        [HttpPost("{ID}/terminer/{IdOrganization}", Name = "terminerContract")]
        public JsonResult TerminerContract(long ID, long IdOrganization, string TypeOrganization)
        {
            try
            {
                BLL_Contract.TerminerContract(ID, IdOrganization, TypeOrganization, "terminer");
                return Json(new { success = true, message = "Terminé avec success" });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }

        }

    }
}
