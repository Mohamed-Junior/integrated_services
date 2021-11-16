using DSSGBOAdmin.Models.BLL;
using DSSGBOAdmin.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSSGBOAdmin.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OffresController : Controller
    {

        /*********************************** MANAGE Offres SUPER ADMIN ***********************************/


        [HttpGet("")]
        public IActionResult IndexOffre()
        {
            try
            {
                var AllOffres = BLL_Offre.SelectAll();
                return Json(new { success = true, message = "Success", data = AllOffres });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }


        [HttpGet("{ID}")]
        public IActionResult DetailOffres(long ID)
        {
            try
            {
                var mOffre = BLL_Offre.SelectById(ID);
                return Json(new { success = true, message = "Success", data = mOffre });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }

        [HttpPost("")]
        public IActionResult NewOffre(Offre mOffre)
        {
            try
            {
                long NewIdOffre = BLL_Offre.Add(mOffre);
                return Json(new { success = true, message = "Success", data = NewIdOffre });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }


        [HttpPost("{ID}")]
        public IActionResult UpdateOffre(long ID, Offre mOffre)
        {

            try
            {
                BLL_Offre.Update(ID, mOffre);
                return Json(new { success = true, message = "Success", data = mOffre });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });

            }
        }

        [HttpPost("{ID}/delete")]
        public IActionResult DeleteOffre(long ID)
        {

            try
            {
                BLL_Offre.Delete(ID);
                return Json(new { success = true, message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }
    }
}