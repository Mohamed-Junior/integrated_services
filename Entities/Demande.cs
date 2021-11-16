using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DSSGBOAdmin.Models.Entities
{
    public class Demande
    {
        public Demande()
        {

        }
        public Demande(long IdDemande,
            string NameDemande, string AffiliationDemande, string FieldOfActivityDemande,
            string AdressDemande, string PostalCodeDemande, string CityDemande,
            string CountryDemande, string EmailDemande,
            string PhoneDemande, string PersonToContactDemande, string ContactMailDemande,
            string ContactPhoneDemande, string ContactPositionDemande,
            string RegDemandDateDemande, string RegDemandDecisionDemande,
            string RegDemandDecisionDateDemande, string RegDecisionCommentsDemande)
        {
            Id = IdDemande;
            Name = NameDemande;
            Affiliation = AffiliationDemande;
            FieldOfActivity = FieldOfActivityDemande;
            Adress = AdressDemande;
            PostalCode = PostalCodeDemande;
            City = CityDemande;
            Country = CountryDemande;
            Email = EmailDemande;
            Phone = PhoneDemande;
            PersonToContact = PersonToContactDemande;
            ContactMail = ContactMailDemande;
            ContactPhone = ContactPhoneDemande;
            ContactPosition = ContactPositionDemande;
            RegDemandDate = RegDemandDateDemande;
            RegDemandDecision = RegDemandDecisionDemande;
            RegDemandDecisionDate = RegDemandDecisionDateDemande;
            RegDecisionComments = RegDecisionCommentsDemande;

        }
        public Demande(Demande mDemande)
        {
            if (mDemande != null)
            {

                Id = mDemande.Id;
                Name = mDemande.Name;
                Affiliation = mDemande.Affiliation;
                FieldOfActivity = mDemande.FieldOfActivity;
                Adress = mDemande.Adress;
                PostalCode = mDemande.PostalCode;
                City = mDemande.City;
                Country = mDemande.Country;
                Email = mDemande.Email;
                Phone = mDemande.Phone;
                PersonToContact = mDemande.PersonToContact;
                ContactMail = mDemande.ContactMail;
                ContactPhone = mDemande.ContactPhone;
                ContactPosition = mDemande.ContactPosition;
                RegDemandDate = mDemande.RegDemandDate;
                RegDemandDecision = mDemande.RegDemandDecision;
                RegDemandDecisionDate = mDemande.RegDemandDecisionDate;
                RegDecisionComments = mDemande.RegDecisionComments;

            }
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public string Affiliation { get; set; }

        public string FieldOfActivity { get; set; }

        public string Adress { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string PersonToContact { get; set; }

        public string ContactMail { get; set; }

        public string ContactPhone { get; set; }

        public string ContactPosition { get; set; }

        public string RegDemandDate { get; set; }

        public string RegDemandDecision { get; set; }

        public string RegDemandDecisionDate { get; set; }

        public string RegDecisionComments { get; set; }
        
        public string StatusActivationEmail { get; set; }
        
        public string Token { get; set; }

    }
}
