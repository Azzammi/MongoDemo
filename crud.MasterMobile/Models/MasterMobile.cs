using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace crud.MasterMobile.Models
{
    public class MasterMobileModel
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public long id { get; set; }
        public string mstMblNo { get; set; }
        public string mstMblCtrCd { get; set; }
        public string mstPosCode { get; set; }
        public bool act { get; set; }
        public int otpMode { get; set; }
        public string otp { get; set; }
        public string email { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime expired { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? shnEndDate { get; set; }
        //public string platform { get; set; }
        //public string version { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime crtOn { get; set; }
        public string crtAt { get; set; }
        public string crtBy { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? uptOn { get; set; }
        public string uptAt { get; set; }
        public string uptBy { get; set; }
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