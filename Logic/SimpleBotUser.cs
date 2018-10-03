
using SimpleBot.Repository.Interfaces;
using SimpleBot.Repository.MongoDB;
using SimpleBot.Repository.SqlServer;
using System;

namespace SimpleBot.Logic
{
    public class SimpleBotUser
    {        
        private IUserProfileRepository _userProfileRepo;

        public SimpleBotUser(IUserProfileRepository userProfileRepo)
        {
            _userProfileRepo = userProfileRepo;
        }

        public string Reply(Message message)
        {
            if(_userProfileRepo is UserProfileMongoRepository)
                ((UserProfileMongoRepository)_userProfileRepo).InsertMessage(message);
            
            var profile = _userProfileRepo.GetProfile(message.Id);
            if (profile is null)
            {
                profile = _userProfileRepo.InsertProfile(message.Id);
            }
            else
            {
                profile.QtdMensagens += 1;
                _userProfileRepo.SetProfile(profile);
            }            

            return $"{message.User} disse '{message.Text}' e mandou {profile.QtdMensagens} mensagens.";
        }                      
    }
}
