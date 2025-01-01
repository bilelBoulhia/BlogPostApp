using ArtcilesServer.DTO;
using ArtcilesServer.Models;
using ArtcilesServer.Repo;
using ArtcilesServer.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArtcilesServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly ArticleRepo _articleRepo;
        private readonly GenericRepository<Article> _articleAction;

        public ArticleController(IMapper mapper,ArticleRepo articleRepo, GenericRepository<Article> articleActions, UserRepo userRepo)
        {

            _articleRepo = articleRepo;
            _mapper = mapper;
            _articleAction = articleActions;

        }

        #region post 
        [HttpPost("CreateArticle")]
        public async Task<IActionResult> CreateArticle([FromBody] ArticleDTO article)
        {
            try
            {
                var Article = _mapper.Map<Article>(article);

                await _articleAction.AddAsync(Article);

                return Ok("User account has been created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred : {ex.Message}");
            }
        }


     
        #endregion

        #region get
        [HttpGet("GettAllArticles")]
        public async Task<IActionResult> GetAllArticles()
        {
            try
            {

                var Result = await _articleAction.GetAllAsync();

                if (Result.Count <= 0)
                {
                    return NotFound("nothing found");
                }

                var foundArticles = _mapper.Map<List<ArticleDTO>>(Result);

                return Ok(foundArticles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }

        }

        [HttpGet("SearchArticle")]
        public async Task<IActionResult> GetArticleBySearch([FromQuery] string searchQuery)
        {
            try
            {

                var searchResult = await _articleRepo.SearchAsync(searchQuery);

                if (searchResult.Count <= 0)
                {
                    return NotFound("nothing found");
                }

                var foundArticles = _mapper.Map<List<Article>>(searchResult);

                return Ok(foundArticles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }

        }

        [HttpGet("GetArticleByUser")]
        public  async Task<IActionResult> GetArticleByUser([FromQuery]int userId) {

            try
            {

                var Result = await _articleRepo.GetArticleByUser(userId);

                if (Result.Count <= 0)
                {
                    return NotFound("nothing found");
                }

                var foundArticles = _mapper.Map<List<Article>>(Result);

                return Ok(foundArticles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }

        }
        [HttpGet("GetArticleByCategory")]
        public async Task<IActionResult> GetArticleByCategory([FromQuery]int categoryId)
        {
            try
            {

                var Result = await _articleRepo.GetArticleByCategory(categoryId);

                if (Result.Count <= 0)
                {
                    return NotFound("nothing found");
                }

                var foundArticles = _mapper.Map<List<Article>>(Result);

                return Ok(foundArticles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }


            #endregion

            #region update 
            [HttpPut("UpdateArticle")]
        public async Task<IActionResult> UpdateArticle([FromBody] ArticleDTO article, [FromQuery] int articleId)
        {
            try
            {

                var selectedArticle = await _articleAction.GetByIdAsync(articleId);
                if (selectedArticle == null)
                {
                    return NotFound($"user not found.");
                }
                var updatedArticle = _mapper.Map<Article>(article);
                await _articleAction.Update(updatedArticle);

                return Ok("updated successfully.");


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred : {ex.Message}");
            }
        }
        #endregion

        #region delete
        [HttpDelete("DeleteArticle")]
        public async Task<IActionResult> DeleteArticle([FromQuery] int userId)
        {
            try
            {

                var selectedArticle = await _articleAction.GetByIdAsync(userId);
                if (selectedArticle == null)
                {
                    return NotFound("article not found.");
                }
                await _articleAction.Delete(selectedArticle);

                return Ok("deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred : {ex.Message}");
            }
        }

        #endregion
    }
}
