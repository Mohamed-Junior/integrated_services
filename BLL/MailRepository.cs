using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
namespace DSSGBOAdmin.Models.BLL
{
    public class MailRepository
    {
        private readonly string mailServer, login, password;
        private readonly int port;
        private readonly bool ssl;
        public MailRepository(string mailServer, int port, bool ssl, string login, string password)
        {
            this.mailServer = mailServer;
            this.port = port;
            this.ssl = ssl;
            this.login = login;
            this.password = password;
        }
        public string SendEmailBureauOrdre(string SenderEmail, string UserName, string Password)
        {
            string msg = "";
            string message = $"<h3>Données d'authentification:<br/> Nom d'utilisateur = <b>{UserName}</b> <br/> Mot de passe = <b>{Password}</b>.</h3>";
            message += "<p><br/></p><div style='color:black !important;'><p>**************************************************************<br/><b>Ce mail est envoyé par l'administrateur de société Doc Stream solutions.</b></p></div>";
            try
            {
                using (var client = new SmtpClient())
                {
                    client.Connect(mailServer, port, ssl);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(login, password);
                    var email = new MimeMessage();
                    email.From.Add(MailboxAddress.Parse(login));
                    email.To.Add(MailboxAddress.Parse(SenderEmail));
                    email.Subject = "Authentification au bureau d'ordre";
                    email.Body = new TextPart(TextFormat.Html) { Text = message };
                    client.Send(email);
                    msg = "Email envoyé avec succès.";
                    client.Disconnect(true);
                }


            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }
        public string SendEmailConfirmation(string SenderEmail,string Name, string Url)
        {
            string Result = "";
            string body = "<br/><br/>Nous sommes ravis de vous annoncer que votre compte est" +
     " créé avec succès. Veuillez cliquer sur le lien ci-dessous pour vérifier votre compte" +
     " <br/><br/><a href='" + Url + "'>" + Url + "</a> ";
            try
            {
                using (var client = new SmtpClient())
                {
                    client.Connect(mailServer, port, ssl);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(login, password);
                    var email = new MimeMessage();
                    email.From.Add(new MailboxAddress("Super Admin Hirundo", login));
                    email.To.Add(new MailboxAddress(Name, SenderEmail));
                    email.Subject = "Le compte a éte créé avec succès";
                    email.Body = new TextPart(TextFormat.Html) { Text = body };
                    client.Send(email);
                    Result = $"Succès! L'inscription a été effectuée et le lien d'activation du compte a été envoyé à votre email={SenderEmail}";
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                Result = ex.Message;
            }
            return Result;
        }
    }
}
