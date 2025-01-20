using ArtcilesServer.Data;
using ArtcilesServer.DTO;
using ArtcilesServer.Interfaces;
using ArtcilesServer.Models;
using ArtcilesServer.Repos;
using ArtcilesServer.Services;
using Microsoft.AspNetCore.Http.HttpResults;
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
       .Where(u => u.UserId == userId)
       .SelectMany(u => u.Follwers)
       .Select(f => new FollowerDTO
       {
           UserId = f.UserId,
           UserName = f.UserName,
           UserFamilyName = f.UserFamilyName,
       })
       .ToListAsync();
        }

        public async Task<ICollection<HobbyDTO>> GetUserHobbies(int userId)
        {
            return await _context.Users
       .Where(u => u.UserId == userId)
       .SelectMany(u => u.Hobbies)
       .Select(f => new HobbyDTO
       {
           HobbyId = f.HobbyId,
           HobbyName = f.HobbyName
       })
       .ToListAsync();
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

        public async Task followUser(FollowDTO followDTO)
        {
            var userWhoWillBeFollowed = _context.Users.Where(u => u.UserId == followDTO.UserId).FirstOrDefault();
            var userWhoWantsToFollow = _context.Users.Where(u => u.UserId == followDTO.followerId).FirstOrDefault();
            userWhoWillBeFollowed?.Follwers.Add(userWhoWantsToFollow);
          
            await SaveAsync();
        }

        public async Task AddHobbies(List<Hobby> hobbies, int userId)
        {
         
            var user = await _context.Users
                .Include(u => u.Hobbies) 
                .FirstOrDefaultAsync(u => u.UserId == userId);

            foreach (var hobby in hobbies)
            {
              
                var existingHobby = await _context.Hobbies.FirstOrDefaultAsync(h => h.HobbyId == hobby.HobbyId);
                if (!user.Hobbies.Any(h => h.HobbyId == hobby.HobbyId))
                {

                    user.Hobbies.Add(existingHobby);
                }
            }

            await _context.SaveChangesAsync();
        }



        public async Task removeFollower(FollowDTO followDTO)
        {
            var userWhoWillBeFollowed = _context.Users.Where(u => u.UserId == followDTO.UserId).FirstOrDefault();
            var userWhoWantsToFollow = _context.Users.Where(u => u.UserId == followDTO.followerId).FirstOrDefault();
            userWhoWillBeFollowed?.Follwers.Remove(userWhoWantsToFollow);

            await SaveAsync();

        }

        public async Task<bool> RemindUserToAddProfilePicture(int userId)
        {
            var hasProfilePicture = await _context.Users.Where(u => u.UserId == userId).Select(u => u.UserImage).FirstOrDefaultAsync();

            return !hasProfilePicture.Equals("''");
        }
    }
}

