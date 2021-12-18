using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace crud.salesorder.Models
{
    public class ClsModel
    {

    }

    public class NewSalesOrder
    {
        public SalesOrderHeader SalesOrderHeader { get; set; }
        public List<SalesOrderDetail> SalesOrderDetails { get; set; }
    }
    
    public class BizPartner
    {
        public ObjectId _id { get; set; }
        public long id { get; set; }
        public string idBizPartner { get; set; }
        public string nameBizPartner { get; set; }
        public DateTime crtOn { get; set; }
        public string crtAt { get; set; }
        public string crtBy { get; set; }    
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? uptOn { get; set; }
        public string uptAt { get; set; }
        public string uptBy { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Goods
    {
        //public ObjectId _id { get; set; }
        public long id { get; set; }
        public string itemId { get; set; }
        public string itemName { get; set; }
        public decimal? price { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime crtOn { get; set; }
        public string crtAt { get; set; }
        public string crtBy { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? uptOn { get; set; }
        public string uptAt { get; set; }
        public string uptBy { get; set; }

    }

    public class SalesOrderHeader
    {        
        public long id { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime transactionDate { get; set; }
        public int status { get; set; }
        public string transactionNumber { get; set; }
        public string idBizPartner { get; set; }
        public string nameBizPartner { get; set; }
        public string Remarks { get; set; }
        public DateTime crtOn { get; set; }
        public string crtAt { get; set; }
        public string crtBy { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? uptOn { get; set; }
        public string uptAt { get; set; }
        public string uptBy { get; set; }
        public virtual ICollection<SalesOrderDetail> SalesOrderDetails { get; set; }
    }

    public class SalesOrderDetail
    {        
        public long id { get; set; }
        public long idSalesOrderHeader { get; set; }
        public string itemId { get; set; }
        public string itemName { get; set; }
        public string tagID { get; set; }
        public int qty { get; set; }
        public int amount { get; set; }  
        public SalesOrderHeader SalerOrderHeader { get; set; }

    }

    public class AccountReceivableHeader
    {        
        public long id { get; set; }
        public string idBizPartner { get; set; }
        public string nameBizPartner { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime crtOn { get; set; }
        public string crtAt { get; set; }
        public string crtBy { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? uptOn { get; set; }
        public string uptAt { get; set; }
        public string uptBy { get; set; }
    }

    public class AccountReceivableDetail
    {        
        public string id { get; set; }
        public string idAccountReceivableHeader { get; set; }
        public int status { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime transactionDate { get; set; }
        public string transactionNumber { get; set; }
        public decimal? amount { get; set; }
        public string Remarks { get; set; }
    }

    public class Sequence
    {
        [BsonId]
        public ObjectId _id { get; set; }

        public string name { get; set; }

        public long value { get; set; }

        public long GetNextSequenceValue(string sequenceName, string dbName)
        {
            string constr = ConfigurationManager.AppSettings["connectionString"];
            var client = new MongoClient(constr);
            IMongoDatabase database = client.GetDatabase(dbName);
            var collection = database.GetCollection<Sequence>("seq");
            var filter = Builders<Sequence>.Filter.Eq(a => a.name, sequenceName);

            if (collection.Find(filter).Limit(1).SingleOrDefault() == null)
            {
                //Jika id = 0 maka return 1
                Sequence data = new Sequence();
                data.name = sequenceName;
                data.value = 1;
                collection.InsertOne(data);
                return 1;
            }
            else
            {
                //Jika id sudah ada sebelumnya maka return last id + 1
                var update = Builders<Sequence>.Update.Inc(a => a.value, 1);
                var sequence = collection.FindOneAndUpdate(filter, update);
                return sequence.value + 1;
            }
        }
    }

}