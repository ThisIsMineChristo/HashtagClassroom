using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReplicaSystemMongoDB.Models
{
    public class Learnership
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("learnership_imagelink")]
        public string[] ImageLink { get; set; }
        [BsonElement("title")]
        public string[] Title { get; set; }
        [BsonElement("description")]
        public string[] Description { get; set; }
        [BsonElement("open_date")]
        public string[] OpenDate { get; set; }
    }
}