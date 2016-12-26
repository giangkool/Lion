using Lion;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lion.Data
{
    public class MongoHelper
    {
        public string MongoServer, MongoDatabase;
        public MongoDatabase _database;

        public MongoHelper(){}
        //connect mongodb server
        public MongoHelper(string _server, string _db)
        {
            MongoServer = _server;
            MongoDatabase = _db;
        }

        //get database from mongodb
        public MongoDatabase Database
        {
            get
            {
                try
                {
                    if(_database == null)
                    {
                        MongoClient client = new MongoClient(MongoServer);
                        MongoServer server = client.GetServer();
                        server.Connect();
                        _database = server.GetDatabase(MongoDatabase);
                    }
                    return _database;
                }
                catch
                {
                    return null;
                }
            }
        }

        // delete object by object Id
        public bool Delete(string objectName, dynamic _id)
        {
            FindAndRemoveArgs arg = new FindAndRemoveArgs();
            arg.Query = Query.EQ("_id", _id);
            FindAndModifyResult result = Database.GetCollection(objectName).FindAndRemove(arg);
            return result.Ok;
        }

        // get document with collection name
        private BsonDocument GetDocumnet(string collectionName, IMongoQuery query)
        {
            var doc = Database.GetCollection(collectionName).Find(query).FirstOrDefault();
            return doc;
        }

        // get object by object name
        public DynamicObj Get(string objectName, IMongoQuery query)
        {
            BsonDocument doc = GetDocumnet(objectName, query);
            if (doc == null)
                return null;
            return new DynamicObj(doc);
        }

        // get list document by collection name
        private List<BsonDocument> ListDocumnet( string collectionName, IMongoQuery query)
        {
            var doc = Database.GetCollection(collectionName).Find(query).ToList();
            return doc;
        }
        private List<BsonDocument> ListDocument(string collectionName, IMongoQuery query, IMongoSortBy sort)
        {
            var doc = Database.GetCollection(collectionName).Find(query).SetSortOrder(sort).ToList();
            return doc;
        }

        // get list by object name
        public DynamicObj[] List(string objectName, IMongoQuery query)
        {

            List<BsonDocument> list = ListDocumnet(objectName, query);
            List<DynamicObj> lstObj = new List<DynamicObj>();
            foreach (BsonDocument doc in list)
            {
                lstObj.Add(new DynamicObj(doc));
            }
            return lstObj.ToArray();
        }

        // get list by object name sort
        public DynamicObj[] List(string objectName, IMongoQuery query, IMongoSortBy sort)
        {

            List<BsonDocument> list = ListDocument(objectName, query, sort);
            List<DynamicObj> lstObj = new List<DynamicObj>();
            foreach (BsonDocument doc in list)
            {
                lstObj.Add(new DynamicObj(doc));
            }
            return lstObj.ToArray();
        }

        // save documnet by collection name
        private bool SaveDocument(string collectionName, BsonDocument document)
        {
            WriteConcernResult result = Database.GetCollection(collectionName).Save(document);
            return result.Ok;
        }

        // save dynamic by object name
        public bool SaveDynamicWithDaytime(string objectName, dynamic obj)
        {
            DateTime _date = DateTime.Now;
            dynamic dyna = obj;
            dyna.system_last_updated_time = _date.ToString("yyyMMddHHmmss");
            dyna.system_last_updated_date = _date.ToString("yyyMMdd");

            BsonDocument doc = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(obj.ToString());
            return SaveDocument(objectName, doc);
        }

        public bool SaveWithDaytime(string objectName, DynamicObj obj)
        {
            DateTime _date = DateTime.Now;
            dynamic dyna = obj;
            dyna.system_last_updated_time = _date.ToString("yyyMMddHHmmss");
            dyna.system_last_updated_date = _date.ToString("yyyMMdd");

            return SaveDocument(objectName, dyna.ToBsonDocument());
        }

        public bool Save(string objectName, DynamicObj obj)
        {
            dynamic dyna = obj;
            return SaveDocument(objectName, dyna.ToBsonDocument());
        }

        // insert documnet by collection name
        private bool InsertDocument(string collectionName, BsonDocument document)
        {
            WriteConcernResult result = Database.GetCollection(collectionName).Insert(document);
            return result.Ok;
        }

        // insert dynamic by object name
        public bool InsertDynamic(string objectName, dynamic obj)
        {
            DateTime _date = DateTime.Now;
            dynamic dyna = obj;
            dyna.system_last_updated_time = _date.ToString("yyyMMddHHmmss");
            dyna.system_last_updated_date = _date.ToString("yyyMMdd");
            dyna.system_created_time = _date.ToString("yyyMMddHHmmss");
            dyna.system_created_date = _date.ToString("yyyMMdd");
            BsonDocument doc = BsonDocument.Parse(JsonConvert.SerializeObject(dyna));
            return InsertDocument(objectName, doc);
        }

        public bool Insert(string objectName, DynamicObj obj)
        {
            DateTime _date = DateTime.Now;
            dynamic dyna = obj;
            dyna.system_last_updated_time = _date.ToString("yyyMMddHHmmss");
            dyna.system_last_updated_date = _date.ToString("yyyMMdd");
            dyna.system_created_time = _date.ToString("yyyMMddHHmmss");
            dyna.system_created_date = _date.ToString("yyyMMdd");

            return InsertDocument(objectName, dyna.ToBsonDocument());
        }

        // update object by object name
        public bool UpdateObject(string objectName, IMongoQuery query, IMongoUpdate update)
        {
            WriteConcernResult result = Database.GetCollection(objectName).Update(query, update);
            return result.Ok;
        }

        // create document
        private void CreateDocument(string collectionName, BsonDocument document, IMongoQuery query)
        {
            var old = GetDocumnet(collectionName, query);
            if (old == null)
                InsertDocument(collectionName, document);
        }

        public void Create(string objectName, DynamicObj obj, IMongoQuery query)
        {
            CreateDocument(objectName, obj.ToBsonDocument(), query);
        }

        // count object
        public long Count(string objectName, IMongoQuery query)
        {
            return Database.GetCollection(objectName).Find(query).Count();
        }
    }
}
