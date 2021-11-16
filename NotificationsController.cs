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
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("{Id}")]
        public IActionResult DetailNotification(long Id)
        {
            try
            {
                var mNotification = BLL_Notification.SelectById(Id);
                return Json(new { success = true, message = "Success", data = mNotification });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
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

        [HttpPost("{Id}")]
        public IActionResult PutAsNotSeen(long Id)
        {
            try
            {
                BLL_Notification.Update(Id, 0);
                return Json(new { success = true, message = "Success", data = Id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("{Id}/delete")]
        public IActionResult DeleteNotification(long Id)
        {
            try
            {
                BLL_Notification.Delete(Id);
                return Json(new { success = true, message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
