using ReplicaSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Oracle.DataAccess.Client;

namespace ReplicaSystem.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public string TNS = "Data Source=(DESCRIPTION =" +
        "(ADDRESS = (PROTOCOL = TCP)(HOST = desktop-bem33mc)(PORT = 1521))" +
        "(CONNECT_DATA =" +
        "(SERVER = DEDICATED)" +
        "(SERVICE_NAME = REPLICADB)));" +
        "User Id=C##REPLICA;Password=replica";

        [HttpGet]
        public ActionResult LoginView()
        {
            try
            {

                FormsAuthentication.SignOut();


                HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

                Session.Clear();
                System.Web.HttpContext.Current.Session.RemoveAll();

                
            }
            catch
            {
                throw;
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LoginView(LoginVM entity)
        {
            var userInfo = new LoginVM();
            
            try
            {
                //conn.Open();
                if (!ModelState.IsValid)
                    return View(entity);
                string username = entity.Email;
                string password = entity.Password;
                Boolean isLogin = comparePass(username, password);

                if (isLogin)
                {
                    SignInRemember(entity.Email, true);

                    //Set A Unique ID in session
                    //Session["UserID"] = userInfo.Email;

                    // If we got this far, something failed, redisplay form
                    return RedirectToAction("Index", "Home");
                    //return RedirectToLocal(entity.ReturnURL);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(entity);
                    //Login Fail
                    //TempData["ErrorMSG"] = "Access Denied! Wrong Credential";
                    //return View(entity);
                }
            }
            catch
            {
                throw;
            }
           
        }

        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(SignupVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    OracleConnection Con = new OracleConnection(TNS);
                    Con.Open();
                    
                     string emailQuery = "SELECT EMAIL_ADDRESS, COUNT(*) " +
                                    "FROM USER_DETAIL " +
                                    "GROUP BY EMAIL_ADDRESS " +
                                    "HAVING COUNT(*) > 0";
                   bool duplicateEmail = false;
                   
                    // da = new OracleDataAdapter();
                     OracleCommand cmd = new OracleCommand(emailQuery, Con);
                   
                    OracleDataReader readerUniqueEmail;

                    readerUniqueEmail = cmd.ExecuteReader();
                    
                    while (readerUniqueEmail.Read())
                    {
                        string val = readerUniqueEmail.GetValue(0).ToString();
                       
                        if ((val).CompareTo(model.Email) == 0)
                        {
                            duplicateEmail = true;
                        }

                    }
                    readerUniqueEmail.Close();
                    if (!duplicateEmail)
                    {
                        OracleDataAdapter da = new OracleDataAdapter();
                        cmd = new OracleCommand("INSERT INTO USER_DETAIL(USERNAME, PASSWORD,FIRST_NAME, LAST_NAME, MOBILE_NUMBER, EMAIL_ADDRESS)" +
                                        "VALUES('" + model.Username + "', '" + ComputeSha256Hash(model.Password) + "', '" + model.FName + "', '" +
                                        model.LName + " ', '" + model.CellNum + "', '" + model.Email + "')", Con);

                        cmd.ExecuteNonQuery();
                        Con.Close();
                        Con.Dispose();
                        return RedirectToAction("LoginView", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email already in use.");
                        return View(model);
                    }

                }
                catch
                {
                    throw;
                }
            }
            return View(model);
        }

        //POST: Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            try
            {
               
                FormsAuthentication.SignOut();

               
                HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

                Session.Clear();
                System.Web.HttpContext.Current.Session.RemoveAll();
                
                return RedirectToAction("LoginView", "Account");
            }
            catch
            {
                throw;
            }
        }

        private void SignInRemember(string userName, bool isPersistent = false)
        {
            // Clear any lingering authencation data
            FormsAuthentication.SignOut();

            // Write the authentication cookie
            FormsAuthentication.SetAuthCookie(userName, isPersistent);
            Session["Email"] = userName;
        }

        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private Boolean comparePass(string username, string inputPass)
        {
            OracleConnection Con = new OracleConnection(TNS);
            Con.Open();

            string sql = "SELECT USER_ID, PASSWORD FROM USER_DETAIL WHERE EMAIL_ADDRESS = '" + username + "'";
           

            // da = new OracleDataAdapter();
            OracleCommand cmd = new OracleCommand(sql, Con);
            // sqlComm = new SqlCommand(emailQuery, conn);
            OracleDataReader readPass;

            readPass = cmd.ExecuteReader();
            string dataPass = null;
            while (readPass.Read())
            {
                 dataPass = readPass.GetValue(1).ToString();
                Session["UserID"] = readPass.GetValue(0);
            }
           

            Con.Close();
            Con.Dispose();
            
            bool login;
            string inputHash = ComputeSha256Hash(inputPass);
            if (inputHash.CompareTo(dataPass) == 0)
            {
                login = true;
                
            }
            else
            {
                login = false;
            }
            readPass.Close();
            readPass.Dispose();
            return login;

        }
    }
}