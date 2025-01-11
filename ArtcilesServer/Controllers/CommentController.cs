using ArtcilesServer.DTO;
using ArtcilesServer.Interfaces;
using ArtcilesServer.Models;
using ArtcilesServer.Repo;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArtcilesServer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ValidatedControllerBase
    {
        private readonly IMapper _mapper;

        private readonly CommentRepo _commentRepo;
        private readonly GenericRepository<Comment> _commentAction;

        public CommentController(IMapper mapper, CommentRepo commentRepo, GenericRepository<Comment> commentActions)
        {

            _commentRepo = commentRepo;
            _mapper = mapper;
            _commentAction = commentActions;

        }

        [HttpPost("CreateComment")]
        public async Task<IActionResult> CreateComment([FromBody] CommentDTO comment)
        {
            try
            {
                var validationResult = ValidateUser(comment.UserId);
                if (validationResult != null) return validationResult;

                var Comment = _mapper.Map<Comment>(comment);

                await _commentAction.AddAsync(Comment);

                return Ok("comment added.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred : {ex.Message}");
            }
        }



        [HttpPost("addlike")]
        public async Task<IActionResult> AddLike([FromBody] CommentLikeDTO commentLike)
        {

            var validationResult = ValidateUser(commentLike.UserId);
            if (validationResult != null) return validationResult;


            try
            {
                await _commentRepo.addLike(commentLike);
                return Ok("Like added successfully.");
            }
            catch (ArgumentException ex)
            {

                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {

                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }



        [HttpGet("GetCommentByUser")]
        public async Task<IActionResult> GetCommentByUser([FromQuery] int userId)
        {

            try
            {


                var Result = await _commentRepo.GetCommentByUser(userId);

                if (Result.Count <= 0)
                {
                    return NotFound("nothing found");
                }

                var foundArticles = _mapper.Map<List<CommentDTO>>(Result);

                return Ok(foundArticles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }

        }

        [HttpGet("GetCommentByArticle")]
        public async Task<IActionResult> GetCommentByArticle([FromQuery] int articleId)
        {

            try
            {

                var Result = await _commentRepo.GetCommentByArticle(articleId);

                if (Result.Count <= 0)
                {
                    return NotFound("nothing found");
                }

                var foundComment = _mapper.Map<List<CommentDTO>>(Result);

                return Ok(foundComment);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }

        }

        [HttpGet("GetAllLikesOfaComment")]
        public async Task<IActionResult> GetAllLikesOfaComment([FromQuery] int commentId)
        {
            try
            {

                var Result = await _commentRepo.getAllLikesOfaComment(commentId);

                if (Result.Count <= 0)
                {
                    return NotFound("nothing found");
                }

                var foundLikes = _mapper.Map<List<UserDTO>>(Result);

                return Ok(foundLikes);
            }
            catch (ArgumentException ex)
            {

                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {

                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }

        }


        [HttpPut("UpdateComment")]
        public async Task<IActionResult> UpdateComment([FromBody] CommentDTO comment, [FromQuery] int commentId)
        {
            try
            {
                var validationResult = ValidateUser(comment.UserId);
                if (validationResult != null) return validationResult;

                var existingComment = await _commentAction.GetByIdAsync(commentId);
                if (existingComment == null)
                {
                    return NotFound($"Comment not found.");
                }

           
                _mapper.Map(comment, existingComment);  
                await _commentAction.Update(existingComment);

                return Ok("Updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("DeleteComment")]
        public async Task<IActionResult> DeleteComment([FromQuery] int commentId)
        {
            try
            {


                var selectedComment = await _commentAction.GetByIdAsync(commentId);

                var validationResult = ValidateUser(selectedComment.UserId);
                if (validationResult != null) return validationResult;

                if (selectedComment == null)
                {
                    return NotFound("Comment not found.");
                }
                await _commentAction.Delete(selectedComment);

                return Ok("deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred : {ex.Message}");


            }
        }


        [HttpDelete("RemoveCommentLike")]
        public async Task<IActionResult> DeleteLikeFromComment([FromQuery] int userId, [FromQuery] int commentId)
        {
            try
            {
                var validationResult = ValidateUser(userId);
                if (validationResult != null) return validationResult;

                await _commentRepo.RemoveLike(userId, commentId);

                return Ok("deleted successfully.");
            }
            catch (ArgumentException ex)
            {

                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {

                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

    }


}