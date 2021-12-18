using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
    using crud.salesorder.Components;
    using crud.salesorder.Models;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using MongoDB.Bson;
    using MongoDB.Driver;

namespace crud.salesorder.Controllers
{
    public class MasterDataController : Controller
    {
        string constr = ConfigurationManager.AppSettings["connectionString"];
        string databaseName = ConfigurationManager.AppSettings["MongoDatabaseName"];

        // GET: MasterData
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddItem()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddItem(Goods goods)
        {
            if (ModelState.IsValid)
            {
                var Client = new MongoClient(constr);
                var DB = Client.GetDatabase(databaseName);
                var collection = DB.GetCollection<Goods>("tGoods");

                //goods._id = ObjectId.GenerateNewId();
                goods.id = new Sequence() { }.GetNextSequenceValue("tGoods", databaseName);
                goods.crtAt = GlobalFunctions.GetIPAddress();
                goods.crtBy = "User1";
                goods.crtOn = DateTime.Now;

                collection.InsertOneAsync(goods);
                return RedirectToAction("AddItem");
            }
            return View();
        }

        public ActionResult EditItem (string id)
        {
            var client = new MongoClient(constr);
            var db = client.GetDatabase(databaseName);
            var collection = db.GetCollection<Goods>("tGoods");
            var filter = Builders<Goods>.Filter.Eq("id", id);
            var result = collection.Find(filter).Single();

            return View(result);
        }

        [HttpPost]
        public ActionResult EditItem(string id, Goods goods)
        {            
            try
            {
                var client = new MongoClient(constr);
                var db = client.GetDatabase(databaseName);
                var collection = db.GetCollection<Goods>("tGoods");
                var filter = Builders<Goods>.Filter.Eq("id", id);

                goods.uptAt = GlobalFunctions.GetIPAddress();
                goods.uptBy = "User1";
                goods.uptOn = DateTime.Now;

                var set = Builders<Goods>.Update
                    .Set("itemName", goods.itemName)
                    .Set("price", goods.price)
                    .Set("uptAt", goods.uptAt)
                    .Set("uptBy", goods.uptBy)
                    .Set("uptOn", goods.uptOn);
                collection.FindOneAndUpdateAsync(filter, set);
                return RedirectToAction("IndexItem");
            }
            catch(Exception ex)
            {
                return View();
            }            
        }
        public ActionResult IndexItem()
        {
            return View();
        }
        public ActionResult ReadItem([DataSourceRequest] DataSourceRequest request)
        {
            var Client = new MongoClient(constr);
            var DB = Client.GetDatabase(databaseName);
            var collection = DB.GetCollection<Goods>("tGoods").Find(new BsonDocument()).ToList();
            return Json(collection.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateItem([DataSourceRequest] DataSourceRequest request, Goods goods)
        {
            var Client = new MongoClient(constr);
            var DB = Client.GetDatabase(databaseName);
            var collection = DB.GetCollection<Goods>("tGoods");
            var filter = Builders<Goods>.Filter.Eq("id", goods.id);
            var result = collection.Find(filter).Single();

            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DestroyItem([DataSourceRequest] DataSourceRequest request, Goods goods)
        {
            if(goods != null)
            {
                var Client = new MongoClient(constr);
                var DB = Client.GetDatabase(databaseName);
                var collection = DB.GetCollection<Goods>("tGoods");
                var filter = Builders<Goods>.Filter.Eq("id", goods.id);
                var deleteRecord = collection.DeleteOneAsync(filter);
            }
            return Json(new[] { goods }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult AddBizPartner()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddBizPartner(BizPartner bizPartner)
        {
            if (ModelState.IsValid)
            {
                var Client = new MongoClient(constr);
                var DB = Client.GetDatabase(databaseName);
                var collection = DB.GetCollection<BizPartner>("tBizPartner");

                //goods._id = ObjectId.GenerateNewId();
                bizPartner.id = new Sequence() { }.GetNextSequenceValue("tBizPartner", databaseName);
                bizPartner.crtAt = GlobalFunctions.GetIPAddress();
                bizPartner.crtBy = "User1";
                bizPartner.crtOn = DateTime.Now;

                collection.InsertOneAsync(bizPartner);
                return RedirectToAction("AddItem");
            }
            return View();
        }
        public ActionResult EditBizPartner(string id)
        {
            var client = new MongoClient(constr);
            var db = client.GetDatabase(databaseName);
            var collection = db.GetCollection<BizPartner>("tBizPartner");
            var filter = Builders<BizPartner>.Filter.Eq("id", id);
            var result = collection.Find(filter).Single();

            return View(result);
        }
        [HttpPost]
        public ActionResult EditBizPartner(string id, BizPartner bizPartner)
        {
            try
            {
                var client = new MongoClient(constr);
                var db = client.GetDatabase(databaseName);
                var collection = db.GetCollection<BizPartner>("tBizPartner");
                var filter = Builders<BizPartner>.Filter.Eq("id", id);

                bizPartner.uptAt = GlobalFunctions.GetIPAddress();
                bizPartner.uptBy = "User1";
                bizPartner.uptOn = DateTime.Now;

                var set = Builders<BizPartner>.Update
                    .Set("nameBizPartner", bizPartner.nameBizPartner)                    
                    .Set("uptAt", bizPartner.uptAt)
                    .Set("uptBy", bizPartner.uptBy)
                    .Set("uptOn", bizPartner.uptOn);
                collection.FindOneAndUpdateAsync(filter, set);
                return RedirectToAction("IndexItem");
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}