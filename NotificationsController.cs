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
    public class NotificationsController : Controller
    {

        /*********************************** MANAGE Notifications SUPER ADMIN ***********************************/


        [HttpGet("")]
        public JsonResult IndexNotification(bool IsDashboard)
        {
            try
            {
                var AllNotifications = BLL_Notification.SelectAll(IsDashboard);
                return Json(new { success = true, message = "Success", data = AllNotifications });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message});
            }

        }


        [HttpGet("{ID}")]
        public IActionResult DetailNotification(long ID)
        {
            try
            {
                var mNotification = BLL_Notification.SelectById(ID);
                return Json(new { success = true, message = "Success", data = mNotification });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message});
            }

        }
        
        [HttpPost("")]
        public IActionResult NewNotification(Notification mNotification)
        {
            try
            {
                long NewIdNotification = BLL_Notification.Add(mNotification);
                return Json(new { success = true, message = "Success", data = NewIdNotification });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }

        [HttpPost("{ID}")]
        public IActionResult PutAsNotSeen(long ID)
        {
            try
            {
                BLL_Notification.Update(ID, 0);
                return Json(new { success = true, message = "Success", data = ID });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }


        [HttpPost("{ID}/delete")]
        public IActionResult DeleteNotification(long ID)
        {

            try
            {
                BLL_Notification.Delete(ID);
                return Json(new { success = true, message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }


        }



    }
}
