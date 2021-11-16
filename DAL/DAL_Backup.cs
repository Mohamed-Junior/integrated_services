using DSSGBOAdmin.Models.Entities;
using MyUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DSSGBOAdmin.Models.DAL
{
    public class DAL_Backup
    {
        public static string Backup(string FilePath, string DatabaseName)
        {
            var con = new SqlConnection();
            var message = "";
            try
            {
                using (con = DBConnection.GetAuthConnection())
                {
                    con.Open();
                    var StrSQL = $"backup database [{DatabaseName}] to disk='" + FilePath + "'";
                    using (var cmd = new SqlCommand(StrSQL, con))
                    {
                        cmd.ExecuteNonQuery();
                        message = "Base de données sauvegardée avec succès";
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return message;
        }

        // insert backups
        public static long Add(Backups backups)
        {
            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                var StrSQL = " INSERT INTO [Backups] (IdOrganization,IntervalJour,Message,DatePlanification,DateExecution,Status,Size)" +
                              " output INSERTED.Id " +
                              " VALUES(@IdOrganization,@IntervalJour,@Message,@DatePlanification,@DateExecution,@Status,@Size)";
                var command = new SqlCommand(StrSQL, con);
                command.Parameters.AddWithValue("@IdOrganization", backups.IdOrganization);
                //command.Parameters.AddWithValue("@OrganizationSystemPrefix", backups.OrganizationSystemPrefix);
                command.Parameters.AddWithValue("@IntervalJour", backups.IntervalJour);
                command.Parameters.AddWithValue("@Message", backups.Message);
                command.Parameters.AddWithValue("@DatePlanification", backups.DatePlanification);
                command.Parameters.AddWithValue("@DateExecution", backups.DateExecution ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Status", backups.Status);
                command.Parameters.AddWithValue("@Size", backups.Size);
                return Convert.ToInt64(DataBaseAccessUtilities.ScalarRequest(command));
            }

        }

        // update backups
        public static void Update(long Id, Backups backups)
        {
            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                var StrSQL = "UPDATE [Backups] set " +
                             "Message = @Message," +
                             "Size = @Size," +
                             "Status= @Status WHERE Id = @CurId";

                var command = new SqlCommand(StrSQL, con);
                command.Parameters.AddWithValue("@CurId", Id);
                command.Parameters.AddWithValue("@Message", backups.Message);
                command.Parameters.AddWithValue("@Size", backups.Size);
                command.Parameters.AddWithValue("@Status", backups.Status);
                DataBaseAccessUtilities.NonQueryRequest(command);
            }
        }

        // delete backups
        public static void Delete(long id)
        {
            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                string StrSQL = "DELETE FROM [Backups] WHERE Id=@Id";
                SqlCommand command = new SqlCommand(StrSQL, con);
                command.Parameters.AddWithValue("@Id", id);
                DataBaseAccessUtilities.NonQueryRequest(command);
            }
        }

        // select one record of table backups
        public static Backups SelectById(long id)
        {
            Backups backups = null;

            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                try
                {
                    connection.Open();
                    var StrSQL = " select o.Id as IdOrganization, o.NameFr as NameOrganization, o.OrganizationSystemPrefix," +
                                 " b.Id as IdBackup, b.IntervalJour, b.Status, b.Message, b.DatePlanification, b.DateExecution, b.Size " +
                                 " from Backups b, organization o " +
                                 " where b.Id = @Id and o.Id = b.IdOrganization " +
                                 " order by b.Id desc";
                    var command = new SqlCommand(StrSQL, connection);
                    command.Parameters.AddWithValue("@Id", id);
                    SqlDataReader dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        backups = ConvertDataReaderToBackups(dataReader);
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
                return backups;
            }
        }

        // select all record of table backups by organization
        public static List<Backups> SelectByOrganization(long IdOrg)
        {
            List<Backups> Backupss = new List<Backups>();
            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                try
                {
                    connection.Open();
                    var StrSQL = " select o.Id as IdOrganization, o.NameFr as NameOrganization, o.OrganizationSystemPrefix," +
                                 " b.Id as IdBackup, b.IntervalJour, b.Status, b.Message, b.DatePlanification, b.DateExecution, b.Size " +
                                 " from Backups b, organization o " +
                                 " where b.IdOrganization = @IdOrg and o.Id = b.IdOrganization " +
                                 " order by b.Id desc";
                    var command = new SqlCommand(StrSQL, connection);
                    command.Parameters.AddWithValue("@IdOrg", IdOrg);
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Backupss.Add(ConvertDataReaderToBackups(dataReader));
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
                return Backupss;
            }
        }

        // select all record of table backups
        public static List<Backups> SelectAll()
        {
            List<Backups> Backupss = new List<Backups>();
            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                try
                {
                    connection.Open();
                    var StrSQL = " select o.Id as IdOrganization, o.NameFr as NameOrganization, o.OrganizationSystemPrefix," +
                                 " b.Id as IdBackup, b.IntervalJour, b.Status, b.Message, b.DatePlanification, b.DateExecution, b.Size " +
                                 " from Backups b, organization o " +
                                 " where o.Id = b.IdOrganization " +
                                 " order by b.Id desc";
                    var command = new SqlCommand(StrSQL, connection);
                    var dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        Backupss.Add(ConvertDataReaderToBackups(dataReader));
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
                return Backupss;
            }
        }

        // select all record of table backups
        public static string SelectAllBackupsIndex()
        {
            string AllBackupsIndex = "";
            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                try
                {
                    connection.Open();
                    var StrSQL = " select o.Id as IdOrganization, o.NameFr as NameOrganization, o.OrganizationSystemPrefix, " +
                                " (select count(id) from Backups where IdOrganization = o.Id) as NumberOfBackup," +
                                " (select max(DateExecution) from Backups where IdOrganization = o.Id and Status != 'attente') as LastBackupsDone," +
                                " (select count(id) from Backups where IdOrganization = o.Id and Status = 'attente') as CountStatusAttent" +
                                " from Organization o" +
                                " order by NumberOfBackup desc , CountStatusAttent desc";

                    var command = new SqlCommand(StrSQL, connection);
                    var dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        AllBackupsIndex += $"{dataReader["IdOrganization"]},{dataReader["NameOrganization"]},{dataReader["OrganizationSystemPrefix"]},{dataReader["NumberOfBackup"]},{dataReader["LastBackupsDone"]},{dataReader["CountStatusAttent"]};";
                    }
                    AllBackupsIndex = AllBackupsIndex.Remove(AllBackupsIndex.Length - 1);
                }
                catch (SqlException e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    connection.Close();
                }
                return AllBackupsIndex;
            }
        }


        // select all record of table backups to be executed today
        public static List<Backups> SelectAllDateExecutionToday(string DateExecution)
        {
            List<Backups> Backups = new List<Backups>();
            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                try
                {
                    connection.Open();
                    var StrSQL = " select o.Id as IdOrganization, o.NameFr as NameOrganization, o.OrganizationSystemPrefix," +
                                 " b.Id as IdBackup, b.IntervalJour, b.Status, b.Message, b.DatePlanification, b.DateExecution, b.Size " +
                                 " from Backups b, organization o " +
                                 " where b.status = 'attente' and b.DateExecution=@DateExecution and o.Id = b.IdOrganization " +
                                 " order by b.Id desc";
                    var command = new SqlCommand(StrSQL, connection);
                    command.Parameters.AddWithValue("@DateExecution", DateExecution);
                    var dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        Backups.Add(ConvertDataReaderToBackups(dataReader));
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
                return Backups;
            }
        }

        private static Backups ConvertDataReaderToBackups(SqlDataReader dataReader)
        {
            Backups backups = new Backups();
            backups.Id = long.Parse(dataReader["IdBackup"].ToString());
            backups.IdOrganization = long.Parse(dataReader["IdOrganization"].ToString());
            backups.OrganizationSystemPrefix = dataReader["OrganizationSystemPrefix"].ToString();
            backups.NameOrganization = dataReader["NameOrganization"].ToString();
            backups.IntervalJour = int.Parse(dataReader["IntervalJour"].ToString());
            backups.Message = dataReader["Message"].ToString();
            backups.DatePlanification = dataReader.GetString("DatePlanification");
            backups.DateExecution = dataReader.GetString("DateExecution");
            backups.Size = dataReader.GetString("Size");
            backups.Status = dataReader["Status"].ToString();
            return backups;
        }
    }
}

