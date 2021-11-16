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

        [HttpGet("{Id}")]
        public IActionResult DetailOffres(long Id)
        {
            try
            {
                var mOffre = BLL_Offre.SelectById(Id);
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

        [HttpPost("{Id}")]
        public IActionResult UpdateOffre(long Id, Offre mOffre)
        {
            try
            {
                BLL_Offre.Update(Id, mOffre);
                return Json(new { success = true, message = "Success", data = mOffre });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("{Id}/delete")]
        public IActionResult DeleteOffre(long Id)
        {
            try
            {
                BLL_Offre.Delete(Id);
                return Json(new { success = true, message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
