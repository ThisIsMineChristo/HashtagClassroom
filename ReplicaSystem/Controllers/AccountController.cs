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
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LoginView(LoginVM entity)
        {
            var userInfo = new LoginVM();

            SqlConnection conn;
             conn = new SqlConnection();

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
                    Session["UserID"] = userInfo.Email;

                    // If we got this far, something failed, redisplay form
                    return RedirectToAction("ManageEmployee", "Account");
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
                    OracleDataAdapter da = new OracleDataAdapter();
                    OracleCommand cmd = new OracleCommand("INSERT INTO USER_DETAIL(USERNAME, PASSWORD,FIRST_NAME, LAST_NAME, MOBILE_NUMBER, EMAIL_ADDRESS)" +
                                    "VALUES('@Username', '@Password', '@FName', ' @LNAME', '@CellNum', '@Email')", Con);
                    cmd.Parameters.Add("@Username", model.Username);
                    cmd.Parameters.Add("@Password", ComputeSha256Hash(model.Username));
                    cmd.Parameters.Add("@FName", model.FName);
                    cmd.Parameters.Add("@LName", model.LName);
                    cmd.Parameters.Add("@CellNum", model.CellNum);
                    cmd.Parameters.Add("@Email", model.Email);
                    cmd.ExecuteNonQuery();
                    Con.Close();
                    Con.Dispose();
                    
                   /* string nextIDQuery = "SELECT MAX( EmployeeNumber) FROM Employee";
                    SqlCommand sqlComm = new SqlCommand(nextIDQuery, conn);
                    SqlDataReader readerNewID;

                    readerNewID = sqlComm.ExecuteReader();

                    int nextID = 0;
                    while (readerNewID.Read())
                    {
                        //dataPass = String.Format("{0}", reader[0]);
                        nextID = (int)readerNewID.GetValue(0) + 1;
                    }
                    conn.Close();
                    readerNewID.Close();*/
                    string emailQuery = "SELECT Email, COUNT(*) " +
                                    "FROM Employee " +
                                    "GROUP BY Email " +
                                    "HAVING COUNT(*) > 1";
                   bool duplicateEmail = false;
                    // conn.Open();
                   /* sqlComm = new SqlCommand(emailQuery, conn);
                    SqlDataReader readerUniqueEmail;

                   // readerUniqueEmail = sqlComm.ExecuteReader();
                    
                    while (readerUniqueEmail.Read())
                    {
                        string val = readerUniqueEmail.GetValue(0).ToString();
                        //dataPass = String.Format("{0}", reader[0]);
                        if ((val).CompareTo(model.Email) == 0)
                        {
                            duplicateEmail = true;
                        }

                    }
                    readerUniqueEmail.Close();*/
                    if (!duplicateEmail)
                    {/*
                        string insertQuery = "INSERT INTO Employee (Email, Password, EmployeeNumber, Age,Gender, DistanceFromHome, MaritalStatus, NumberCompaniesWorked,TotalWorkingYears, Over18)" +
                        " VALUES(@Email,@Password, @EmployeeNumber, @Age, @Gender, @DistanceFromHome, @MaritalStatus, @NumberCompaniesWorked, @TotalWorkingYears, @Over18)";
                        sqlComm = new SqlCommand(insertQuery, conn);
                        sqlComm.Parameters.AddWithValue("@Email", model.Email);
                        sqlComm.Parameters.AddWithValue("@Password", ComputeSha256Hash(model.Password));
                        sqlComm.Parameters.AddWithValue("@EmployeeNumber", nextID);
                        sqlComm.Parameters.AddWithValue("@Age", model.Age);
                        sqlComm.Parameters.AddWithValue("@Gender", model.EmpGender);
                        sqlComm.Parameters.AddWithValue("@DistanceFromHome", model.HomeDistance);
                        sqlComm.Parameters.AddWithValue("@MaritalStatus", model.MaritalStatus);
                        sqlComm.Parameters.AddWithValue("@NumberCompaniesWorked", model.NoCompWorked);
                        sqlComm.Parameters.AddWithValue("@TotalWorkingYears", model.TotalWorkY);
                        sqlComm.Parameters.AddWithValue("@Over18", (((int)model.Age) >= 18));


                        sqlComm.ExecuteNonQuery();
                        insertQuery = "INSERT INTO EmployeeDetails (EmployeeNumber) Values(@EmployeeNumber)";
                        sqlComm = new SqlCommand(insertQuery, conn);
                        sqlComm.Parameters.AddWithValue("@EmployeeNumber", nextID);
                        sqlComm.ExecuteNonQuery();
                        conn.Close();*/
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
                
                return RedirectToAction("Index", "Home");
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

            SqlConnection conn = new SqlConnection(TNS);
            string sql = "SELECT Password FROM Employee WHERE Email ='" + username + "'";
            conn.Open();
            SqlCommand command = new SqlCommand(sql, conn);
            //datas = new DataSet();

            string dataPass = null;
            SqlDataReader readerCompare;
            command = new SqlCommand(sql, conn);
            readerCompare = command.ExecuteReader();
            while (readerCompare.Read())
            {
                //dataPass = String.Format("{0}", reader[0]);
                dataPass = readerCompare.GetValue(0).ToString();
            }

            conn.Close();
            readerCompare.Close();
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
            return login;

        }
    }
}