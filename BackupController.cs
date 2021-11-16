using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdminServices.Models.BLL;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using AdminServices.Models.Entities;

namespace DSSGBOAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackupController : Controller
    {

        private readonly IWebHostEnvironment webHostingEnvironment;
        private static Timer timer;
        public BackupController(IWebHostEnvironment environment)
        {
            webHostingEnvironment = environment;
        }

        [HttpGet("/timer")]
        public IActionResult SetTimerBackup()
        {

            //DateTime dateTime = new DateTime(2021, 11, 1, 24, 00, 00);
            DateTime dateTime = DateTime.Now.AddSeconds(30);
            var startTimeSpan = dateTime.Subtract(DateTime.Now);
            var periodTimeSpan = TimeSpan.FromMinutes(15);
            if (timer == null)
                timer = new Timer(async (ee) =>
                {
                    await BLL_Backup.BackupsDateExectuionIsToday(webHostingEnvironment.ContentRootPath);

                }, null, startTimeSpan, periodTimeSpan);
            return Json(new { data = "Ok" });
        }


        [HttpGet("")]
        public JsonResult GetAllBackupsIndex()
        {
            try
            {
                string AllBackupsIndex = BLL_Backup.SelectAllBackupsIndex();
                return Json(new { success = true, message = "Success", data = AllBackupsIndex });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        [HttpGet("{IdOrganization}")]
        public JsonResult GetAllBackupsByOrg(long IdOrganization)
        {
            try
            {
                List<Backups> AllBackups = BLL_Backup.SelectBackupsByOrg(IdOrganization);
                return Json(new { success = true, message = "Success", data = AllBackups });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        [HttpPost("")]
        public JsonResult AdminBackUpData(ParamsBackupOrg ParamsBackupOrg)
        {
            var isSuccess = false;
            var messageResp = "";
            try
            {
                if (ParamsBackupOrg == null || ParamsBackupOrg.IdOrganization == 0 || string.IsNullOrWhiteSpace(ParamsBackupOrg.PrefixOrganization) == true)
                    throw new Exception("Veuillez vérifier les données de la sauvegarde.");

                Task.Run(async () =>
                {
                    await BLL_Backup.BackupDataAdmin(ParamsBackupOrg, webHostingEnvironment.ContentRootPath);
                });
                isSuccess = true;
                messageResp = "Succès plannification backup de l'organisation";
            }
            catch (Exception ex)
            {
                isSuccess = false;
                messageResp = $"Erreur Backup de l'organisation. \n Message : {ex.Message} ";
            }
            return Json(new { success = isSuccess, message = messageResp });
        }

        [HttpPost("planningBackup")]
        public JsonResult AdminPlanningBackUpData(ParamsBackupPlanning paramsPlanning)
        {
            try
            {
                BLL_Backup.BackupPlanningListOrganization(paramsPlanning);
                return Json(new { success = true, message = $"Planning Succes. Les backups se feront chaque {paramsPlanning.NbrJourInterval} jour(s)." });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }
    }
}
