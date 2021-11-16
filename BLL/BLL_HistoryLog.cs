using DSSGBOAdmin.Models.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DSSGBOAdmin.Models.BLL
{
    public class BLL_HistoryLog
    {
        public static void SaveLog(long IdUserAction,string NameUserAction, string PrefixOrgAction, UserActionsHistory NewActionHistory, string WebRootPath)
        {
            // Chemin de chaque organisation au niveau de GBO.
            string PathRooteHistories = Path.Combine(WebRootPath, "Histories");
            string FullPathFile = Path.Combine(PathRooteHistories, $"HistoryLog_{PrefixOrgAction}.json");
            List<UserHistoryLog> AllHistoryOrg = GetOrgHistoryLogs(PathRooteHistories, FullPathFile);
            bool UserExistInHistory = false;

            UserHistoryLog userActionsHistory = AllHistoryOrg.Find(user => user.UserIdCurrent == IdUserAction);
            if(userActionsHistory != null)
            {
                userActionsHistory.AllActionHistories.Add(NewActionHistory);
                UserExistInHistory = true;
            }

            if (UserExistInHistory == false)
            {
                List<UserActionsHistory> NewAllActionHistoriesUser = new List<UserActionsHistory>();
                NewAllActionHistoriesUser.Add(NewActionHistory);

                AllHistoryOrg.Add(new UserHistoryLog()
                {
                    UserIdCurrent = IdUserAction,
                    UserNameCurrent = NameUserAction,
                    PrefixOrganization = PrefixOrgAction,
                    AllActionHistories = NewAllActionHistoriesUser
                });
            }

            File.WriteAllText(FullPathFile, JsonConvert.SerializeObject(AllHistoryOrg));
        }
        private static List<UserHistoryLog> GetOrgHistoryLogs(string PathRooteHistories, string PathFolderOrganization)
        {
            if (!Directory.Exists(PathRooteHistories))
            {
                Directory.CreateDirectory(PathRooteHistories);
            }
            if (File.Exists(PathFolderOrganization) == false)
                File.WriteAllText(PathFolderOrganization, "[]");

            List<UserHistoryLog> AllHistoryOrg = JsonConvert.DeserializeObject<List<UserHistoryLog>>(File.ReadAllText(PathFolderOrganization));
            if (AllHistoryOrg == null)
            {
                AllHistoryOrg = new List<UserHistoryLog>();
            }
            return AllHistoryOrg;
        }
        
    }
}
