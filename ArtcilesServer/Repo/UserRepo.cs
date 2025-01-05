using ArtcilesServer.DTO;
using ArtcilesServer.Interfaces;
using ArtcilesServer.Models;
using ArtcilesServer.Repos;
using ArtcilesServer.Services;
using Microsoft.EntityFrameworkCore;


namespace ArtcilesServer.Repo
{

    public class UserRepo  : IUser
    {
       private readonly DbConn _context;

        private readonly HashPassword _hashpassowrd;

    

        public UserRepo(DbConn context,HashPassword hashPassword)
        {
            _context = context;
            _hashpassowrd  = hashPassword;
           
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

        public async Task<User> login(LoginDTO loginCredentials)
        {
           
            var user = await _context.Users
                .FirstOrDefaultAsync(u =>u.UserEmail == loginCredentials.UserEmail);

            
            if (user == null || _hashpassowrd.GenerateHash(loginCredentials.UserHash, user.UserSalt) != user.UserHash)
            {
                throw new ArgumentException("Error logging in check your email or password");
            }

            return user;
        }

    }
}

