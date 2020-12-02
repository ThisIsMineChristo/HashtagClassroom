using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;

namespace ReplicaSystemMongoDB.Models
{
    public class MongoHelper
    {
        public static IMongoClient client { get; set; }
        public static IMongoDatabase database { get; set; }

        public static string MongoConnection = "mongodb+srv://new_user_30:Replica@cluster0.1sews.mongodb.net/Opportunities_Galore?retryWrites=true&w=majority";
        public static string MongoDatabase = "Opportunities_Galore";

        public static IMongoCollection<Models.Apprenticeship> apprenticeships_collection { get; set; }
        public static IMongoCollection<Models.Bursary> bursary_collection { get; set; }
        public static IMongoCollection<Models.GradProgramme> grad_collection { get; set; }
        public static IMongoCollection<Models.Internship> internship_collection { get; set; }
        public static IMongoCollection<Models.Learnership> learnership_collection { get; set; }

        internal static void ConnectToMongoService()
        {
            try
            {
                client = new MongoClient(MongoConnection);
                database = client.GetDatabase(MongoDatabase);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}