using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSSGBOAdmin.Models.BLL;
using DSSGBOAdmin.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DSSGBOAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemandeController : Controller
    {

        [Route("")]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<Demande> demandes = BLL_Demande.SelectAll();
                return Json(new { success = true, message = "Demandes trouvées.", data = demandes });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        [Route("{IdDemende}")]
        [HttpGet]
        public IActionResult Get(long IdDemende)
        {
            try
            {
                Demande demande = BLL_Demande.SelectById(IdDemende);
                if (demande != null && demande.Id > 0)
                {
                    return Json(new { success = true, message = "Demande trouvée.", data = demande });
                }
                else
                {
                    return Json(new { success = true, message = "Demande introuvable." });
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }


        [Route("")]
        [HttpPost]
        public JsonResult RegisterNewDemande(Demande demande)
        {
            try
            {

                BLL_Demande.Add(demande);
                return Json(new { success = true, message = "Ajouté avec success" });


            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost("{id}")]
        public JsonResult ResponseDemande(int id, string OrganizationSystemPrefix, [FromBody] Demande demande)
        {
            try
            {

                BLL_Demande.Update(id, demande, OrganizationSystemPrefix);
                return Json(new { success = true, message = "modifié avec success" });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }

    }
}
