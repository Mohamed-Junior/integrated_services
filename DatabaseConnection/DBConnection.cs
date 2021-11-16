using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Security.Principal;
using System.Security.AccessControl;

public static class DBConnection
{
    static string DbCnnStrAuth = "Integrated Security=True;Persist Security Info=False;Initial Catalog=TestPFE;Data Source=DESKTOP-ECBDJEJ\\SQLEXPRESS";
    //static string DbCnnStrAuth = "User ID=testons;Password =123456;Initial Catalog=TestPFE;Server=172.16.234.33,50150";

    public static SqlConnection GetAuthConnection()
    {
        return new SqlConnection(DbCnnStrAuth);
    }

    // Dynamic data base GBO
    private static string DbCnnStr = "Integrated Security=SSPI;Data Source=DESKTOP-ECBDJEJ\\sqlexpress";

    public static string CreateDatabase(string dbName)
    {
        string message = null;
        string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string connectionString = DbCnnStr + ";";
        SqlConnection connection = null;
        try
        {
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                GrantAccess(appPath); //Need to assign the permission for current application to allow create database on server (if you are in domain).
                bool IsExits = CheckDatabaseExists(connection, dbName); //Check database exists in sql server.
                if (!IsExits)
                {
                    return CreateTableDB(connection, dbName);
                }
                else
                    message = "Base de données existe déjà";
            }

        }
        catch (Exception ex)
        {
            message = ex.Message;
        }
        finally
        {
            if (connection != null)
                connection.Close();
        }
        return message;
    }

    public static string CreateTableDB(SqlConnection connection, string DBName)
    {
        string message = null;
        try
        {
            var scriptfile = Path.Combine(Directory.GetCurrentDirectory(), "Utilities", "DatabaseConnection", "script.sql");
            FileInfo file = new FileInfo(scriptfile);
            string script = File.ReadAllText(file.FullName);
            script = script.Replace("NameDB", DBName);
            SqlCommand command = new SqlCommand(script, connection);
            var tab = script.Split("GO");

            command.CommandText = tab[0];
            command.ExecuteNonQuery();
            try
            {
                command.CommandText = tab[1];
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                command.CommandText = "DROP DATABASE " + DBName;
                command.ExecuteNonQuery();
                throw ex;
            }

        }
        catch (Exception ex)
        {
            message = ex.Message;
        }

        return message;
    }

    public static string DeleteDatabase(string dbName)
    {
        string DeleteDatabase;
        string message = null;
        string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string connectionString = DbCnnStr + $";Initial Catalog={dbName}";
        SqlConnection connection = new SqlConnection(connectionString);
        SqlConnection connection1 = new SqlConnection(DbCnnStr);
        GrantAccess(appPath); //Need to assign the permission for current application to allow create database on server (if you are in domain).
        bool IsExits = CheckDatabaseExists(connection, dbName); //Check database exists in sql server.
        if (IsExits)
        {
            DeleteDatabase = $"DROP DATABASE {dbName}";
            SqlCommand command = new SqlCommand(DeleteDatabase, connection1);
            try
            {
                connection1.Open();
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                connection1.Close();
            }
        }

        return message;
    }

    public static bool GrantAccess(string fullPath)
    {
        DirectoryInfo info = new DirectoryInfo(fullPath);
        WindowsIdentity self = WindowsIdentity.GetCurrent();
        DirectorySecurity ds = info.GetAccessControl();
        ds.AddAccessRule(new FileSystemAccessRule(self.Name,
        FileSystemRights.FullControl,
        InheritanceFlags.ObjectInherit |
        InheritanceFlags.ContainerInherit,
        PropagationFlags.None,
        AccessControlType.Allow));
        info.SetAccessControl(ds);
        return true;
    }

    public static bool CheckDatabaseExists(SqlConnection tmpConn, string databaseName)
    {
        string sqlCreateDBQuery;
        bool result = false;
        int resultObj = 0;
        try
        {
            sqlCreateDBQuery = $"SELECT database_id FROM sys.databases WHERE Name='{databaseName}'";
            using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, tmpConn))
            {
                //tmpConn.Open();
                SqlDataReader dataReader = sqlCmd.ExecuteReader();
                while (dataReader.Read())
                {
                    resultObj = Convert.ToInt32(dataReader["database_id"].ToString());
                }
                dataReader.Close();
                if (resultObj != 0)
                {
                    result = true;
                }

            }
        }
        catch (Exception ex)
        {
            //result = false;
        }
        //finally
        //{
        //    tmpConn.Close();
        //}
        return result;
    }
}
