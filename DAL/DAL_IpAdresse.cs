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
                var StrSQL  = " INSERT INTO [IpAdresse] (IdOrganization,IpValue) " +
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
                var StrSQL  = " UPDATE [IpAdresse] SET IpValue=@IpValue WHERE Id = @CurId";
                var command = new SqlCommand(StrSQL, con);
                command.Parameters.Add("@CurId", SqlDbType.BigInt).Value       = id;
                command.Parameters.Add("@IpValue", SqlDbType.NVarChar).Value   = ipAdresse.IpValue;
                DataBaseAccessUtilities.NonQueryRequest(command);
            }
        }

        // delete ipAdresse
        public static void DeleteIpAdresse(long id)
        {
            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                var StrSQL  = "DELETE FROM [IpAdresse] WHERE Id=@CurId";
                var command = new SqlCommand(StrSQL, con);
                command.Parameters.Add("@CurId", SqlDbType.BigInt).Value = id;
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
                    var StrSQL  = " SELECT o.NameFr as NameOrganization, o.Id as IdOrganization, ip.Id, ip.IpValue " +
                                  " FROM [IpAdresse] ip, Organization o " +
                                  " WHERE ip.Id = @Id and o.Id = ip.IdOrganization";

                    var command = new SqlCommand(StrSQL, connection);
                    command.Parameters.Add("@Id", SqlDbType.BigInt).Value = id;
                    SqlDataReader dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        ipAdresse.Id        = Convert.ToInt64(dataReader["Id"]);
                        ipAdresse.IdOrganization        = Convert.ToInt64(dataReader["IdOrganization"]);
                        ipAdresse.NameOrganization   = dataReader["NameOrganization"].ToString();
                        ipAdresse.IpValue   = dataReader["IpValue"].ToString();
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
        public static List<IpAdresse> SelectIpAdresseByOrg(long IdOrganization)
        {
            IpAdresse ipAdresse;
            List<IpAdresse> IpAdresses      = new List<IpAdresse>();
            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                try
                {
                    connection.Open();
                    var StrSQL  = " SELECT o.Id as IdOrganization, o.NameFr as NameOrganization , ip.IpValue , ip.Id " +
                                    " FROM IpAdresse ip, Organization o " +
                                    " WHERE ip.IdOrganization = @IdOrganization and ip.IdOrganization = o.Id ";
                    var command = new SqlCommand(StrSQL, connection);
                    command.Parameters.AddWithValue("@IdOrganization", IdOrganization);
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        ipAdresse           = new IpAdresse();
                        ipAdresse.Id        = Convert.ToInt64(dataReader["Id"]);
                        ipAdresse.IdOrganization        = Convert.ToInt64(dataReader["IdOrganization"]);
                        ipAdresse.NameOrganization   = dataReader["NameOrganization"].ToString();
                        ipAdresse.IpValue   = dataReader["IpValue"].ToString();
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
        public static List<string> SelectAllIpAdresseValidation(long IdOrganization)
        {

            List<string> IpAdresses         = new List<string>();
            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                try
                {
                    connection.Open();
                    var StrSQL  = " SELECt * FROM IpAdresse where IdOrganization = @IdOrganization";
                    var command = new SqlCommand(StrSQL, connection);
                    command.Parameters.AddWithValue("@IdOrganization", IdOrganization);
                    SqlDataReader dataReader = command.ExecuteReader();
                    if (dataReader != null)
                    {
                        while (dataReader.Read())
                        {
                            IpAdresses.Add(dataReader["IpValue"].ToString().Trim());
                        }
                    }
                    return IpAdresses;
                }
                catch (SqlException e)
                {
                    throw new Exception($"Erreur Base de données {e.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }

        }

        // select all record of table ipAdresse
        public static string SelectAllIpAdresseIndex()
        {
            string AllIpAdresseIndex = "";
            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                try
                {
                    connection.Open();
                    var StrSQL = " select max(o.Id) as IdOrganization, max(o.NameFr) as NameOrganization, Count(ip.Id) as NumberOfIpAdresse " +
                                 " from Organization o " +
                                 " LEFT JOIN IpAdresse ip on ip.IdOrganization = o.Id " +
                                 " group by(o.NameFr) " +
                                 " order by max(ip.Id) desc ";

                    var command    = new SqlCommand(StrSQL, connection);
                    var dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        AllIpAdresseIndex += $"{dataReader["IdOrganization"]},{dataReader["NameOrganization"]},{dataReader["NumberOfIpAdresse"]};";
                    }
                    AllIpAdresseIndex = AllIpAdresseIndex.Remove(AllIpAdresseIndex.Length - 1);
                }
                catch (SqlException e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    connection.Close();
                }
                return AllIpAdresseIndex;
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
    }
}
