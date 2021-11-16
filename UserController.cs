using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSSGBOAdmin.Models.BLL;
using DSSGBOAdmin.Models.Entities;
using DSSGBOAdmin.Utilities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DSSGBOAdmin.Controllers
{
    public enum WebsiteLanguage
    {
        Fr = 0,
        Ar = 1
    }
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IWebHostEnvironment webHostingEnvironment;

        public UserController(IWebHostEnvironment environment)
        {
            webHostingEnvironment = environment;
        }
        [Route("Organization")]
        [HttpGet]
        public IActionResult GetAllUers(long IdOrganization)
        {
            try
            {
                List<User> users = BLL_User.SelectAll(IdOrganization);
                if (users != null && users.Count > 0)
                    return Json(new { success = true, message = "Utlisateurs trouves", data = users });
                else
                    return Json(new { success = true, message = "Pas des utlisateurs pour cette organisation", data = users });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erreur serveur: " + ex.Message });
            }
        }

        // GET api/<UserController>/5
        [HttpGet]
        [Route("{iduser:long}")]
        public IActionResult Get(long iduser)
        {
            try
            {
                User user = BLL_User.SelectById(iduser);
                if (user != null && user.Id > 0)
                {
                    return Json(new { success = true, message = "Utlisateur trouve", data = user });
                }
                else
                {
                    return Json(new { success = true, message = "Utilisateur introuvable.", data = user });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erreur serveur: " + ex.Message });
            }
        }

        // POST api/<UserController>
        [Route("")]
        [HttpPost]
        public JsonResult Post([FromForm] User user, long UserRequestId,string UserRequestName, string PrefixOrg)
        {
            try
            {
                //UserRequestId = 5;
                //PrefixOrg = "GBOTest";
                bool IsAdminRequest = false;
                if (PrefixOrg.Equals(MyHelpers.IdentifiantAdminRequest))
                    IsAdminRequest = true;
                BLL_User.Add(user, IsAdminRequest, UserRequestId, UserRequestName, PrefixOrg, webHostingEnvironment.ContentRootPath);
                return Json(new { success = true, message = "Utilisateur ajouté avec success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erreur serveur: " + ex.Message });
            }
        }

        // PUT api/<UserController>/5
        [Route("{id:long}")]
        [HttpPut]
        public JsonResult Put(long id, [FromForm] User user, long UserRequestId, string UserRequestName, string PrefixOrg)
        {
            try
            {
                bool IsAdminRequest = false;
                if (PrefixOrg.Equals(MyHelpers.IdentifiantAdminRequest))
                    IsAdminRequest = true;
                BLL_User.Update(id, user, IsAdminRequest, UserRequestId, UserRequestName, PrefixOrg, webHostingEnvironment.ContentRootPath);
                return Json(new { success = true, message = "Utilisateur modifié avec succès." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erreur serveur: " + ex.Message });
            }
        }

        [Route("{id}")]
        [HttpDelete]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        //[ProducesDefaultResponseType]
        public JsonResult Delete(long id, long UserRequestId, string UserRequestName, string PrefixOrg)
        {
            try
            {
                bool IsAdminRequest = false;
                if (PrefixOrg.Equals(MyHelpers.IdentifiantAdminRequest))
                    IsAdminRequest = true;
                //Response.Headers.Append("Access-Control-Allow-Origin", "*");
                BLL_User.Delete(id, IsAdminRequest, UserRequestId, UserRequestName, PrefixOrg, webHostingEnvironment.ContentRootPath);
                return Json(new { success = true, message = "Utilisateur supprimé avec succès" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erreur serveur: " + ex.Message });
            }
        }

        // test connexion
        [Route("TestConnexion")]
        [HttpGet]
        public KeyValuePair<List<User>, string> TestConnexion(string Name, string Password)
        {
            try
            {
                string message;
                //System.Diagnostics.Debug.WriteLine("message1=" + message);
                List<User> Users = BLL_User.TestConnexion(Name, Password, out message);
                if (Users != null && message == "Connexion réussie")
                {
                    if (Users.Count == 1)
                    {
                        //Get Adress IPV4 With Request
                        string IpRemoteAdress = BLL_IpAdresse.GetIpRequest(HttpContext.Connection.RemoteIpAddress);
                        // Verif IP oranization request.
                        bool TestPlageIPV4 = BLL_IpAdresse.ValidateIpAdresse(Users[0].IdOrganization, IpRemoteAdress);
                        if (TestPlageIPV4)
                        {
                            // Get Last Actif Contract
                            Contract CurrentContract = BLL_Contract.GetCurrentContractByOrganization(Users[0].IdOrganization);
                            // Test validit date Contract 
                            bool TestFinContract = BLL_Contract.CheckValidityContractDate(CurrentContract);
                            if (TestFinContract)
                            {
                                return new KeyValuePair<List<User>, string>(Users, message);
                            }
                            else
                            {
                                return new KeyValuePair<List<User>, string>(Users, $"Le contrat n'est pas valide pour cette organisation {CurrentContract.NameOrganization}");
                            }
                        }
                        else
                        {
                            return new KeyValuePair<List<User>, string>(Users, $"Requete refusée pour cette adresse IP {IpRemoteAdress}");
                        }
                    }
                }
                //System.Diagnostics.Debug.WriteLine("message2=" + message);
                return new KeyValuePair<List<User>, string>(Users, message);

            }
            catch (Exception ex)
            {
                return new KeyValuePair<List<User>, string>(new List<User>(), ex.Message);
            }
        }
        // rechercher compte utilisateur
        // test connexion
        [Route("Login/RechercherCompte")]
        [HttpGet]
        public JsonResult RechercherCompte(string Email)
        {
            try
            {
                string msg;
                if (ModelState.IsValid)
                {
                    List<User> Users = BLL_User.RechercherCompteUser(Email, out msg);
                    if(Users != null && Users.Count == 1)
                    {
                        //Get Adress IPV4 With Request
                        string IpRemoteAdress = BLL_IpAdresse.GetIpRequest(HttpContext.Connection.RemoteIpAddress);
                        // Verif IP oranization request.
                        long IdOrganization = Users[0].IdOrganization;
                        bool TestPlageIPV4 = BLL_IpAdresse.ValidateIpAdresse(IdOrganization, IpRemoteAdress);
                        if (!TestPlageIPV4)
                            return Json(new { success = false, message = $"Requete refusée pour cette adresse IP {IpRemoteAdress}" });
                        // Get Last Actif Contract
                        Contract CurrentContract = BLL_Contract.GetCurrentContractByOrganization(IdOrganization);
                        // Test validit date Contract 
                        bool TestFinContract = BLL_Contract.CheckValidityContractDate(CurrentContract);
                        if (!TestFinContract)
                            return Json(new { success = false, message = $"Le contrat n'est pas valide pour cette organisation {CurrentContract.NameOrganization}" });
                    }
                    else if(Users.Count> 1)
                    {
                        return Json(new { success = true, message = msg, data = Users });
                    }
                    else
                    {
                        return Json(new { success = false, message = msg});
                    }
                   
                }
                return Json(new { success = false,message = "Modèle invalide." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message});
            }
        }
        // Test IP Adress && Test Validit date Contract 
        [Route("ValiditOrganization")]
        [HttpGet]
        public JsonResult ValiditOrganization(long IdOrganization)
        {
            try
            {
                //Get Adress IPV4 With Request
                string IpRemoteAdress = BLL_IpAdresse.GetIpRequest(HttpContext.Connection.RemoteIpAddress);
                // Verif IP oranization request.
                bool TestPlageIPV4 = BLL_IpAdresse.ValidateIpAdresse(IdOrganization, IpRemoteAdress);
                if (!TestPlageIPV4)
                    return Json(new { success = false, message = $"Requete refusée pour cette adresse IP {IpRemoteAdress}" });
                // Get Last Actif Contract
                Contract CurrentContract = BLL_Contract.GetCurrentContractByOrganization(IdOrganization);
                // Test validit date Contract 
                bool TestFinContract = BLL_Contract.CheckValidityContractDate(CurrentContract);
                if (!TestFinContract)
                    return Json(new { success = false, message = $"Le contrat n'est pas valide pour cette organisation {CurrentContract.NameOrganization}" });

                return Json(new { success = true, message = "Contrat Valide"});

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        // test unicite UserName
        [Route("Validation/CreateName")]
        [AcceptVerbs("Get", "Post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateNameValidation(string name, long idOrganization, WebsiteLanguage websiteLanguage)
        {
            try
            {
                if (!BLL_User.CheckNameUnicity(name, idOrganization))
                {
                    switch (websiteLanguage)
                    {
                        case WebsiteLanguage.Ar: return new JsonResult($"إسم {name} مستعمل.");
                        default: return new JsonResult($"Le nom {name} est déjà utilisé.");
                    }
                }
                return new JsonResult(true);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        // test unicite Email
        [Route("Validation/CreateEmail")]
        [AcceptVerbs("Get", "Post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateEmailValidation(string email, long idOrganization, WebsiteLanguage websiteLanguage)
        {
            try
            {
                if (!BLL_User.CheckEmailUnicity(email, idOrganization))
                {
                    switch (websiteLanguage)
                    {
                        case WebsiteLanguage.Ar: return new JsonResult($"البريد الإلكتروني {email} مستعمل.");
                        default: return new JsonResult($"L'email {email} est déjà utilisé.");
                    }
                }
                return new JsonResult(true);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        // test l'unicite Name
        [Route("Validation/EditName")]
        [AcceptVerbs("Get", "Post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult EditNameValidation(string name, long idOrganization, long id, WebsiteLanguage websiteLanguage)
        {
            try
            {
                User currentUser = BLL_User.SelectById(id);
                if (!currentUser.Name.Equals(name) && !BLL_User.CheckNameUnicity(name, idOrganization))
                {
                    switch (websiteLanguage)
                    {
                        case WebsiteLanguage.Ar: return new JsonResult($"إسم {name} مستعمل.");
                        default: return new JsonResult($"Le nom {name} est déjà utilisé.");
                    }
                }
                return new JsonResult(true);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        // test l'unicite Email
        [Route("Validation/EditEmail")]
        [AcceptVerbs("Get", "Post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult EditEmailValidation(string email, long idOrganization, long id, WebsiteLanguage websiteLanguage)
        {
            try
            {
                User currentUser = BLL_User.SelectById(id);
                if (!currentUser.Email.Equals(email) && !BLL_User.CheckEmailUnicity(email, idOrganization))
                {
                    switch (websiteLanguage)
                    {
                        case WebsiteLanguage.Ar: return new JsonResult($"البريد الإلكتروني {email} مستعمل.");
                        default: return new JsonResult($"L'email {email} est déjà utilisé.");
                    }
                }
                return new JsonResult(true);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


    }
}
