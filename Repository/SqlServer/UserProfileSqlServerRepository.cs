using Dapper;
using SimpleBot.Config;
using SimpleBot.Logic;
using SimpleBot.Repository.Interfaces;
using System.Data.SqlClient;

namespace SimpleBot.Repository.SqlServer
{
    public class UserProfileSqlServerRepository : IUserProfileRepository
    {
        public UserProfile GetProfile(string id)
        {
            UserProfile user = null;
            string sql = "SELECT * FROM UserProfile WHERE Id = @id";
            var param = new { id };
            
            using (var connection = new SqlConnection(SQLServerConfiguration.SimpleBotConnectionString))                            
                user = connection.QueryFirstOrDefault<UserProfile>(sql, param: param);
                        
            return user;
        }
        public bool SetProfile(UserProfile profile)
        {
            if (profile is null) return false;

            long affectedRows = 0;
            string sql = $"UPDATE UserProfile SET QtdMensagens = @QtdMensagens WHERE Id = @Id";
            var param = new { profile.Id, profile.QtdMensagens };
            
            using (var connection = new SqlConnection(SQLServerConfiguration.SimpleBotConnectionString))
                affectedRows = connection.Execute(sql, param: param);

            if (affectedRows > 0) return true;
            else return false;
        }
        public UserProfile InsertProfile(string Id)
        {
            int QtdMensagens = 1;            
            string sql = $"INSERT INTO UserProfile VALUES (@Id, @QtdMensagens)";
            var param = new { Id, QtdMensagens };
            UserProfile user = null;

            using (var connection = new SqlConnection(SQLServerConfiguration.SimpleBotConnectionString))
                user = connection.ExecuteScalar<UserProfile>(sql, param: param);
            
            return user;
        }       
    }
}
