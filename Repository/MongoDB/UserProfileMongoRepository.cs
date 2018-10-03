using SimpleBot.Repository.Interfaces;
using SimpleBot.Logic;
using System;

using MongoDB.Bson;
using MongoDB.Driver;
using SimpleBot.Config;

namespace SimpleBot.Repository.MongoDB
{
    public class UserProfileMongoRepository : IUserProfileRepository
    {
        private MongoClient _client;
        private IMongoDatabase _db;
        private IMongoCollection<BsonDocument> UserProfileCollection { get; set; }

        public UserProfileMongoRepository()
        {
            _client = new MongoClient(MongoDbConfiguration.Conexao);
            _db = _client.GetDatabase(MongoDbConfiguration.Banco);
            UserProfileCollection = _db.GetCollection<BsonDocument>(MongoDbConfiguration.TabelaUsuario);
        }

        public UserProfile GetProfile(string id)
        {
            try
            {                
                var filtro = Builders<BsonDocument>.Filter.Eq("id", id);
                var bson = UserProfileCollection.Find(filtro).FirstOrDefault();

                if (bson == null)
                    return new UserProfile { Id = id, QtdMensagens = 1 };
                else
                {
                    return new UserProfile
                    {
                        Id = bson["id"].ToString(),
                        QtdMensagens = bson["mensagens"].ToInt32()
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SetProfile(UserProfile profile)
        {
            try
            {     
                var filtro = Builders<BsonDocument>.Filter.Eq("id", profile.Id);
                var bson = UserProfileCollection.Find(filtro).FirstOrDefault();

                if (bson != null)
                {
                    bson["mensagens"] = profile.QtdMensagens;
                    UserProfileCollection.ReplaceOne(filtro, bson);
                    return true;
                }
                else
                    return false;                
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public UserProfile InsertProfile(string Id)
        {
            UserProfile profile = null;
            try
            {
                UserProfileCollection.InsertOne(new BsonDocument { { "id", Id }, { "mensagens", 1 } });
                profile = new UserProfile()
                {
                    Id = Id,
                    QtdMensagens = 1
                };

                return profile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertMessage(Message message)
        {
            try
            {              
                var bson = new BsonDocument()
                {
                    { "id" , message.Id  },
                    { "user" , message.User  },
                    { "text" , message.Text  }
                };

                UserProfileCollection.InsertOne(bson);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
