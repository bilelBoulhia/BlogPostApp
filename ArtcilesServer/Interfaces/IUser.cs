using ArtcilesServer.DTO;
using ArtcilesServer.Models;

namespace ArtcilesServer.Repos
{
    public interface IUser
    {
        Task<ICollection<User>> SearchAsync(string searchQuery);
        Task<ICollection<FollowerDTO>> GetUserFollowing(int userId);
        Task AddHobbies(List<Hobby> hobbies, int userId);
        Task<bool> RemindUserToAddProfilePicture(int userId);
        Task followUser(FollowDTO followDTO);
        Task removeFollower(FollowDTO followDTO);

        Task<User> login(LoginDTO loginCreditianls);

    }
}
