using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSSGBOAdmin.Models.BLL;
using DSSGBOAdmin.Models.Entities;
using DSSGBOAdmin.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DSSGBOAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : Controller
    {
        private readonly IWebHostEnvironment webHostingEnvironment;
        public OrganizationController(IWebHostEnvironment environment)
        {
            webHostingEnvironment = environment;
        }

        [Route("{idString}")]
        [HttpGet]
        public IActionResult Get(string idString)
        {
            try
            {
                if (long.TryParse(idString, out long id))
                {
                    Organization organization = BLL_Organization.SelectById(id);
                    if (organization != null && organization.Id > 0)
                    {
                        return Json(new { success = true, message = "Organisation trouvée", data = organization });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Organisation introuvable.", data = organization });
                    }
                }
                else if (idString.ToLower().Equals("all"))
                {
                    List<Organization> Organizations = BLL_Organization.SelectAll();
                    return Json(new { success = true, message = "Organisations trouvées", data = Organizations });
                }
                return Json(new { success = false, message = "Paramètre : ' " + idString + " ' invalide. " });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erreur serveur: " + ex.Message });
            }
        }

        [Route("")]
        [HttpPost]
        public JsonResult Post([FromForm] Organization organization)
        {
            try
            {
                BLL_Organization.Add(organization);
                return Json(new { success = true, message = "Organisation ajouté avec success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erreur serveur: " + ex.Message });
            }
        }

        [Route("{id}")]
        [HttpPut]
        public JsonResult Put(long id, [FromBody] Organization organization, long UserRequestId, string UserRequestName, string PrefixOrg)
        {
            try
            {
                bool IsAdminRequest = false;
                if (PrefixOrg.Equals(MyHelpers.IdentifiantAdminRequest))
                    IsAdminRequest = true;
                BLL_Organization.Update(id, organization, IsAdminRequest, UserRequestId, UserRequestName, PrefixOrg, webHostingEnvironment.ContentRootPath);
                return Json(new { success = true, message = "Organisation modifié avec succès" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erreur serveur: " + ex.Message });
            }
        }

        // DELETE api/<OrganizationController>/5
        [Route("{id}")]
        [HttpDelete]
        public JsonResult Delete(long id)
        {
            try
            {
                BLL_Organization.Delete(id);
                return Json(new { success = true, message = "Organisation supprimé avec succès" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("{id}/statistic")] //changer l'url pour avoir les statistiques
        public JsonResult GetStatisticOrganization(long id)
        {
            try
            {
                var result = BLL_Organization.GetStatsOrganization(id); //on va prendre le chemin de GBO a partir de la table Paramter
                return Json(new { success = true, message = "", data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("{id}/status")]
        public JsonResult UpdateStatusOrganization(long id, string NewStatus, string NewType)
        {
            try
            {
                BLL_Organization.UpdateStatusOrganization(id, NewStatus, NewType);
                return Json(new { success = true, message = "modifié avec success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("{id}/active")]
        public JsonResult ActiveOrganization(long id, Contract contract)
        {
            try
            {
                contract = BLL_Organization.ActiveOrganization(id, contract);
                return Json(new { success = true, message = "activé avec success", data = contract });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("{id}/desactive")]
        public JsonResult DesactiveOrganization(long id, string OldTypeOrganization)
        {
            try
            {
                BLL_Organization.DesactiveOrganization(id, OldTypeOrganization);
                return Json(new { success = true, message = "desactivé avec success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
