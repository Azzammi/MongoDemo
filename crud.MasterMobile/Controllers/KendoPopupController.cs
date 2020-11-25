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
    public class KendoPopupController : Controller
    {
        string constr = ConfigurationManager.AppSettings["connectionString"];
        string databaseName = ConfigurationManager.AppSettings["MongoDatabaseName"];

        // GET: KendoPopup
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Select([DataSourceRequest] DataSourceRequest request)
        {
            var Client = new MongoClient(constr);
            var db = Client.GetDatabase(databaseName);
            var collection = db.GetCollection<MasterMobileModel>("tMasterMobile").Find(new BsonDocument()).ToList();
            return Json(collection.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        /* Grid multiple editing */
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Create([DataSourceRequest] DataSourceRequest request, MasterMobileModel masterMobile)
        {            
            var Client = new MongoClient(constr);
            var DB = Client.GetDatabase(databaseName);

            if (masterMobile != null && ModelState.IsValid)
            {                
                    var collection = DB.GetCollection<MasterMobileModel>("tMasterMobile");

                    masterMobile._id = ObjectId.GenerateNewId();
                    masterMobile.id  = new Sequence() { }.GetNextSequenceValue("tMasterMobile", databaseName);
                    masterMobile.crtAt = GlobalFunctions.GetIPAddress();
                    masterMobile.crtBy = "Luthfi";
                    masterMobile.crtOn = DateTime.Now;
                    masterMobile.expired = DateTime.Now.AddHours(3);
                    masterMobile.shnEndDate = DateTime.Now.AddDays(14);

                    collection.InsertOneAsync(masterMobile);                              
            }
            return Json(new[] { masterMobile }.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Update([DataSourceRequest] DataSourceRequest request, MasterMobileModel masterMobile)
        {
            var Client = new MongoClient(constr);
            var DB = Client.GetDatabase(databaseName);

            if (masterMobile != null && ModelState.IsValid)
            {
                
                    var collection = DB.GetCollection<MasterMobileModel>("tMasterMobile");
                    var filter = Builders<MasterMobileModel>.Filter.Eq("_id", masterMobile._id);

                    masterMobile.uptAt = GlobalFunctions.GetIPAddress();
                    masterMobile.uptBy = "User";
                    masterMobile.uptOn = DateTime.Now;

                    var set = Builders<MasterMobileModel>.Update
                        .Set("id", masterMobile.id)
                        .Set("mstMblNo", masterMobile.mstMblNo)
                        .Set("mstMblCtrCd", masterMobile.mstMblCtrCd)
                        .Set("mstPosCode", masterMobile.mstPosCode)
                        .Set("otpMode", masterMobile.otpMode)
                        .Set("act", masterMobile.act)
                        .Set("otp", masterMobile.otp)
                        .Set("email", masterMobile.email)
                        .Set("expired", masterMobile.expired)
                        .Set("shnEndDate", masterMobile.shnEndDate)
                        .Set("uptOn", masterMobile.uptOn)
                        .Set("uptAt", masterMobile.uptAt)
                        .Set("uptBy", masterMobile.uptBy);
                    collection.FindOneAndUpdateAsync(filter, set);                
            }
            return Json(new[] { masterMobile }.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Destroy([DataSourceRequest] DataSourceRequest request, MasterMobileModel masterMobile)
        {
            if (masterMobile != null)
            {                
                    var client = new MongoClient(constr);
                    var db = client.GetDatabase(databaseName);
                    var collection = db.GetCollection<MasterMobileModel>("tMasterMobile");
                    var filter = Builders<MasterMobileModel>.Filter.Eq("id", masterMobile.id);
                    var deleteRecord = collection.DeleteOneAsync(filter);                
            }

            return Json(new[] { masterMobile }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Edit(string id)
        {
            var client = new MongoClient(constr);
            var db = client.GetDatabase(databaseName);
            var collection = db.GetCollection<MasterMobileModel>("tMasterMobile");
            var filter = Builders<MasterMobileModel>.Filter.Eq("id", id);
            var result = collection.Find(filter).Single();

            return View("~/Views/System/Edit.cshtml",result);
        }
        [HttpPost]
        public ActionResult Edit(string id, MasterMobileModel MstrMobileDet)
        {
            try
            {
                var client = new MongoClient(constr);
                var db = client.GetDatabase(databaseName);
                var collection = db.GetCollection<MasterMobileModel>("tMasterMobile");
                var filter = Builders<MasterMobileModel>.Filter.Eq("id", id);

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
    }
}