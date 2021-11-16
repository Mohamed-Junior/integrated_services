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
    public class DAL_Demande
    {

        public static string CreateDatabaseIfNotExists(string OrganizationSystemPrefix)
        {
            return DBConnection.CreateDatabase(OrganizationSystemPrefix);
        }
        private static Demande ConvertDataReaderToDemande(SqlDataReader dataReader)
        {
            Demande demande = new Demande();
            demande.ID = dataReader.GetInt64("Id");
            demande.Name = dataReader["Name"].ToString().Trim();
            demande.Affiliation = dataReader["Affiliation"].ToString().Trim();
            demande.FieldOfActivity = dataReader["FieldOfActivity"].ToString().Trim();
            demande.Adress = dataReader["Adress"].ToString().Trim();
            demande.PostalCode = dataReader["PostalCode"].ToString().Trim();
            demande.City = dataReader["City"].ToString().Trim();
            demande.Country = dataReader["Country"].ToString().Trim();
            demande.Email = dataReader["Email"].ToString().Trim();
            demande.Phone = dataReader["Phone"].ToString().Trim();
            demande.PersonToContact = dataReader["PersonToContact"].ToString().Trim();
            demande.ContactMail = dataReader["ContactMail"].ToString().Trim();
            demande.ContactPhone = dataReader["ContactPhone"].ToString().Trim();
            demande.ContactPosition = dataReader["ContactPosition"].ToString().Trim();
            demande.RegDemandDate = dataReader["RegDemandDate"].ToString().Split(" ")[0];
            demande.RegDemandDecision = dataReader["RegDemandDecision"].ToString().Trim();
            demande.RegDemandDecisionDate = dataReader["RegDemandDecisionDate"].ToString().Split(" ")[0];
            demande.RegDecisionComments = dataReader["RegDecisionComments"].ToString().Trim();

            return demande;
        }

        public static Demande selectByField(string field, string valueSearch)
        {
            Demande demande = new Demande();
            long Id = 0;
            string StrSQL;
            SqlCommand command;
            if (field.Trim().ToLower().Equals("id"))
            {
                Id = long.Parse(valueSearch);
            }

            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                try
                {
                    connection.Open();
                    if (Id != 0)
                    {
                        StrSQL = "select * FROM RegistrationDemand where Id = @value";
                        command = new SqlCommand(StrSQL, connection);
                        command.Parameters.Add("@value", SqlDbType.BigInt).Value = Id;
                    }
                    else //search by antoher field
                    {
                        StrSQL = "select * FROM RegistrationDemand where " + field + " = @value";
                        command = new SqlCommand(StrSQL, connection);
                        command.Parameters.Add("@value", SqlDbType.VarChar).Value = valueSearch;
                    }
                    SqlDataReader dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        demande = ConvertDataReaderToDemande(dataReader);
                        //demande.ID = dataReader.GetInt64(0);
                        //demande.Name = dataReader["Name"].ToString().Trim();
                        //demande.Affiliation = dataReader["Affiliation"].ToString().Trim();
                        //demande.FieldOfActivity = dataReader["FieldOfActivity"].ToString().Trim();
                        //demande.Adress = dataReader["Adress"].ToString().Trim();
                        //demande.PostalCode = dataReader["PostalCode"].ToString().Trim();
                        //demande.City = dataReader["City"].ToString().Trim();
                        //demande.Country = dataReader["Country"].ToString().Trim();
                        //demande.Email = dataReader["Email"].ToString().Trim();
                        //demande.Phone = dataReader["Phone"].ToString().Trim();
                        //demande.PersonToContact = dataReader["PersonToContact"].ToString().Trim();
                        //demande.ContactMail = dataReader["ContactMail"].ToString().Trim();
                        //demande.ContactPhone = dataReader["ContactPhone"].ToString().Trim();
                        //demande.ContactPosition = dataReader["ContactPosition"].ToString().Trim();
                        //demande.RegDemandDate = dataReader["RegDemandDate"].ToString().Split(" ")[0];
                        //demande.RegDemandDecision = dataReader["RegDemandDecision"].ToString().Trim();
                        //demande.RegDemandDecisionDate = dataReader["RegDemandDecisionDate"].ToString().Split(" ")[0];
                        //demande.RegDecisionComments = dataReader["RegDecisionComments"].ToString().Trim();

                    }
                    return demande;
                }
                catch (SqlException e)
                {
                    throw new MyException(e, "Erreur Base de données", e.Message, "DAL");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public static void AddDemande(Demande newDemande)
        {
            if (selectByField("Name", newDemande.Name).ID != 0)
            {
                throw new MyException("Base De Données Erreur", "Le Nom est déja utilisé.", "DAL");
            }
            if(selectByField("Email", newDemande.Email).ID != 0)
            {
                throw new MyException("Base De Données Erreur", "L'Email est déja utilisé.", "DAL");
            }
            if (selectByField("Phone", newDemande.Phone).ID != 0)
            {
                throw new MyException("Base De Données Erreur", "Le Numéro de téléphone est déja utilisé.", "DAL");
            }
            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                string StrSQL = "insert into [RegistrationDemand]  " +
                    "([Name],[Affiliation],[FieldOfActivity],[Adress],[PostalCode],[City],[Country],[Email],[Phone],[PersonToContact],[ContactMail],[ContactPhone],[ContactPosition],[RegDemandDate],[RegDemandDecision],[RegDemandDecisionDate],[RegDecisionComments],StatusActivationEmail,Token)" +
                    " output INSERTED.Id " +
                    "values  (" +
                    "@Name,@Affiliation,@FieldOfActivity,@Adress,@PostalCode,@City,@Country,@Email,@Phone,@PersonToContact,@ContactMail,@ContactPhone,@ContactPosition,@RegDemandDate,@RegDemandDecision,@RegDemandDecisionDate,@RegDecisionComments,@StatusActivationEmail,@Token)";

                SqlCommand MySqlCommand = new SqlCommand(StrSQL, con);
                MySqlCommand.Parameters.AddWithValue("@Name", newDemande.Name);
                MySqlCommand.Parameters.AddWithValue("@Affiliation", newDemande.Affiliation ?? (object)DBNull.Value);
                MySqlCommand.Parameters.AddWithValue("@FieldOfActivity", newDemande.FieldOfActivity ?? (object)DBNull.Value);
                MySqlCommand.Parameters.AddWithValue("@Adress", newDemande.Adress ?? (object)DBNull.Value);
                MySqlCommand.Parameters.AddWithValue("@PostalCode", newDemande.PostalCode ?? (object)DBNull.Value);
                MySqlCommand.Parameters.AddWithValue("@City", newDemande.City ?? (object)DBNull.Value);
                MySqlCommand.Parameters.AddWithValue("@Country", newDemande.Country ?? (object)DBNull.Value);
                MySqlCommand.Parameters.AddWithValue("@Email", newDemande.Email ?? (object)DBNull.Value);
                MySqlCommand.Parameters.AddWithValue("@Phone", newDemande.Phone ?? (object)DBNull.Value);
                MySqlCommand.Parameters.AddWithValue("@PersonToContact", newDemande.PersonToContact ?? (object)DBNull.Value);
                MySqlCommand.Parameters.AddWithValue("@ContactMail", newDemande.ContactMail ?? (object)DBNull.Value);
                MySqlCommand.Parameters.AddWithValue("@ContactPhone", newDemande.ContactPhone ?? (object)DBNull.Value);
                MySqlCommand.Parameters.AddWithValue("@ContactPosition", newDemande.ContactPosition ?? (object)DBNull.Value);
                MySqlCommand.Parameters.AddWithValue("@RegDemandDate", newDemande.RegDemandDate ?? (object)DBNull.Value);
                MySqlCommand.Parameters.AddWithValue("@RegDemandDecision", newDemande.RegDemandDecision ?? (object)DBNull.Value);
                MySqlCommand.Parameters.AddWithValue("@RegDemandDecisionDate", newDemande.RegDemandDecisionDate ?? (object)DBNull.Value);
                MySqlCommand.Parameters.AddWithValue("@RegDecisionComments", newDemande.RegDecisionComments ?? (object)DBNull.Value);
                MySqlCommand.Parameters.AddWithValue("@StatusActivationEmail", newDemande.StatusActivationEmail ?? (object)DBNull.Value);
                MySqlCommand.Parameters.AddWithValue("@Token", newDemande.Token ?? (object)DBNull.Value);
                DataBaseAccessUtilities.NonQueryRequest(MySqlCommand); 

            }
        }

        internal static string DeleteDatabaseIfExists(string OrganizationSystemPrefix)
        {
            return DBConnection.DeleteDatabase(OrganizationSystemPrefix);
        }

        public static void UpdateDemande(long Id, Demande newDemande)
        {
            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                //string StrSQL = "update [RegistrationDemand] set " +
                //" Name = @Name , Affiliation = @Affiliation , FieldOfActivity = @FieldOfActivity , " +
                //" Adress = @Adress , PostalCode = @PostalCode , " +
                //" City = @City , Country = @Country , " +
                //" Email = @Email , Phone = @Phone , " +
                //" PersonToContact = @PersonToContact , " +
                //" ContactMail = @ContactMail , ContactPhone = @ContactPhone , ContactPosition = @ContactPosition , " +
                //" RegDemandDate = @RegDemandDate , RegDemandDecision = @RegDemandDecision , " +
                //" RegDemandDecisionDate = @RegDemandDecisionDate , RegDecisionComments = @RegDecisionComments " +
                //" where Id = @currId";
                string StrSQL = "update RegistrationDemand set" +
                " RegDemandDecision = @RegDemandDecision , " +
                " RegDemandDecisionDate = @RegDemandDecisionDate , RegDecisionComments = @RegDecisionComments " +
                " where Id = @currId";
                SqlCommand MySqlCommand = new SqlCommand(StrSQL, con);
                MySqlCommand.Parameters.Add("@currId", SqlDbType.BigInt).Value = Id;
                MySqlCommand.Parameters.Add("@RegDemandDecision", SqlDbType.NVarChar).Value = newDemande.RegDemandDecision ?? (object)DBNull.Value;
                MySqlCommand.Parameters.Add("@RegDemandDecisionDate", SqlDbType.Date).Value = newDemande.RegDemandDecisionDate == null ? (object)DBNull.Value : newDemande.RegDemandDecisionDate;
                MySqlCommand.Parameters.Add("@RegDecisionComments", SqlDbType.NVarChar).Value = newDemande.RegDecisionComments ?? (object)DBNull.Value;
                DataBaseAccessUtilities.NonQueryRequest(MySqlCommand);
            }

        }
        public static void UpdateDemandeStatusActivationEmail(string Token, string StatusActivationEmail)
        {
            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                string StrSQL = "update RegistrationDemand set" +
                " StatusActivationEmail = @StatusActivationEmail , " +
                " where Token = @Token";
                SqlCommand MySqlCommand = new SqlCommand(StrSQL, con);
                MySqlCommand.Parameters.AddWithValue("@Token", Token);
                MySqlCommand.Parameters.AddWithValue("@StatusActivationEmail", StatusActivationEmail);
               
                DataBaseAccessUtilities.NonQueryRequest(MySqlCommand);
            }

        }

        public static void UpdateDemandeToken(string NewToken, string OldToken)
        {
            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                string StrSQL = "update RegistrationDemand set" +
                " Token = @NewToken , " +
                " where Token = @OldToken";
                SqlCommand MySqlCommand = new SqlCommand(StrSQL, con);
                MySqlCommand.Parameters.AddWithValue("@NewToken", NewToken);
                MySqlCommand.Parameters.AddWithValue("@OldToken", OldToken);

                DataBaseAccessUtilities.NonQueryRequest(MySqlCommand);
            }

        }
        public static void DeleteDemande(long Id)
        {
            using (SqlConnection con = DBConnection.GetAuthConnection())
            {

                SqlCommand MySqlCommand = new SqlCommand("delete from [RegistrationDemand] where Id = @Id", con);
                MySqlCommand.Parameters.Add("@Id", SqlDbType.BigInt).Value = Id;
                DataBaseAccessUtilities.NonQueryRequest(MySqlCommand);

            }

        }
        public static List<Demande> selectAll()
        {
            List<Demande> demandes = new List<Demande>();
            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                try
                {
                    connection.Open();
                    string StrSQL = "SELECt * FROM RegistrationDemand order by Id desc";
                    SqlCommand command = new SqlCommand(StrSQL, connection);
                    SqlDataReader dataReader = command.ExecuteReader();
                    if (dataReader != null)
                    {
                        while (dataReader.Read())
                        {
                            demandes.Add(ConvertDataReaderToDemande(dataReader));
                        }
                    }
                    return demandes;
                }
                catch (SqlException e)
                {
                    System.Diagnostics.Debug.WriteLine("e.Message=" + e.Message);
                    throw new MyException(e, "Erreur Base de données", e.Message, "DAL");
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static KeyValuePair<string,string> SelectByToken(string Token)
        {
            KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>();
            using (SqlConnection connection = DBConnection.GetAuthConnection())
            {
                try
                {
                    connection.Open();
                    string StrSQL = "select Token,RegDemandDate from RegistrationDemand where Token = @Token";
                    SqlCommand command = new SqlCommand(StrSQL, connection);
                    command.Parameters.AddWithValue("@Token", Token);
                    SqlDataReader dataReader = command.ExecuteReader();
                    while(dataReader.Read())
                    {
                        keyValuePair = new KeyValuePair<string, string>(dataReader["Token"].ToString(), dataReader["RegDemandDate"].ToString());
                    }
                }
                catch (SqlException e)
                {
                    throw new MyException(e, "Erreur Base de données", e.Message, "DAL");
                }
                finally
                {
                    connection.Close();
                }
                return keyValuePair;
            }
        }
    }
}
