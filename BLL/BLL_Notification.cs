using DSSGBOAdmin.Models.DAL;
using DSSGBOAdmin.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSSGBOAdmin.Models.BLL
{

    public class BLL_Notification
    {
        
        public static long Add(Notification notification)
        {
            notification.Status = 0;
            notification.Date = DateTime.Now.ToShortDateString();

            return DAL_Notification.Add(notification);

        }
        public static void Update(long id, int NewStatus)
        {
            DAL_Notification.Update(id, NewStatus);
        }
        public static void Delete(long id)
        {
            DAL_Notification.Delete(id);
        }
        public static Notification SelectById(long Id)
        {
            Notification mNotification = DAL_Notification.SelectById(Id);
            if (mNotification != null && mNotification.Id == 0)
            {
                throw new Exception("Cette notification n'existe pas dans la base de données");
            }
            try
            {
                //mettre la notification comme deja vu
                BLL_Notification.Update(Id, 1);
            }
            catch (Exception e) { }

            return mNotification;
        }
        public static List<Notification> SelectAll(bool IsDashboard)
        {
            return DAL_Notification.SelectAll(IsDashboard);
        }
    }
}
