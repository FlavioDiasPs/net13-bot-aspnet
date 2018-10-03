using SimpleBot.Logic;

namespace SimpleBot.Repository.Interfaces
{
    public interface IUserProfileRepository
    {
        UserProfile GetProfile(string id);

        bool SetProfile(UserProfile profile);

        UserProfile InsertProfile(string Id);
    }
}
