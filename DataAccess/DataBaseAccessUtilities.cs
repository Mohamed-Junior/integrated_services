using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace MyUtilities
{
    public static class DataBaseAccessUtilities
    {
        public static int NonQueryRequest(SqlCommand MyCommand)
        {
            try
            {
                try
                {
                    MyCommand.Connection.Open();
                }
                catch (SqlException e)
                {
                    throw new Exception(e.Message, e);
                }

                return MyCommand.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                MyCommand.Connection.Close();
            }
        }
        public static object ScalarRequest(SqlCommand MyCommand)
        {
            try
            {
                try
                {
                    MyCommand.Connection.Open();
                }
                catch (SqlException e)
                {
                    throw new Exception(e.Message, e);
                }

                return MyCommand.ExecuteScalar();
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                MyCommand.Connection.Close();
            }
        }
    }

    public class MyException : Exception
    {

        string _Level;
        string _MyExceptionTitle;
        string _MyExceptionMessage;


        public string Level
        {
            get
            {
                return this._Level;
            }
        }

        public string MyExceptionTitle
        {
            get
            {
                return this._MyExceptionTitle;
            }
        }

        public string MyExceptionMessage
        {
            get
            {
                return this._MyExceptionMessage.ToString();
            }
        }


        public MyException(string MyExceptionTitle, string MyExceptionMessage, string lev) : base(MyExceptionMessage)
        {
            this._Level = lev;
            this._MyExceptionTitle = MyExceptionTitle;
            this._MyExceptionMessage = MyExceptionMessage;
        }

        public MyException(Exception e, string MyExceptionTitle, string MyExceptionMessage, string lev) : base(e.Message)
        {
            this._Level = lev;
            this._MyExceptionTitle = MyExceptionTitle;
            this._MyExceptionMessage = MyExceptionMessage;
        }

    }
}
