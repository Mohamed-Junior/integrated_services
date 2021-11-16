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

    public class DAL_Notification
    {
        public static long Add(Notification notification)
        {
            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                var StrSQL = "INSERT INTO [Notification] (Title,Message,Status,Date) " +
                                " VALUES(@Title,@Message,@Status,@Date)";
                var command = new SqlCommand(StrSQL, con);
                command.Parameters.Add("@Title", SqlDbType.NVarChar).Value = notification.Title;
                command.Parameters.Add("@Message", SqlDbType.NVarChar).Value = notification.Message;
                command.Parameters.Add("@Status", SqlDbType.Int).Value = notification.Status;
                command.Parameters.Add("@Date", SqlDbType.NVarChar).Value = notification.Date;
                return Convert.ToInt64(DataBaseAccessUtilities.ScalarRequest(command));
            }

        }

        // update notification
        public static void Update(long id, int NewStatus)
        {
            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                var StrSQL = "UPDATE [Notification] SET Status=@Status WHERE Id = @CurId";
                var command = new SqlCommand(StrSQL, con);
                command.Parameters.Add("@CurId", SqlDbType.BigInt).Value = id;
                command.Parameters.Add("@Status", SqlDbType.Int).Value = NewStatus;
                DataBaseAccessUtilities.NonQueryRequest(command);
            }
        }
        // delete notification
        public static void Delete(long id)
        {
            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                var StrSQL = "DELETE FROM [Notification] WHERE Id=" + id;
                var command = new SqlCommand(StrSQL, con);
                DataBaseAccessUtilities.NonQueryRequest(command);
            }
        }
        // select one record of table notification
        public static Notification SelectById(long id)
        {
            Notification notification = new Notification();

            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                try
                {
                    connection.Open();
                    var StrSQL = "SELECT * FROM [Notification] WHERE Id = @Id order by id desc";
                    var command = new SqlCommand(StrSQL, connection);
                    command.Parameters.Add("@Id", SqlDbType.BigInt).Value = id;

                    SqlDataReader dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        notification.Id = Convert.ToInt64(dataReader["Id"]);
                        notification.Title = dataReader["Title"].ToString();
                        notification.Date = dataReader["Date"].ToString();
                        notification.Status = int.Parse(dataReader["Status"].ToString());
                        notification.Message = dataReader["Message"].ToString();
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
                return notification;
            }
        }
        // select all record of table notification
        public static List<Notification> SelectAll(bool IsDashboard)
        {
            List<Notification> Notifications = new List<Notification>();
            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                try
                {
                    connection.Open();
                    string StrSQL = "SELECT * FROM [Notification] ";

                    if (!IsDashboard)
                        StrSQL += " where status = 0 ";

                    StrSQL += " order by id desc ";

                    SqlCommand command = new SqlCommand(StrSQL, connection);
                    SqlDataReader dataReader = command.ExecuteReader();

                    Notification notification;
                    while (dataReader.Read())
                    {
                        notification = new Notification();
                        notification.Id = Convert.ToInt64(dataReader["Id"]);
                        notification.Title = dataReader["Title"].ToString();
                        notification.Date = dataReader["Date"].ToString();
                        notification.Status = int.Parse(dataReader["Status"].ToString());
                        notification.Message = dataReader["Message"].ToString();
                        Notifications.Add(notification);
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
                return Notifications;
            }
        }


    }
}
