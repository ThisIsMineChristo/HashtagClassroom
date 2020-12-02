using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;

namespace ReplicaSystemMongoDB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Models.MongoHelper.ConnectToMongoService();
            Models.MongoHelper.apprenticeships_collection =
                Models.MongoHelper.database.GetCollection<Models.Apprenticeship>("apprenticeship_tb");
            var filter = Builders<Models.Apprenticeship>.Filter.Ne("Id", "");
            var apprenticeship = Models.MongoHelper.apprenticeships_collection.Find(filter).ToList();

            ViewData["Apprenticeships"] = apprenticeship.Count;

            Models.MongoHelper.ConnectToMongoService();
            Models.MongoHelper.bursary_collection =
                Models.MongoHelper.database.GetCollection<Models.Bursary>("bursaries_tb");
            var filter2 = Builders<Models.Bursary>.Filter.Ne("Id", "");
            var bursary = Models.MongoHelper.bursary_collection.Find(filter2).ToList();

            ViewData["Bursaries"] = bursary.Count;

            Models.MongoHelper.ConnectToMongoService();
            Models.MongoHelper.grad_collection =
                Models.MongoHelper.database.GetCollection<Models.GradProgramme>("gradprogrammes_tb");
            var filter3 = Builders<Models.GradProgramme>.Filter.Ne("Id", "");
            var gradProg = Models.MongoHelper.grad_collection.Find(filter3).ToList();

            ViewData["Graduate Programmes"] = gradProg.Count;

            Models.MongoHelper.ConnectToMongoService();
            Models.MongoHelper.internship_collection =
                Models.MongoHelper.database.GetCollection<Models.Internship>("internships_tb");
            var filter4 = Builders<Models.Internship>.Filter.Ne("Id", "");
            var internship = Models.MongoHelper.internship_collection.Find(filter4).ToList();

            ViewData["Internships"] = internship.Count;

            Models.MongoHelper.ConnectToMongoService();
            Models.MongoHelper.learnership_collection =
                Models.MongoHelper.database.GetCollection<Models.Learnership>("learnerships_tb");
            var filter5 = Builders<Models.Learnership>.Filter.Ne("Id", "");
            var learnership = Models.MongoHelper.learnership_collection.Find(filter5).ToList();

            ViewData["Learnerships"] = learnership.Count;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult LandingPage()
        {
            
            return View();
        }
    }
}