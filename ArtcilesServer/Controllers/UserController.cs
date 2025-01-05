using ArtcilesServer.DTO;
using ArtcilesServer.Interfaces;
using ArtcilesServer.Models;
using ArtcilesServer.Repo;
using ArtcilesServer.Services;
using ArticlesServer.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtcilesServer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {




        private readonly IMapper _mapper;
        private readonly UserRepo _userRepo;
        private readonly GenericRepository<User> _userAction;
        private readonly HashPassword _hashPassword;
        private readonly AuthService _authService;
        public UserController(IMapper mapper,AuthService authService ,GenericRepository<User> userActions,UserRepo userRepo,HashPassword hashPassword)
        {

            _hashPassword = hashPassword;
            _userRepo = userRepo;
            _mapper = mapper;
            _userAction = userActions;
            _authService = authService;

        }


        #region Post actions
        [AllowAnonymous]
        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccount([FromBody] UserDTO user)
        {
            try
            {
                var User = _mapper.Map<User>(user);

           
                var salt = _hashPassword.GenerateSalt();
                var hashedPassword = _hashPassword.GenerateHash(user.UserHash, salt);

               
                User.UserHash = hashedPassword;
                User.UserSalt = salt; 

   
                await _userAction.AddAsync(User);

                return Ok("User account has been created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred : {ex.Message}");
            }
        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            try
            {
                Console.WriteLine("hey");
                
                 var user = await  _userRepo.login(login);


                if (user == null)
                    return Unauthorized();

                var token = _authService.GenerateToken(user);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred : {ex.Message}");
            }
        }








        #endregion
        #region Get actions

        [Authorize]
        [HttpGet("CheckAuth")]
        public async Task<IActionResult> CheckAuth()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value });
            return Ok(new
            {
                Message = "Auth working!",
                Username = User.Identity.Name,
                Claims = claims
            });
        }


        [HttpGet("GetAllAccounts")]
        public async Task<IActionResult> GettAllAccounts()
        {
            try
            {

                var users = await _userAction.GetAllAsync();
                var userlist = _mapper.Map<List<User>>(users);


                if (userlist.Count < 0)
                {
                    return NotFound("No users exist");
                }
                return Ok(userlist);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while creating the employee: {ex.Message}");
            }

        }
        [HttpGet("GetAccountById")]
        public async Task<IActionResult> GetUserById([FromQuery]int id)
        {
            try
            {

                var user = await _userAction.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound("No user exist");
                }
                var foundUser = _mapper.Map<User>(user);


                return Ok(foundUser);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while creating the employee: {ex.Message}");
            }

        }

        [HttpGet("SearchAccount")]
        public async Task<IActionResult> GetAccountBySearch([FromQuery] string searchQuery)
        {
            try
            {

                var searchResult = await _userRepo.SearchAsync(searchQuery);

                if(searchResult.Count <= 0)
                {
                   return NotFound("nothing found");
                }

                var foundUsers = _mapper.Map<List<User>>(searchResult);

                return Ok(foundUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while creating the employee: {ex.Message}");
            }

        }




        [HttpGet("GetUserFollowing")]
        public async Task<IActionResult> GetUserFollowing([FromQuery] int userId)
        {
            try
            {

                var Result = await _userRepo.GetUserFollowing(userId);

                if (Result.Count <= 0)
                {
                    return NotFound("nothing found");
                }

                var foundUsers = _mapper.Map<List<FollowerDTO>>(Result);

                return Ok(foundUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while creating the employee: {ex.Message}");
            }

        }



        #endregion

        [HttpPut("UpdateAccount")]
        public async Task<IActionResult> UpdateAccountData([FromBody] UserDTO user, [FromQuery] int userId)
        {
            try
            {
                
                var selectedUser = await _userAction.GetByIdAsync(userId);
                if (selectedUser == null)
                {
                 return NotFound($"user not found.");
                }
                var updatedUser = _mapper.Map<User>(user);
                await _userAction.Update(updatedUser);
                
                return Ok("updated successfully.");
                

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while updating the user: {ex.Message}");
            }
        }

        [HttpDelete("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount([FromQuery] int userId)
        {
            try
            {

                var selectedUser = await _userAction.GetByIdAsync(userId);
                if (selectedUser == null)
                {
                    return NotFound("user not found.");
                }
                await _userAction.Delete(selectedUser);

                return Ok("deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while updating the user: {ex.Message}");
            }
        }


    }

}
