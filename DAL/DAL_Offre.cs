using DSSGBOAdmin.Models.Entities;
using MyUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DSSGBOAdmin.Models.DAL
{

    public class DAL_Offre
    {
        public static long Add(Offre offre)
        {
            if (!CheckEntityUnicityName(offre.Name))
            {
                throw new Exception("Le nom de l'offre doit être unique.");
            }

            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                var StrSQL = "INSERT INTO [Offre] (Name,NbrMois,NbrUser,NbrCPU,Disk,RAM,Price) " +
                                " VALUES(@Name,@NbrMois,@NbrUser,@NbrCPU,@Disk,@RAM,@Price)";
                var command = new SqlCommand(StrSQL, con);
                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = offre.Name;
                command.Parameters.Add("@NbrMois", SqlDbType.Int).Value = offre.NbrMois;
                command.Parameters.Add("@NbrUser", SqlDbType.Int).Value = offre.NbrUser;
                command.Parameters.Add("@NbrCPU", SqlDbType.Int).Value = offre.NbrCPU;
                command.Parameters.Add("@Disk", SqlDbType.NVarChar).Value = offre.Disk;
                command.Parameters.Add("@RAM", SqlDbType.NVarChar).Value = offre.RAM;
                command.Parameters.Add("@Price", SqlDbType.Float).Value = offre.Price;
                return Convert.ToInt64(DataBaseAccessUtilities.ScalarRequest(command));
            }

        }

        public static void Update(long id, Offre offre)
        {
            var oldoffre = SelectById(id);

            if (oldoffre.Name != offre.Name)
            {
                if (!CheckEntityUnicityName(offre.Name))
                {
                    throw new Exception("Le nom de l'Offre doit être unique.");
                }
            }
            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                string StrSQL = "UPDATE [Offre] SET Name=@Name,NbrMois=@NbrMois,NbrUser=@NbrUser,NbrCPU=@NbrCPU," +
                    "Disk=@Disk,RAM=@RAM,Price=@Price WHERE Id = @CurId";
                SqlCommand command = new SqlCommand(StrSQL, con);
                command.Parameters.Add("@CurId", SqlDbType.BigInt).Value = id;
                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = offre.Name;
                command.Parameters.Add("@NbrMois", SqlDbType.Int).Value = offre.NbrMois;
                command.Parameters.Add("@NbrUser", SqlDbType.Int).Value = offre.NbrUser;
                command.Parameters.Add("@NbrCPU", SqlDbType.Int).Value = offre.NbrCPU;
                command.Parameters.Add("@Disk", SqlDbType.NVarChar).Value = offre.Disk;
                command.Parameters.Add("@RAM", SqlDbType.NVarChar).Value = offre.RAM;
                command.Parameters.Add("@Price", SqlDbType.Float).Value = offre.Price;
                DataBaseAccessUtilities.NonQueryRequest(command);
            }
        }

        public static void Delete(long id)
        {
            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                var StrSQL = "DELETE FROM [Offre] WHERE Id=" + id;
                var command = new SqlCommand(StrSQL, con);
                DataBaseAccessUtilities.NonQueryRequest(command);
            }
        }

        public static Offre SelectById(long id)
        {
            Offre offre = new Offre();

            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                try
                {
                    connection.Open();
                    var StrSQL = "SELECT * FROM [Offre] WHERE Id = @Id";
                    var command = new SqlCommand(StrSQL, connection);
                    command.Parameters.Add("@Id", SqlDbType.BigInt).Value = id;
                    SqlDataReader dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        offre = ConvertDataReaderToOffre(dataReader);
                    }
                }
                catch (SqlException e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    connection.Close();
                }
                return offre;
            }
        }

        public static List<Offre> SelectAll()
        {
            List<Offre> Offres = new List<Offre>();
            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                try
                {
                    connection.Open();
                    var StrSQL = "SELECT * FROM [Offre]";
                    var command = new SqlCommand(StrSQL, connection);
                    var dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Offres.Add(ConvertDataReaderToOffre(dataReader));
                    }
                }
                catch (SqlException e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    connection.Close();
                }
                return Offres;
            }
        }

        private static Offre ConvertDataReaderToOffre(SqlDataReader dataReader)
        {
            Offre offre = new Offre();
            offre.Id = Convert.ToInt64(dataReader["Id"]);
            offre.Name = dataReader["Name"].ToString();
            offre.NbrMois = int.Parse(dataReader["NbrMois"].ToString());
            offre.NbrUser = int.Parse(dataReader["NbrUser"].ToString());
            offre.NbrCPU = int.Parse(dataReader["NbrCPU"].ToString());
            offre.RAM = dataReader["RAM"].ToString();
            offre.Disk = dataReader["Disk"].ToString();
            offre.Price = double.Parse(dataReader["Price"].ToString());
            return offre;
        }

        // Test unicity Name
        private static bool CheckEntityUnicityName(string Name)
        {
            int ocurrencesNumber = 0;
            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                var query = "SELECT COUNT(*) FROM [Offre] WHERE Name = @Name";
                var command = new SqlCommand(query, connection);
                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;
                ocurrencesNumber = (int)DataBaseAccessUtilities.ScalarRequest(command);
            }

            if (ocurrencesNumber == 0)
                return true;
            else
                return false;
        }
        public static bool CheckNameUnicity(string Name)
        {
            return CheckEntityUnicityName(Name);
        }
    }
}
