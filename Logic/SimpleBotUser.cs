using MongoDB.Bson;
using MongoDB.Driver;
using SimpleBot.Config;
using SimpleBot.Repository.SqlServer;
using System;

namespace SimpleBot.Logic
{
    public class SimpleBotUser
    {
        // usar o construtor para definir o objeto corretamente
        public static IUserProfileRepository _userProfileRepo = new Repo(blablabla..);

        public static string Reply(Message message)
        {
            InserirMensagemMongoDb(message);

            // usar o objeto _userProfileRepo
            var profile = _userProfileRepo.GetProfile(message.Id);            
            SetProfile(profile);
            profile = _userProfileRepo.GetProfile(message.Id);

            return $"{message.User} disse '{message.Text}' e mandou {profile.QtdMensagens} mensagens.";
        }


        public static UserProfile GetProfile(string id)
        {
            try
            {
                var cliente = new MongoClient(MongoDbConfiguration.Conexao);
                var db = cliente.GetDatabase(MongoDbConfiguration.Banco);
                var col = db.GetCollection<BsonDocument>(MongoDbConfiguration.TabelaUsuario);

                var filtro = Builders<BsonDocument>.Filter.Eq("id", id);
                var bson = col.Find(filtro).FirstOrDefault();

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
            catch (Exception)
            {
                throw;
            }
        }

        public static void SetProfile(UserProfile profile)
        {
            try
            {
                var cliente = new MongoClient(MongoDbConfiguration.Conexao);
                var db = cliente.GetDatabase(MongoDbConfiguration.Banco);
                var col = db.GetCollection<BsonDocument>(MongoDbConfiguration.TabelaUsuario);

                var filtro = Builders<BsonDocument>.Filter.Eq("id", profile.Id);
                var bson = col.Find(filtro).FirstOrDefault();

                if (bson == null)
                {
                    col.InsertOne(new BsonDocument { { "id", profile.Id }, { "mensagens", 1 } });
                }
                else
                {
                    bson["mensagens"] = profile.QtdMensagens + 1;
                    col.ReplaceOne(filtro, bson);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void InserirMensagemMongoDb(Message message)
        {
            try
            {
                var client = new MongoClient(MongoDbConfiguration.Conexao);
                var db = client.GetDatabase(MongoDbConfiguration.Banco);
                var col = db.GetCollection<BsonDocument>(MongoDbConfiguration.TabelaMensagem);

                var bson = new BsonDocument()
                {
                    { "id" , message.Id  },
                    { "user" , message.User  },
                    { "text" , message.Text  }
                };

                col.InsertOne(bson);                
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
