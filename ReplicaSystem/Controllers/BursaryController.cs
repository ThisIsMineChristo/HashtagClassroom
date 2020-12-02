using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReplicaSystemMongoDB.Controllers
{
    public class BursaryController : Controller
    {
        // GET: Bursary
        public ActionResult Index()
        {
            Models.MongoHelper.ConnectToMongoService();
            Models.MongoHelper.bursary_collection =
                Models.MongoHelper.database.GetCollection<Models.Bursary>("bursaries_tb");
            var filter = Builders<Models.Bursary>.Filter.Ne("Id", "");
            var result = Models.MongoHelper.bursary_collection.Find(filter).ToList();

            return View(result);
        }

        // GET: Bursary/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Bursary/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Bursary/Create
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

        // GET: Bursary/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Bursary/Edit/5
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

        // GET: Bursary/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Bursary/Delete/5
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
