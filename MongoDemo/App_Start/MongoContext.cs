using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MongoDB.Driver;

namespace MongoDemo.App_Start
{
    public class MongoContext
    {
        MongoClient _client;
        MongoServerAddress _server;
        public MongoDatabaseBase _database;
        public MongoContext()
        {
            var MongoDatabaseName = ConfigurationManager.AppSettings["MongoDatabaseName"];
            var MongoUsername = ConfigurationManager.AppSettings["MongoUsername"];
            var MongoPassword = ConfigurationManager.AppSettings["MongoPassword"];
            var MongoPort = ConfigurationManager.AppSettings["MongoPort"];
            var MongoHost = ConfigurationManager.AppSettings["MongoHost"];

            // Creating credentials
            var credential = MongoCredential.CreateMongoCRCredential(MongoDatabaseName, MongoUsername, MongoPassword);

            // Creating MongoClientSettings
            var settings = new MongoClientSettings
            {
                Credentials = new[] { credential },
                Server = new MongoServerAddress(MongoHost, Convert.ToInt32(MongoPort))
            };
            _client = new MongoClient(settings);
            //_database = _client.GetDatabase(MongoDatabaseName);
            
        }
    }
}