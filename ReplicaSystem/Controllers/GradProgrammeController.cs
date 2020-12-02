using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReplicaSystemMongoDB.Controllers
{
    public class GradProgrammeController : Controller
    {
        // GET: GradProgramme
        public ActionResult Index()
        {
            Models.MongoHelper.ConnectToMongoService();
            Models.MongoHelper.grad_collection =
                Models.MongoHelper.database.GetCollection<Models.GradProgramme>("gradprogrammes_tb");
            var filter = Builders<Models.GradProgramme>.Filter.Ne("Id", "");
            var result = Models.MongoHelper.grad_collection.Find(filter).ToList();

            return View(result);
        }

        // GET: GradProgramme/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GradProgramme/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GradProgramme/Create
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

        // GET: GradProgramme/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: GradProgramme/Edit/5
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

        // GET: GradProgramme/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GradProgramme/Delete/5
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
