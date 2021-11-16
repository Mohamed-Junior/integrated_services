﻿using DSSGBOAdmin.Models.Entities;
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
            demande.Id = dataReader.GetInt64("Id");
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
                        //demande.Id = dataReader.GetInt64(0);
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
            if (selectByField("Name", newDemande.Name).Id != 0 || selectByField("Email", newDemande.Email).Id != 0)
            {
                throw new MyException("Base De Données Erreur", "Demande doit etre unique.", "DAL");
            }
            using (SqlConnection con = DBConnection.GetAuthConnection())
            {
                string StrSQL = "insert into [RegistrationDemand]  " +
                    "([Name],[Affiliation],[FieldOfActivity],[Adress],[PostalCode],[City],[Country],[Email],[Phone],[PersonToContact],[ContactMail],[ContactPhone],[ContactPosition],[RegDemandDate],[RegDemandDecision],[RegDemandDecisionDate],[RegDecisionComments])" +
                    " output INSERTED.Id " +
                    "values  (" +
                    "@Name,@Affiliation,@FieldOfActivity,@Adress,@PostalCode,@City,@Country,@Email,@Phone,@PersonToContact,@ContactMail,@ContactPhone,@ContactPosition,@RegDemandDate,@RegDemandDecision,@RegDemandDecisionDate,@RegDecisionComments)";

                SqlCommand MySqlCommand = new SqlCommand(StrSQL, con);
                MySqlCommand.Parameters.Add("@Name", SqlDbType.NVarChar).Value = newDemande.Name;
                MySqlCommand.Parameters.Add("@Affiliation", SqlDbType.NVarChar).Value = newDemande.Affiliation ?? (object)DBNull.Value;
                MySqlCommand.Parameters.Add("@FieldOfActivity", SqlDbType.NVarChar).Value = newDemande.FieldOfActivity ?? (object)DBNull.Value;
                MySqlCommand.Parameters.Add("@Adress", SqlDbType.NVarChar).Value = newDemande.Adress ?? (object)DBNull.Value;
                MySqlCommand.Parameters.Add("@PostalCode", SqlDbType.NVarChar).Value = newDemande.PostalCode ?? (object)DBNull.Value;
                MySqlCommand.Parameters.Add("@City", SqlDbType.NVarChar).Value = newDemande.City ?? (object)DBNull.Value;
                MySqlCommand.Parameters.Add("@Country", SqlDbType.NVarChar).Value = newDemande.Country ?? (object)DBNull.Value;
                MySqlCommand.Parameters.Add("@Email", SqlDbType.NVarChar).Value = newDemande.Email ?? (object)DBNull.Value;
                MySqlCommand.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = newDemande.Phone ?? (object)DBNull.Value;
                MySqlCommand.Parameters.Add("@PersonToContact", SqlDbType.NVarChar).Value = newDemande.PersonToContact ?? (object)DBNull.Value;
                MySqlCommand.Parameters.Add("@ContactMail", SqlDbType.NVarChar).Value = newDemande.ContactMail ?? (object)DBNull.Value;
                MySqlCommand.Parameters.Add("@ContactPhone", SqlDbType.NVarChar).Value = newDemande.ContactPhone ?? (object)DBNull.Value;
                MySqlCommand.Parameters.Add("@ContactPosition", SqlDbType.NVarChar).Value = newDemande.ContactPosition ?? (object)DBNull.Value;
                MySqlCommand.Parameters.Add("@RegDemandDate", SqlDbType.Date).Value = newDemande.RegDemandDate ?? (object)DBNull.Value;
                MySqlCommand.Parameters.Add("@RegDemandDecision", SqlDbType.NVarChar).Value = newDemande.RegDemandDecision ?? (object)DBNull.Value;
                MySqlCommand.Parameters.Add("@RegDemandDecisionDate", SqlDbType.Date).Value = newDemande.RegDemandDecisionDate == null ? (object)DBNull.Value : newDemande.RegDemandDecisionDate;
                MySqlCommand.Parameters.Add("@RegDecisionComments", SqlDbType.NVarChar).Value = newDemande.RegDecisionComments ?? (object)DBNull.Value;
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
                    string StrSQL = "SELECt * FROM RegistrationDemand order by id desc";
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
    }
}
