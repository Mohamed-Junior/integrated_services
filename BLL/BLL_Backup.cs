using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO.Compression;
using System.Threading.Tasks;
using DSSGBOAdmin.Utilities;
using DSSGBOAdmin.Models.DAL;
using System.Collections.Generic;
using DSSGBOAdmin.Models.Entities;

namespace DSSGBOAdmin.Models.BLL
{
    public class BLL_Backup
    {
        private static async Task<bool> SendBackupToSaveIt(byte[] FileBackupByte, string PrefixOrg, string UrlToSaveBackupFileToCoud)
        {
            try
            {
                //var urlApi = $"https://localhost:5001/backUps/{PrefixOrg}/saveBackup";
                ByteArrayContent bytes = new ByteArrayContent(FileBackupByte);
                MultipartFormDataContent multiContent = new MultipartFormDataContent { { bytes, "file", "Backup_" + PrefixOrg } };

                var requestResponseAPI = await MyHelpers.SendRequestToServiceAPI(HttpMethod.Post, UrlToSaveBackupFileToCoud + PrefixOrg, multiContent);

                var result = requestResponseAPI.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<bool>(result);

            }
            catch (Exception e)
            {
                return false;
            }


        }
        
        private static string BackupDB(string PrefixOrg, string RootPathFolder, string TimeExecution)
        {
            string backupDIR = Path.Combine(RootPathFolder, "Backup", $"Backup{PrefixOrg}Docs", $"DSS-GBO-Database-{PrefixOrg}_{TimeExecution}");
            if (!Directory.Exists(backupDIR))
            {
                Directory.CreateDirectory(backupDIR);
            }
            var databaseName = PrefixOrg + "DB";
            var path = backupDIR + "\\GBO-Database_" + PrefixOrg + "_" + TimeExecution + ".bak";
            DAL_Backup.Backup(path, databaseName);

            return backupDIR;
        }
        
