using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReplicaSystemMongoDB.Controllers
{
    public class LearnershipController : Controller
    {
        // GET: Learnership
        public ActionResult Index()
        {
            Models.MongoHelper.ConnectToMongoService();
            Models.MongoHelper.learnership_collection =
                Models.MongoHelper.database.GetCollection<Models.Learnership>("learnerships_tb");
            var filter = Builders<Models.Learnership>.Filter.Ne("Id", "");
            var result = Models.MongoHelper.learnership_collection.Find(filter).ToList();

            return View(result);
        }

        // GET: Learnership/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Learnership/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Learnership/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Learnership/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Learnership/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Learnership/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Learnership/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
