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
    public class TransactionController : Controller
    {
        string constr = ConfigurationManager.AppSettings["connectionString"];
        string databaseName = ConfigurationManager.AppSettings["MongoDatabaseName"];

        // GET: Transaction
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddSalesOrder()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddSalesOrder(SalesOrderHeader salesOrderHeader, List<SalesOrderDetail> details)
        {
            var Client = new MongoClient(constr);
            var DB = Client.GetDatabase(databaseName);
            var collection = DB.GetCollection<SalesOrderHeader>("tSalerOrderHeader");

            salesOrderHeader.id = new Sequence() { }.GetNextSequenceValue("tSalesOrderHeader", databaseName);
            salesOrderHeader.status = 0; //Draft
            salesOrderHeader.crtAt = GlobalFunctions.GetIPAddress();
            salesOrderHeader.crtBy = "User1";
            salesOrderHeader.crtOn = DateTime.Now;            

            foreach (var detail in details)
            {
                var collectionDetail = DB.GetCollection<SalesOrderDetail>("tSalesOrderDetail");
                detail.idSalesOrderHeader = salesOrderHeader.id;
                detail.id = new Sequence() { }.GetNextSequenceValue("tSalerOrderDetail", databaseName);

                collectionDetail.InsertOneAsync(detail);
            }
            collection.InsertOneAsync(salesOrderHeader);

            return View();
        }      

        public ActionResult AddSalesOrderDetail(int? i)
        {
            ViewBag.i = i;
            return PartialView();
        }

        public ActionResult SalesOrder()
        {
            return View();
        }
        public ActionResult ReadSalesOrder([DataSourceRequest] DataSourceRequest request)
        {
            var Client = new MongoClient(constr);
            var db = Client.GetDatabase(databaseName);
            var collection = db.GetCollection<SalesOrderHeader>("tSalerOrderHeader").Find(new BsonDocument()).ToList();
            return Json(collection.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadSalesOrderDetail([DataSourceRequest] DataSourceRequest request)
        {
            var Client = new MongoClient(constr);
            var db = Client.GetDatabase(databaseName);
            var collection = db.GetCollection<SalesOrderDetail>("tSalesOrderDetail").Find(new BsonDocument()).ToList();
            return Json(collection.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
    }
}