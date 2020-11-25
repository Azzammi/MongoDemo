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
    public class KendoBatchGridController : Controller
    {
        string constr = ConfigurationManager.AppSettings["connectionString"];
        string databaseName = ConfigurationManager.AppSettings["MongoDatabaseName"];

        // GET: KendoBatchGrid
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
        public ActionResult Editing_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<MasterMobileModel> masterMobiles)
        {
            var results = new List<MasterMobileModel>();
            var Client = new MongoClient(constr);
            var DB = Client.GetDatabase(databaseName);

            if (masterMobiles != null && ModelState.IsValid)
            {
                foreach (var masterMobile in masterMobiles)
                {
                    var collection = DB.GetCollection<MasterMobileModel>("tMasterMobile");

                    masterMobile._id = ObjectId.GenerateNewId();
                    masterMobile.crtAt = GlobalFunctions.GetIPAddress();
                    masterMobile.crtBy = "Luthfi";
                    masterMobile.crtOn = DateTime.Now;
                    masterMobile.expired = DateTime.Now.AddHours(3);
                    masterMobile.shnEndDate = DateTime.Now.AddDays(14);

                    collection.InsertOneAsync(masterMobile);
                    results.Add(masterMobile);
                }
            }
            return Json(results.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<MasterMobileModel> masterMobiles)
        {
            var Client = new MongoClient(constr);
            var DB = Client.GetDatabase(databaseName);

            if (masterMobiles != null && ModelState.IsValid)
            {
                foreach (var masterMobile in masterMobiles)
                {
                    var collection = DB.GetCollection<MasterMobileModel>("tMasterMobile");
                    var filter = Builders<MasterMobileModel>.Filter.Eq("id", masterMobile.id);

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
            }
            return Json(masterMobiles.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<MasterMobileModel> masterMobiles)
        {
            if (masterMobiles.Any())
            {
                foreach (var masterMobile in masterMobiles)
                {
                    var client = new MongoClient(constr);
                    var db = client.GetDatabase(databaseName);
                    var collection = db.GetCollection<MasterMobileModel>("tMasterMobile");
                    var filter = Builders<MasterMobileModel>.Filter.Eq("id", masterMobile.id);
                    var deleteRecord = collection.DeleteOneAsync(filter);
                }
            }

            return Json(masterMobiles.ToDataSourceResult(request, ModelState));
        }
    }
}