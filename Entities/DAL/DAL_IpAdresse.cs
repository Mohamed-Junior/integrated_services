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
    public class DAL_IpAdresse
    {
        // insert ipAdresse
        public static long AddIpAdresse(IpAdresse ipAdresse)
        {
            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                var StrSQL = " INSERT INTO [IpAdresse] (IdOrganization,IpValue) output INSERTED.Id " +
                                " VALUES(@IdOrganization,@IpValue)";
                var command = new SqlCommand(StrSQL, con);
                command.Parameters.AddWithValue("@IdOrganization", ipAdresse.IdOrganization);
                command.Parameters.AddWithValue("@IpValue", ipAdresse.IpValue);
                return Convert.ToInt64(DataBaseAccessUtilities.ScalarRequest(command));
            }

        }

        // update ipAdresse
        public static void UpdateIpAdresse(long id, IpAdresse ipAdresse)
        {

            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                var StrSQL = " UPDATE [IpAdresse] SET IdOrganization=@IdOrganization, IpValue=@IpValue WHERE Id = @CurId";
                var command = new SqlCommand(StrSQL, con);
                command.Parameters.AddWithValue("@CurId", id);
                command.Parameters.AddWithValue("@IdOrganization", ipAdresse.IdOrganization);
                command.Parameters.AddWithValue("@IpValue", ipAdresse.IpValue);
                DataBaseAccessUtilities.NonQueryRequest(command);
            }
        }

        // delete ipAdresse
        public static void DeleteIpAdresse(long id)
        {
            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                var StrSQL = "DELETE FROM [IpAdresse] WHERE Id=@CurId";
                var command = new SqlCommand(StrSQL, con);
                command.Parameters.AddWithValue("@CurId", id);
                DataBaseAccessUtilities.NonQueryRequest(command);
            }
        }

        // select one record of table ipAdresse
        public static IpAdresse SelectIpAdresseById(long id)
        {
            IpAdresse ipAdresse = new IpAdresse();

            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                try
                {
                    connection.Open();
                    var StrSQL = " SELECT o.NameFr as NameOrg, ip.Id, ip.IdOrganization, ip.IpValue FROM " +
                                  " [IpAdresse] ip, Organization o " +
                                  " WHERE o.Id = ip.IdOrganization AND ip.Id = @CurId";

                    var command = new SqlCommand(StrSQL, connection);
                    command.Parameters.AddWithValue("@CurId", id);
                    SqlDataReader dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        ipAdresse.Id = dataReader.GetInt64("Id");
                        ipAdresse.IdOrganization = dataReader.GetInt64("IdOrganization");
                        ipAdresse.IpValue = dataReader["IpValue"].ToString();
                        ipAdresse.NameOrg = dataReader["NameOrg"].ToString();
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
                return ipAdresse;
            }
        }

        // select all record of table ipAdresse
        public static List<IpAdresse> SelectIpAdresseByPrefixOrg(long IdOrganization)
        {
            IpAdresse ipAdresse;
            List<IpAdresse> IpAdresses = new List<IpAdresse>();
            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                try
                {
                    connection.Open();
                    var StrSQL = " SELECT mip.IdOrganization, o.NameFr as NameOrg , mip.IpValue , mip.Id " +
                                    " FROM [IpAdresse] mip, Organization o " +
                                    " WHERE o.Id = mip.IdOrganization AND mip.IdOrganization = @IdOrganization";
                    var command = new SqlCommand(StrSQL, connection);
                    command.Parameters.AddWithValue("@IdOrganization", IdOrganization);
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        ipAdresse = new IpAdresse();
                        ipAdresse.Id = dataReader.GetInt64("Id");
                        ipAdresse.IdOrganization = dataReader.GetInt64("IdOrganization");
                        ipAdresse.IpValue = dataReader["IpValue"].ToString();
                        ipAdresse.NameOrg = dataReader["NameOrg"].ToString();
                        IpAdresses.Add(ipAdresse);
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
                return IpAdresses;
            }
        }

        // select all record of table ipAdresse
        public static int CountIpAdresseValidationByOrganization(long IdOrganization, string IPValue)
        {
            int CountIpAdresses = 0;
            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                var StrSQL = "SELECT Count(IpValue) FROM IpAdresse where IdOrganization = @IdOrganization AND IpValue = @IpValue";
                var command = new SqlCommand(StrSQL, connection);
                command.Parameters.AddWithValue("@IdOrganization", IdOrganization);
                command.Parameters.AddWithValue("@IpValue", IPValue);
                CountIpAdresses = (int)DataBaseAccessUtilities.ScalarRequest(command);
            }
            return CountIpAdresses;
        }

        // select all record of table ipAdresse
        public static List<IpAdresse> SelectAllIpAdresse()
        {
            IpAdresse ipAdresse;
            List<IpAdresse> IpAdresses = new List<IpAdresse>();
            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                try
                {
                    connection.Open();
                    var StrSQL = " SELECT mip.IdOrganization, MAX(o.NameFr) as NameOrg , MAX(mip.Id) as Id" +
                                     " FROM [IpAdresse] mip, Organization o" +
                                     " WHERE o.Id = mip.IdOrganization" +
                                     " group by mip.IdOrganization";
                    var command = new SqlCommand(StrSQL, connection);
                    var dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        ipAdresse = new IpAdresse();
                        ipAdresse.Id = dataReader.GetInt64("Id");
                        ipAdresse.IdOrganization = dataReader.GetInt64("IdOrganization");
                        //ipAdresse.IpValue = dataReader["IpValue"].ToString();
                        ipAdresse.NameOrg = dataReader["NameOrg"].ToString();
                        IpAdresses.Add(ipAdresse);
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
                return IpAdresses;
            }
        }

    }
}
