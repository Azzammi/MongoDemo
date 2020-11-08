using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using crud.MasterMobile.Components;
using crud.MasterMobile.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MongoDB.Bson;
using MongoDB.Driver;

namespace crud.MasterMobile.Controllers
{
    public class SystemController : Controller
    {
        string constr = ConfigurationManager.AppSettings["connectionString"];
        string databaseName = ConfigurationManager.AppSettings["MongoDatabaseName"];

       
        // GET: System      
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(MasterMobileModel mstrMobile)
        {
            if (ModelState.IsValid)
            {
                var Client = new MongoClient(constr);
                var DB = Client.GetDatabase(databaseName);
                var collection = DB.GetCollection<MasterMobileModel>("tMasterMobile");

                mstrMobile._id = ObjectId.GenerateNewId();
                mstrMobile.crtAt = GlobalFunctions.GetIPAddress();
                mstrMobile.crtBy = "Luthfi";
                mstrMobile.crtOn = DateTime.Now;
                mstrMobile.expired = DateTime.Now.AddHours(3);
                mstrMobile.shnEndDate = DateTime.Now.AddDays(14);

                collection.InsertOneAsync(mstrMobile);
                return RedirectToAction("index");
            }
            return View();
        }

        public ActionResult Index()
        {
            var Client = new MongoClient(constr);
            var db = Client.GetDatabase(databaseName);
            var collection = db.GetCollection<MasterMobileModel>("tMasterMobile").Find(new BsonDocument()).ToList();

            return View(collection);
        }

        public ActionResult IndexWithKendo()
        {
            return View();
        }

        public ActionResult Delete(string id)
        {
            if (ModelState.IsValid)
            {
                var client = new MongoClient(constr);
                var db = client.GetDatabase(databaseName);
                var collection = db.GetCollection<MasterMobileModel>("tMasterMobile");
                var filter = Builders<MasterMobileModel>.Filter.Eq("_id", ObjectId.Parse(id));
                var deleteRecord = collection.DeleteOneAsync(filter);
                return RedirectToAction("index");
            }
            return View();
        }
        public ActionResult Edit(string _id)
        {
            var client = new MongoClient(constr);
            var db = client.GetDatabase(databaseName);
            var collection = db.GetCollection<MasterMobileModel>("tMasterMobile");
            var filter = Builders<MasterMobileModel>.Filter.Eq("_id", ObjectId.Parse(_id));
            var result = collection.Find(filter).Single();

            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(string _id, MasterMobileModel MstrMobileDet)
        {           
            try
            {
                var client = new MongoClient(constr);
                var db = client.GetDatabase(databaseName);
                var collection = db.GetCollection<MasterMobileModel>("tMasterMobile");
                var filter = Builders<MasterMobileModel>.Filter.Eq("_id", ObjectId.Parse(_id));

                MstrMobileDet.uptAt = GlobalFunctions.GetIPAddress();
                MstrMobileDet.uptBy = "User";
                MstrMobileDet.uptOn = DateTime.Now;

                var set = Builders<MasterMobileModel>.Update                   
                    .Set("id", MstrMobileDet.id)
                    .Set("mstMblNo", MstrMobileDet.mstMblNo)
                    .Set("mstMblCtrCd", MstrMobileDet.mstMblCtrCd)
                    .Set("mstPosCode", MstrMobileDet.mstPosCode)
                    .Set("otpMode", MstrMobileDet.otpMode)
                    .Set("act", MstrMobileDet.act)
                    .Set("otp", MstrMobileDet.otp)
                    .Set("email", MstrMobileDet.email)
                    .Set("expired", MstrMobileDet.expired)
                    .Set("shnEndDate", MstrMobileDet.shnEndDate)
                    .Set("uptOn", MstrMobileDet.uptOn)
                    .Set("uptAt", MstrMobileDet.uptAt)
                    .Set("uptBy", MstrMobileDet.uptBy);
                collection.FindOneAndUpdateAsync(filter, set);
                return RedirectToAction("index");               
            }
            catch (Exception ex)
            {
                return View();
            }           
        }
        public ActionResult Select([DataSourceRequest]DataSourceRequest request)
        {
            var Client = new MongoClient(constr);
            var db = Client.GetDatabase(databaseName);
            var collection = db.GetCollection<MasterMobileModel>("tMasterMobile").Find(new BsonDocument()).ToList();
            return Json(collection.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        
    }
}