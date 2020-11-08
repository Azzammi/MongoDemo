using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDemo.Models;

namespace MongoDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(EmployeeDetails Emp)
        {
            if (ModelState.IsValid)
            {
                string constr = ConfigurationManager.AppSettings["connectionString"];
                var Client = new MongoClient(constr);
                var DB = Client.GetDatabase("Employee");
                var collection = DB.GetCollection<EmployeeDetails>("EmployeeDetails");

                Emp.Id = ObjectId.GenerateNewId();
                collection.InsertOneAsync(Emp);
                return RedirectToAction("emplist");
            }
            return View();
        }

        public ActionResult emplist()
        {
            string constr = ConfigurationManager.AppSettings["connectionString"];
            var Client = new MongoClient(constr);
            var db = Client.GetDatabase("Employee");
            var collection = db.GetCollection<EmployeeDetails>("EmployeeDetails").Find(new BsonDocument()).ToList();

            return View(collection);
        }

        public ActionResult Delete(string id)
        {
            if (ModelState.IsValid) {
                string constr = ConfigurationManager.AppSettings["connectionString"];
                var Client = new MongoClient(constr);
                var DB = Client.GetDatabase("Employee");
                var collection = DB.GetCollection<EmployeeDetails>("EmployeeDetails");
                var filter = Builders<EmployeeDetails>.Filter.Eq("_id", ObjectId.Parse(id));
                var DeleteRecord = collection.DeleteOneAsync(filter);
                return RedirectToAction("emplist");
            }
            return View();
        }
        public ActionResult Edit(string id)
        {
                
                string constr = ConfigurationManager.AppSettings["connectionString"];
                var Client = new MongoClient(constr);
                var DB = Client.GetDatabase("Employee");
                var collection = DB.GetCollection<EmployeeDetails>("EmployeeDetails");
                var filter = Builders<EmployeeDetails>.Filter.Eq("_id", ObjectId.Parse(id));
                var result = collection.Find(filter).Single();

                //var update = collection.FindOneAndUpdateAsync(Builders<EmployeeDetails>.Filter.Eq("Id", EmpDet.Id), Builders<EmployeeDetails>.Update.Set("Name", EmpDet.Name).Set("Department", EmpDet.Department).Set("Address", EmpDet.Address).Set("City", EmpDet.City).Set("Country", EmpDet.Country));
                return View(result);
            
        }
        public ActionResult Update(EmployeeDetails EmpDet)
        {
            if (ModelState.IsValid)
            {
                string constr = ConfigurationManager.AppSettings["connectionString"];
                var Client = new MongoClient(constr);
                var DB = Client.GetDatabase("Employee");
                var collection = DB.GetCollection<EmployeeDetails>("EmployeeDetails");
                var update = collection.FindOneAndUpdateAsync(Builders<EmployeeDetails>.Filter.Eq("_id", EmpDet.Id), Builders<EmployeeDetails>.Update.Set("Name", EmpDet.Name).Set("Department", EmpDet.Department).Set("Address", EmpDet.Address).Set("City", EmpDet.City).Set("Country", EmpDet.Country));
                return RedirectToAction("emplist");
            }
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
    }
}