        private static async Task<string> DoBackup(string PrefixOrg, string RootPathFolder, string UrlToSaveBackupToCoud)
        {
            var SizeFileBackup = "0";
            var PathDB = "";
            var PathFilesZip = "";
            var webRootResult = "";
            var PathDatabaseZip = "";
            var ResultSendSaveBackupFile = false;
            var RootPrefixOrgFolder = $"{PrefixOrg}Docs";
            var RootPathBackup = Path.Combine(RootPathFolder, "Backup");
            var RootPathUploadDocument = Path.Combine(RootPathFolder, "UploadDocument");
            var TimeExecution = DateTime.Now.ToString("dd-MM-yyyy__hh:mm:ss").Replace(":", "_");
            var RootPathOrgBackup = Path.Combine(RootPathBackup, $"Backup{RootPrefixOrgFolder}");
            var RootPathOrgUploadDocument = Path.Combine(RootPathUploadDocument, $"UploadDocument{RootPrefixOrgFolder}");
            try
            {
                if (!Directory.Exists(RootPathBackup))
                {
                    Directory.CreateDirectory(RootPathBackup);
                }

                if (!Directory.Exists(RootPathOrgBackup))
                {
                    Directory.CreateDirectory(RootPathOrgBackup);
                }

                if (!Directory.Exists(RootPathUploadDocument))
                {
                    Directory.CreateDirectory(RootPathUploadDocument);
                }

                if (!Directory.Exists(RootPathOrgUploadDocument))
                {
                    Directory.CreateDirectory(RootPathOrgUploadDocument);
                }

                if (!Directory.Exists(Path.Combine(RootPathFolder, "Courriers")))
                {
                    Directory.CreateDirectory(Path.Combine(RootPathFolder, "Courriers"));
                }

                var PathCurrentBackupDB = Path.Combine(RootPathOrgBackup, $"Backup{RootPrefixOrgFolder}_{TimeExecution}");

                if (!Directory.Exists(PathCurrentBackupDB))
                {
                    Directory.CreateDirectory(PathCurrentBackupDB);
                }

                PathDatabaseZip = Path.Combine(PathCurrentBackupDB, $"DSS-GBO-{RootPrefixOrgFolder}-Database_{TimeExecution}.zip");

                PathFilesZip = Path.Combine(PathCurrentBackupDB, $"DSS-GBO-{RootPrefixOrgFolder}-Documents_{TimeExecution}.zip");

                webRootResult = Path.Combine(RootPathOrgUploadDocument, $"DSS-GBO-{RootPrefixOrgFolder}_{TimeExecution}.zip");

                string PathFiles = Path.Combine(RootPathFolder, "Courriers", "Courriers_" + PrefixOrg);

                if (Directory.Exists(PathFiles) == false)
                    throw new Exception($"Le dossier de l'organization n'existe pas.");

                if (File.Exists(webRootResult) == false)
                {
                    PathDB = BackupDB(PrefixOrg, RootPathFolder, TimeExecution);


                    if (File.Exists(PathDatabaseZip) == false)
                        ZipFile.CreateFromDirectory(PathDB, PathDatabaseZip);
                    if (File.Exists(PathFilesZip) == false)
                        ZipFile.CreateFromDirectory(PathFiles, PathFilesZip);

                    if (Directory.Exists(PathDB))
                    {
                        Directory.Delete(PathDB, true);
                    }
                    ZipFile.CreateFromDirectory(PathCurrentBackupDB, webRootResult);

                    if (File.Exists(webRootResult))
                    {
                        File.Delete(PathDatabaseZip);
                        File.Delete(PathFilesZip);
                        Directory.Delete(PathCurrentBackupDB);
                    }
                }
                byte[] finalResult = await File.ReadAllBytesAsync(webRootResult);

                if (finalResult == null || !finalResult.Any())
                {
                    throw new Exception($"Probleme au niveau du fichier: {webRootResult} ");
                }

                using (FileStream fs = new FileStream(webRootResult, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await fs.CopyToAsync(ms);
                        finalResult = ms.ToArray();
                        await ms.DisposeAsync();
                    }
                    fs.Close();
                }

                ResultSendSaveBackupFile = await SendBackupToSaveIt(finalResult, PrefixOrg, UrlToSaveBackupToCoud);

                if (ResultSendSaveBackupFile == false)
                    throw new Exception($"Erreur lors de la suvegarde du backup de l'organisation dans le cloud. \n");
                else
                {

                    if (Directory.Exists(PathDB))
                    {
                        Directory.Delete(PathDB, true);
                    }
                    if (File.Exists(PathDatabaseZip))
                    {
                        File.Delete(PathDatabaseZip);
                    }
                    if (File.Exists(PathFilesZip))
                    {
                        File.Delete(PathFilesZip);
                    }

                    try
                    {
                        SizeFileBackup = Math.Round((new FileInfo(webRootResult).Length / double.Parse("1000000")), 2) + " MB";
                    }
                    catch { }
                    if (File.Exists(webRootResult))
                    {
                        File.Delete(webRootResult);
                    }

                    return SizeFileBackup;
                }


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string SelectAllBackupsIndex()
        {
            return DAL_Backup.SelectAllBackupsIndex();
        }

        public static List<Backups> SelectBackupsByOrg(long IdOrganization)
        {
            return DAL_Backup.SelectByOrganization(IdOrganization);
        }

        public static async Task BackupsDateExectuionIsToday(string ContentRootPath)
        {
            try
            {
                var DateExecution = DateTime.Now.ToString("dd/MM/yyyy");
                List<Backups> AllBackupsDateExecutionToday = DAL_Backup.SelectAllDateExecutionToday(DateExecution);
                foreach (var backups in AllBackupsDateExecutionToday)
                {
                    try
                    {
                        backups.Size = await BLL_Backup.DoBackup(backups.OrganizationSystemPrefix, ContentRootPath, MyHelpers.UrlSaveFieBackupCloud);
                        //terminer le backup effectué
                        backups.Status = "Terminer";
                        backups.Message = $"Success Backup Organisation : {backups.NameOrganization}";
                        DAL_Backup.Update(backups.Id, backups);


                        //crée une nouvelle ligne pour faire le prochain backup
                        Backups NewBackups = new Backups()
                        {
                            IdOrganization = backups.IdOrganization,
                            OrganizationSystemPrefix = backups.OrganizationSystemPrefix,
                            NameOrganization = backups.NameOrganization,
                            IntervalJour = backups.IntervalJour,
                            Size = "0 MB",
                            Status = "Attente",
                            Message = $"Backup organisation : {backups.NameOrganization}. Il va s'executer après {backups.IntervalJour} jour de sa plannification",
                            DatePlanification = backups.DateExecution,
                            DateExecution = DateTime.Now.AddDays(backups.IntervalJour).ToString("dd/MM/yyyy")
                        };

                        DAL_Backup.Add(NewBackups);
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            backups.Status = "Erreur";
                            backups.Message = $"Erreur lors du Backup Organisation : {backups.NameOrganization} \n Erreur : {e.Message}";

                            DAL_Backup.Update(backups.Id, backups);
                        }
                        catch (Exception ex)
                        {
                        }

                    }

                }

            }
            catch (Exception e)
            {
                //envoi de notification d'erreur pour la plannification des backups
            }
        }

        public static void BackupPlanningListOrganization(ParamsBackupPlanning paramsPlanning)
        {
            foreach (var org in paramsPlanning.ListOrg)
            {
                Backups backups = new Backups()
                {
                    IdOrganization = long.Parse(org.Split(",")[0]),
                    OrganizationSystemPrefix = org.Split(",")[1],
                    NameOrganization = org.Split(",")[2],
                    IntervalJour = paramsPlanning.NbrJourInterval,
                    Message = $"Organisation : {org.Split(",")[2]}. Il va s'executer après {paramsPlanning.NbrJourInterval} jour de sa plannification",
                    Status = "attente",
                    Size = "0 MB",
                    DatePlanification = DateTime.Now.ToString("dd/MM/yyyy"),
                    DateExecution = DateTime.Now.AddDays(paramsPlanning.NbrJourInterval).ToString("dd/MM/yyyy")
                };
                DAL_Backup.Add(backups);
            }

        }

        public static async Task BackupDataAdmin(ParamsBackupOrg ParamsOrgBackup, string RootPathFolder)
        {
            Backups backups = new Backups()
            {
                IdOrganization = ParamsOrgBackup.IdOrganization,
                OrganizationSystemPrefix = ParamsOrgBackup.PrefixOrganization,
                NameOrganization = ParamsOrgBackup.NameOrganization,
                IntervalJour = 0,
                Size = "0 MB",
                Message = $"Execution de Backup terminer pour l'organisation : {ParamsOrgBackup.NameOrganization}.",
                Status = "Terminer",
                DatePlanification = DateTime.Now.ToString("dd/MM/yyyy"),
            };

            try
            {
                backups.Size = await DoBackup(ParamsOrgBackup.PrefixOrganization, RootPathFolder, MyHelpers.UrlSaveFieBackupCloud);

                backups.DateExecution = DateTime.Now.ToString("dd/MM/yyyy");
                DAL_Backup.Add(backups);

            }
            catch (Exception e)
            {
                backups.Message = $"Erreur lors du backup de l'organisation : {backups.NameOrganization}. Erreur : {e.Message}.";
                backups.Status = "Erreur";
                backups.DateExecution = DateTime.Now.ToString("dd/MM/yyyy");
                DAL_Backup.Add(backups);
                throw e;
            }
        }
    }
}
