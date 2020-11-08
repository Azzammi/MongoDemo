using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
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
}