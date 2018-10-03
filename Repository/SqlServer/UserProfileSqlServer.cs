using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleBot.Repository.SqlServer
{
    [BsonIgnoreExtraElements]
    public class UserProfileSqlServer
    {
        public int Id { get; set; }

        public int Visitas { get; set; }
    }
}