using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using SimpleBot.Logic;
using SimpleBot.Repository.Interfaces;
using SimpleBot.Repository.MongoDB;
using SimpleBot.Repository.SqlServer;

namespace SimpleBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            if ( activity != null && activity.Type == ActivityTypes.Message)
            {
                await HandleActivityAsync(activity);
            }

            // HTTP 202
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        // Estabelece comunicação entre o usuário e o SimpleBotUser
        async Task HandleActivityAsync(Activity activity)
        {
            string text = activity.Text;
            string userFromId = activity.From.Id;
            string userFromName = activity.From.Name;

            var message = new Message(userFromId, userFromName, text);

            IUserProfileRepository repository = text.Contains("Mongo") ? 
                (IUserProfileRepository) new UserProfileMongoRepository() : new UserProfileSqlServerRepository();

            string response = new SimpleBotUser(repository).Reply(message);

            await ReplyUserAsync(activity, response);
        }

        // Responde mensagens usando o Bot Framework Connector
        async Task ReplyUserAsync(Activity message, string text)
        {
            var connector = new ConnectorClient(new Uri(message.ServiceUrl));
            var reply = message.CreateReply(text);

            await connector.Conversations.ReplyToActivityAsync(reply);
        }
    }
}