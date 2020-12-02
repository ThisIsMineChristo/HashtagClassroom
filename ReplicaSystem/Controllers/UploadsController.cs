using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver.Core;
using MongoDB.Bson;
using ReplicaSystemMongoDB.App_Start;
using ReplicaSystemMongoDB.Models;
using MongoDB.Driver;
using Oracle.DataAccess.Client;

namespace ReplicaSystemMongoDB.Controllers
{

    public class UploadsController : Controller
    {
        private MongoDBContext dbContext;
        private IMongoCollection<UploadsModel> uploadsCollection;
        public string TNS = "Data Source=(DESCRIPTION =" +
        "(ADDRESS = (PROTOCOL = TCP)(HOST = desktop-bem33mc)(PORT = 1521))" +
        "(CONNECT_DATA =" +
        "(SERVER = DEDICATED)" +
        "(SERVICE_NAME = REPLICADB)));" +
        "User Id=C##REPLICA;Password=replica";

        public UploadsController()
        {
            dbContext = new MongoDBContext();
            uploadsCollection = dbContext.database.GetCollection<UploadsModel>("Uploads");

        }
        // GET: Uploads
        public ActionResult Index()
        {
            List<UploadsModel> uploads = uploadsCollection.AsQueryable().ToList();
            return View(uploads);
        }
        [HttpGet]
        public ActionResult Index(string search)
        {
            ViewBag.search = search;

            List<UploadsModel> uploads = uploadsCollection.AsQueryable<UploadsModel>().ToList();
            var fquery = from x in uploads select x;

            if (!String.IsNullOrEmpty(search))
            {
                fquery = fquery.Where(x => x.Subject_Code.Contains(search) || x.Subject_Name.Contains(search) || x.Hashtags.Contains(search));
            }
            //List<UploadsModel> uploads = uploadsCollection.AsQueryable<UploadsModel>().ToList();
            return View(fquery.ToList());
        }
        // GET: Uploads/Details/5


        // GET: Uploads/Create
        public ActionResult Create()
        {
            //var userinfo = new UploadsModel();
            //userinfo.UserId = Session["UserID"].ToString();
            List<string> subjects = new List<string>();

            OracleConnection Con = new OracleConnection(TNS);
            Con.Open();

            string sql = "SELECT SUBJECT_CODE FROM SUBJECT";


            // da = new OracleDataAdapter();
            OracleCommand cmd = new OracleCommand(sql, Con);
            // sqlComm = new SqlCommand(emailQuery, conn);
            OracleDataReader readSubject;

            readSubject = cmd.ExecuteReader();
            
            while (readSubject.Read())
            {
                subjects.Add(readSubject.GetValue(0).ToString()) ;
                
            }


            Con.Close();
            Con.Dispose();
            readSubject.Close();
            ViewBag.Subject = new SelectList(subjects, "Subjects");
            return View();
        }

        // POST: Uploads/Create
        [HttpPost]
        public ActionResult Create(UploadsModel uploads)
        {
            try
            {
                uploadsCollection.InsertOne(uploads);
                OracleConnection Con = new OracleConnection(TNS);
                Con.Open();
                var username = Session["UserID"].ToString();
                OracleCommand cmd = new OracleCommand("INSERT INTO UPLOAD(USER_ID, SUBJECT_CODE,CONTENT_DESCRIP, HASHTAGS, PRIVATE, FILE_DIR)" +
                                "VALUES(" + username + ", '" + uploads.Subject_Code + "', '" +uploads.Description + "', '" +
                                uploads.Hashtags + "','Y', '...')", Con);

                cmd.ExecuteNonQuery();
                Con.Close();
                Con.Dispose();
                
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Uploads/Edit/5
        public ActionResult Edit(string id)
        {
            var uploadId = id;
            var upload = uploadsCollection.AsQueryable().SingleOrDefault(x => x.Id == uploadId);
            //var upload = uploadsCollection.AsQueryable<UploadsModel>().SingleOrDefault(x => x.Id == id);
            return View(upload);
        }

        // POST: Uploads/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, UploadsModel uploads)
        {
            try
            {

                var filter = Builders<UploadsModel>.Filter.Eq("_id", ObjectId.Parse(id));
                //var filter = Builders<UploadsModel>.Filter.Eq("_id", uploads.Id);
                var update = Builders<UploadsModel>.Update
                    .Set("UserId", uploads.UserId)
                    .Set("Subject_Code", uploads.Subject_Code)
                    .Set("Subject_Name", uploads.Subject_Name)
                    .Set("Study_Year", uploads.Study_Year)
                    .Set("Description", uploads.Description)
                    .Set("Hashtags", uploads.Hashtags)
                    .Set("File_Upload", uploads.File_Upload);
                var result = uploadsCollection.UpdateOne(filter, update);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Uploads/Delete/5
        public ActionResult Delete(string id)
        {
            var uploadId = id;
            var upload = uploadsCollection.AsQueryable().SingleOrDefault(x => x.Id == uploadId);
            return View(upload);

        }

        // POST: Uploads/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, UploadsModel uploads)
        {
            try
            {

                uploadsCollection.DeleteOne(Builders<UploadsModel>.Filter.Eq("_id", ObjectId.Parse(id)));
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
