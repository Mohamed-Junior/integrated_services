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
                if (demande != null && demande.ID > 0)
                {
                    return Json(new { success = true, message = "Demande trouvée.", data = demande });
                }
                else
                {
                    return Json(new { success = false, message = "Demande introuvable." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }


        [Route("")]
        [HttpPost]
        public JsonResult RegisterNewDemande(Demande demande,string Url)
        {
            try
            {

               string message = BLL_Demande.Add(demande, Url);
                return Json(new { success = true, message = message });

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

        [Route("Tokens/{Token}")]
        [HttpGet]
        public IActionResult GetByToken(string Token)
        {
            try
            {

                KeyValuePair<string,string> keyValuePair = BLL_Demande.SelectByToken(Token);
                if (keyValuePair.Key != null)
                {
                    return Json(new { success = true, message = "Demande trouvée.", data = keyValuePair });
                }
                else
                {
                    return Json(new { success = false, message = "Demande introuvable." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }
        [Route("UpdateDemandeStatusActivationEmail/{Token}/{StatusActivationEmail}")]
        [HttpPost]
        public JsonResult UpdateDemandeStatusActivationEmail(string Token, string StatusActivationEmail)
        {
            try
            {

                BLL_Demande.UpdateDemandeStatusActivationEmail(Token, StatusActivationEmail);
                return Json(new { success = true, message = "Success" });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [Route("UpdateDemandeToken/{NewToken}/{OldToken}")]
        [HttpPost]
        public JsonResult UpdateDemandeToken(string NewToken, string OldToken)
        {
            try
            {

                BLL_Demande.UpdateDemandeToken(NewToken, OldToken);
                return Json(new { success = true, message = "Success" });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        //[Route("{idString}")]
        //[HttpGet]
        //public IActionResult Get(string idString)
        //{
        //    try
        //    {

        //        if (long.TryParse(idString, out long id))
        //        {
        //            Demande demande = BLL_Demande.SelectById(id);
        //            if (demande != null && demande.ID > 0)
        //            {
        //                return Json(new { success = true, message = "Demand trouve", data = demande });
        //            }
        //            else
        //            {
        //                return Json(new { success = false, message = "Demand pas trouve", data = demande });
        //            }
        //        }
        //        else if (idString.ToLower().Equals("all"))
        //        {
        //            List<Demande> Demands = BLL_Demande.SelectAll();
        //            if (Demands != null && Demands.Count > 0)
        //            {
        //                return Json(new { success = true, message = "Demands trouve", data = Demands });
        //            }
        //            else
        //            {
        //                return Json(new { success = true, message = "Pas de Demands dans la base de données", data = Demands });
        //            }
        //        }
        //        return Json(new { success = false, message = " Le paramètre de la request : (' " + idString + " ')  est invalide. " });
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { success = false, message = e.Message });
        //    }
        //}

        ////[ValidateAntiForgeryToken]
        //[Route("")]
        //[HttpPost]
        //public JsonResult RegisterNewDemande(Demande demande)
        //{
        //    try
        //    {
        //        demande.RegDemandDate = DateTime.Now.ToShortDateString();
        //        demande.RegDemandDecisionDate = null;
        //        demande.RegDecisionComments = null;
        //        BLL_Demande.Add(demande);
        //        return Json(new { success = true, message = "Ajouté avec success" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}



        //// PUT api/<DemandeController>/5
        //[HttpPost("{id}")]
        //public JsonResult ResponseDemande(int id, string OrganizationSystemPrefix, [FromBody] Demande demande)
        //{
        //    try
        //    {
        //        demande.RegDemandDecisionDate = DateTime.Now.ToShortDateString();
        //        BLL_Demande.Update(id, demande, OrganizationSystemPrefix);
        //        return Json(new { success = true, message = "modifié avec success" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message });
        //    }

        //}

    }
}
