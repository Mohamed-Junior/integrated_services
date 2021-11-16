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
    public class DAL_Contract
    {
        // insert contract
        public static long Add(Contract contract)
        {

            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                var StrSQL  = " INSERT INTO [Contract] (IdOrganization,NbrMois,NbrUser,Disk,DateStart,Status)" +
                              " output INSERTED.Id " +
                              " VALUES(@IdOrganization,@NbrMois,@NbrUser,@Disk,@DateStart,@Status)";
                var command = new SqlCommand(StrSQL, con);
                command.Parameters.Add("@IdOrganization", SqlDbType.BigInt).Value = contract.IdOrganization;
                command.Parameters.Add("@NbrMois", SqlDbType.Int).Value           = contract.NbrMois;
                command.Parameters.Add("@NbrUser", SqlDbType.Int).Value           = contract.NbrUser;
                command.Parameters.Add("@Disk", SqlDbType.NVarChar).Value         = contract.Disk;
                command.Parameters.Add("@DateStart", SqlDbType.NVarChar).Value    = contract.DateStart;
                //command.Parameters.Add("@DateEnd", SqlDbType.NVarChar).Value      = contract.DateEnd;
                command.Parameters.Add("@Status", SqlDbType.NVarChar).Value       = contract.Status;
                return Convert.ToInt64(DataBaseAccessUtilities.ScalarRequest(command));
            }

        }

        // update contract
        public static void Update(long Id, Contract contract)
        {

            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                var StrSQL = "UPDATE [Contract] " +
                             "SET IdOrganization = @IdOrganization," +
                             "NbrMois = @NbrMois," +
                             "NbrUser = @NbrUser," +
                             "Disk = @Disk," +
                             "DateStart = @DateStart," +
                             //"DateEnd = @DateEnd," +
                             "Status= @Status WHERE Id = @CurId";

                var command = new SqlCommand(StrSQL, con);
                command.Parameters.Add("@CurId", SqlDbType.BigInt).Value          = Id;
                command.Parameters.Add("@IdOrganization", SqlDbType.BigInt).Value = contract.IdOrganization;
                command.Parameters.Add("@NbrMois", SqlDbType.Int).Value           = contract.NbrMois;
                command.Parameters.Add("@NbrUser", SqlDbType.Int).Value           = contract.NbrUser;
                command.Parameters.Add("@Disk", SqlDbType.NVarChar).Value         = contract.Disk;
                command.Parameters.Add("@DateStart", SqlDbType.NVarChar).Value    = contract.DateStart;
                //command.Parameters.Add("@DateEnd", SqlDbType.NVarChar).Value      = contract.DateEnd;
                command.Parameters.Add("@Status", SqlDbType.NVarChar).Value       = contract.Status;
                DataBaseAccessUtilities.NonQueryRequest(command);
            }
        }

        // update status contract
        public static void UpdateStatus(long Id, string NewStatus)
        {

            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                string StrSQL = "UPDATE [Contract] SET Status= @Status WHERE Id = @CurId";

                SqlCommand command = new SqlCommand(StrSQL, con);
                command.Parameters.Add("@CurId", SqlDbType.BigInt).Value    = Id;
                command.Parameters.Add("@Status", SqlDbType.NVarChar).Value = NewStatus;
                DataBaseAccessUtilities.NonQueryRequest(command);
            }
        }

        public static void TerminerContractsByOrganization(long IdOrganization)
        {
            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                string StrSQL = "UPDATE [Contract] SET Status='terminer' WHERE IdOrganization = @OrgId";
                
                SqlCommand command = new SqlCommand(StrSQL, con);
                command.Parameters.Add("@OrgId", SqlDbType.BigInt).Value = IdOrganization;
                DataBaseAccessUtilities.NonQueryRequest(command);
            }
        }

        public static void ActiveLastContractsByOrganization(long IdOrganization)
        {
            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                string  StrSQL = "update [Contract] SET Status='en cour' WHERE ID = (SELECT MAX(ID) FROM [Contract] where IdOrganization = @OrgId)";

                SqlCommand command = new SqlCommand(StrSQL, con);
                command.Parameters.Add("@OrgId", SqlDbType.BigInt).Value = IdOrganization;
                DataBaseAccessUtilities.NonQueryRequest(command);
            }
        }

        // delete contract
        public static void Delete(long id)
        {
            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                string StrSQL = "DELETE FROM [Contract] WHERE Id=" + id;
                SqlCommand command = new SqlCommand(StrSQL, con);
                DataBaseAccessUtilities.NonQueryRequest(command);
            }
        }

        // select one record of table contract
        public static Contract SelectById(long id)
        {
            Contract contract = null;

            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                try
                {
                    connection.Open();
                    var StrSQL  = " SELECT c.Id as IdContract, c.IdOrganization,c.NbrMois,c.NbrUser,c.Disk,c.DateStart,c.Status, o.NameFr, o.AccountStatus, o.AccountType " +
                                    " FROM [Contract] c, [Organization] o " +
                                    " WHERE c.Id = @Id and c.IdOrganization = o.Id order by c.id desc";
                    var command = new SqlCommand(StrSQL, connection);
                    command.Parameters.Add("@Id", SqlDbType.BigInt).Value = id;
                    SqlDataReader dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        contract = ConvertDataReaderToContract(dataReader);
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
                return contract;
            }
        }
        
        // select one record of table contract
        public static Contract SelectActiveContratByOrganization(long idOrganization)
        {
            Contract contract = null;

            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                try
                {
                    connection.Open();
                    var StrSQL  = "SELECT top(1) c.Id as IdContract, c.IdOrganization,c.NbrMois,c.NbrUser,c.Disk,c.DateStart,c.Status, o.NameFr, o.AccountStatus, o.AccountType " +
                                 " FROM [Contract] c, [Organization] o " +
                                 " WHERE c.IdOrganization = @IdOrg and c.Status = 'en cour' order by c.id desc ";
                    var command = new SqlCommand(StrSQL, connection);
                    command.Parameters.Add("@IdOrg", SqlDbType.BigInt).Value = idOrganization;
                    SqlDataReader dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        contract = ConvertDataReaderToContract(dataReader);
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
                return contract;
            }
        }


        // select all record of table contract by organization
        public static List<Contract> SelectByOrganization(long IdOrg)
        {
            List<Contract> Contracts        = new List<Contract>();
            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                try
                {
                    connection.Open();
                    var StrSQL = "SELECT c.Id as IdContract, c.IdOrganization,c.NbrMois,c.NbrUser,c.Disk,c.DateStart,c.Status, o.NameFr , o.AccountStatus, o.AccountType " +
                                 " FROM [Contract] c, [Organization] o " +
                                 " WHERE c.IdOrganization = @Id and c.IdOrganization = o.Id order by c.id desc";
                    var command = new SqlCommand(StrSQL, connection);
                    command.Parameters.Add("@Id", SqlDbType.BigInt).Value = IdOrg;
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Contracts.Add(ConvertDataReaderToContract(dataReader));
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
                return Contracts;
            }
        }

        
        // select all record of table contract by organization
        public static Contract SelectActiveByOrganization(long IdOrg)
        {
            Contract contract = new Contract();
            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                try
                {
                    connection.Open();
                    string StrSQL = 
                        " SELECT c.Id as IdContract, c.IdOrganization,c.NbrMois,c.NbrUser,c.Disk,c.DateStart,c.Status, " +
                        "        o.NameFr , o.AccountStatus, o.AccountType " +
                        " FROM [Contract] c, [Organization] o " +
                        " WHERE c.IdOrganization = @Id and c.IdOrganization = o.Id and c.Status = 'en cour' order by c.id desc";
                    SqlCommand command = new SqlCommand(StrSQL, connection);
                    command.Parameters.Add("@Id", SqlDbType.BigInt).Value = IdOrg;
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        contract = ConvertDataReaderToContract(dataReader);
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
                return contract;
            }
        }


        // select all record of table contract
        public static List<Contract> SelectAll()
        {
            List<Contract> Contracts        = new List<Contract>();
            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                try
                {
                    connection.Open();
                    var StrSQL     = "SELECT c.Id as IdContract, c.IdOrganization,c.NbrMois,c.NbrUser,c.Disk,c.DateStart,c.Status, o.NameFr , o.AccountStatus, o.AccountType  " +
                        " FROM [Contract] c, [Organization] o " +
                        " WHERE c.IdOrganization = o.Id order by c.Status";
                    var command    = new SqlCommand(StrSQL, connection);
                    var dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        Contracts.Add(ConvertDataReaderToContract(dataReader));
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
                return Contracts;
            }
        }

        private static Contract ConvertDataReaderToContract(SqlDataReader dataReader)
        {
            Contract contract           = new Contract();
            contract.Id                 = Convert.ToInt64(dataReader["IdContract"]);
            contract.NameOrganization   = dataReader["NameFr"].ToString();
            contract.StatusOrganisation = dataReader["AccountStatus"].ToString();
            contract.TypeOrganization   = dataReader["AccountType"].ToString();
            contract.IdOrganization     = long.Parse(dataReader["IdOrganization"].ToString());
            contract.NbrMois            = int.Parse(dataReader["NbrMois"].ToString());
            contract.NbrUser            = int.Parse(dataReader["NbrUser"].ToString());
            contract.Disk               = dataReader["Disk"].ToString();
            //contract.DateEnd            = dataReader["DateEnd"].ToString();
            contract.DateStart          = dataReader["DateStart"].ToString();
            contract.Status             = dataReader["Status"].ToString();
            return contract;
        }
    }
}
