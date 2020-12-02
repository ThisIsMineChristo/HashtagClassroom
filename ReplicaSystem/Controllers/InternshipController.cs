using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReplicaSystemMongoDB.Controllers
{
    public class InternshipController : Controller
    {
        // GET: Internship
        public ActionResult Index()
        {
            Models.MongoHelper.ConnectToMongoService();
            Models.MongoHelper.internship_collection =
                Models.MongoHelper.database.GetCollection<Models.Internship>("internships_tb");
            var filter = Builders<Models.Internship>.Filter.Ne("Id", "");
            var result = Models.MongoHelper.internship_collection.Find(filter).ToList();

            return View(result);
        }

        // GET: Internship/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Internship/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Internship/Create
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

        // GET: Internship/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Internship/Edit/5
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

        // GET: Internship/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Internship/Delete/5
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
