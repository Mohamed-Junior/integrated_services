using DSSGBOAdmin.Models.DAL;
using DSSGBOAdmin.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSSGBOAdmin.Models.BLL
{

    public class BLL_Offre
    {
        public static bool CheckNameUnicity(string Name)
        {
            return DAL_Offre.CheckNameUnicity(Name);
        }
        public static long Add(Offre offre)
        {
            return DAL_Offre.Add(offre);
        }
        public static void Update(long id, Offre offre)
        {
            DAL_Offre.Update(id, offre);
        }
        public static void Delete(long id)
        {
            DAL_Offre.Delete(id);
        }
        public static Offre SelectById(long id)
        {
            Offre mOffre = DAL_Offre.SelectById(id);
            if (mOffre != null && mOffre.Id > 0)
            {
                return mOffre;
            }
            throw new Exception("Ce offre n'existe pas dans la base de données");
        }
        public static List<Offre> SelectAll()
        {
            return DAL_Offre.SelectAll();
        }
    }
}
