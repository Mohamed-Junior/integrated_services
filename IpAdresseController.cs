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
    public class IpAdresseController : Controller
    {

        [HttpGet("")]
        public JsonResult GetAllIpAdresses()
        {
            try
            {
                List<IpAdresse> AllIpAdresses = BLL_IpAdresse.SelectAllIpAdresse();
                return Json(new { success = true, message = "Success", data = AllIpAdresses });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        [HttpGet("{IdOrganization}")]
        public JsonResult GetAllIpAdressesByPrefixOrg(long IdOrganization)
        {
            try
            {

                List<IpAdresse> AllIpAdresses = BLL_IpAdresse.SelectIpAdresseByPrefixOrg(IdOrganization);
                return Json(new { success = true, message = "Success", data = AllIpAdresses });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        [HttpPost("")]
        public JsonResult AddNewIpAdresses(IpAdresse ipAdresse)
        {
            try
            {

                BLL_IpAdresse.AddIpAdresse(ipAdresse);
                return Json(new { success = true, message = "Ajouté avec success" });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }


        [HttpPost("{ID}")]
        public JsonResult UpdateIpAdresses(long ID, IpAdresse ipAdresse)
        {
            try
            {

                BLL_IpAdresse.UpdateIpAdresse(ID, ipAdresse);
                return Json(new { success = true, message = "Modifié avec success" });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        [HttpPost("{Id}/delete")]
        public JsonResult DeleteIpAdresses(long Id)
        {
            try
            {

                BLL_IpAdresse.DeleteIpAdresse(Id);
                return Json(new { success = true, message = "Supprimé avec success" });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

    }
}
