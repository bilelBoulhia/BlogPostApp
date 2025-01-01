using ArtcilesServer.DTO;
using ArtcilesServer.Interfaces;
using ArtcilesServer.Models;
using ArtcilesServer.Repos;
using Microsoft.EntityFrameworkCore;


namespace ArtcilesServer.Repo
{

    public class UserRepo  : IUser
    {
       private readonly DbConn _context;

    

        public UserRepo(DbConn context)
        {
            _context = context;
           
        }

        public async Task<ICollection<User>> SearchAsync(string searchQuery)
        {
            return await _context.Users
                .FromSqlInterpolated($"EXECUTE searchUsers @SearchTerm = {searchQuery}")
                .ToListAsync();
        }



        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<FollowerDTO>> GetUserFollowing(int userId)
        {
            return await _context.Users
                                  .Where(follower => follower.UserId == userId)
                                  .Select(follower => new FollowerDTO
                                    {
                                        UserId = follower.UserId,
                                        UserName = follower.UserName,
                                        UserFamilyName = follower.UserFamilyName
                                    }).ToListAsync();
        }
    }
}

