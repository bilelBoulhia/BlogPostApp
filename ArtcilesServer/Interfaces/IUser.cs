﻿using ArtcilesServer.DTO;
using ArtcilesServer.Models;

namespace ArtcilesServer.Repos
{
    public interface IUser
    {
        Task<ICollection<User>> SearchAsync(string searchQuery);
        Task<ICollection<FollowerDTO>> GetUserFollowing(int userId);

        Task<User> login(LoginDTO loginCreditianls);

    }
}
