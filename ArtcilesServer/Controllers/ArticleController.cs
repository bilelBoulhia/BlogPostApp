using ArtcilesServer.DTO;
using ArtcilesServer.Interfaces;
using ArtcilesServer.Models;
using ArtcilesServer.Repo;
using ArtcilesServer.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;


namespace ArtcilesServer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ValidatedControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ArticleRepo _articleRepo;
        private readonly GenericRepository<Article> _articleAction;
        private readonly GenericRepository<Category> _categoryAction;


        public ArticleController(IMapper mapper,ArticleRepo articleRepo, GenericRepository<Category> categoryActions, GenericRepository<Article> articleActions, UserRepo userRepo)
        {

            _articleRepo = articleRepo;
            _mapper = mapper;
            _articleAction = articleActions;
            _categoryAction = categoryActions;


        }
    
        #region post 
        [HttpPost("CreateArticle")]
        public async Task<IActionResult> CreateArticle([FromBody] ArticleDTO article)
        {




            var validationResult = ValidateUser(article.UserId);
            if (validationResult != null) return validationResult;

            var catergory = await _categoryAction.GetByIdAsync(article.CategoryId);
            if (catergory == null) return BadRequest("category doesn't exist");

            try
            {
                var Article = _mapper.Map<Article>(article);
                await _articleAction.AddAsync(Article);
                return Ok("created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("addlike")]
        public async Task<IActionResult> AddLike([FromBody] ArticleLikeDTO articlelike)
        {

            var validationResult = ValidateUser(articlelike.UserId);
            if (validationResult != null) return validationResult;


            try
            {
                await _articleRepo.addLike(articlelike);
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
        [HttpPost("SaveArticle")]
        public async Task<IActionResult> SaveArticle([FromBody] SaveArticleDTO SavedarticleDTO)
        {

            var validationResult = ValidateUser(SavedarticleDTO.UserId);
            if (validationResult != null) return validationResult;


            try
            {
                await _articleRepo.saveArticle(SavedarticleDTO);
                return Ok("Article saved successfully.");
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



        #endregion

        #region get
        [HttpGet("GetAllArticles")]
        public async Task<IActionResult> GetAllArticles()
        {
            try
            {


                var Result = await _articleRepo.GetAllArticles();

                if (Result.Count <= 0)
                {
                    return NotFound("nothing found");
                }

    

                return Ok(Result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }

        }


        [HttpGet("GetFullArticleDetails")]
        public async Task<IActionResult> GetFullArticleDetails([FromQuery]int articleId)
        {
            try
            {


                var Result = await _articleRepo.GetFullArticleDetails(articleId);

                if (Result.Count <= 0)
                {
                    return NotFound("nothing found");
                }



                return Ok(Result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }

        }

        [HttpGet("GetAllArticlesLikedByUser")]
        public async Task<IActionResult> GetAllArticlesLikedByUser([FromQuery]int userId)
        {
            try
            {

                var Result = await _articleRepo.getAllArticlesLikedByUser(userId);

                if (Result.Count <= 0)
                {
                    return NotFound("nothing found");
                }


                var result = _mapper.Map<List<ArticleDTO>>(Result);

                return Ok(result);
            } catch (ArgumentException ex)
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



        [HttpGet("GetAllArticlesSavedByUser")]
        public async Task<IActionResult> GetAllArticlesSavedByUser([FromQuery] int userId)
        {
            try
            {

                var Result = await _articleRepo.getAllSavedArticlesByUser(userId);

                if (Result.Count <= 0)
                {
                    return NotFound("nothing found");
                }




                return Ok(Result);
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

                

                return Ok(searchResult);
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


                return Ok(Result);
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


                return Ok(Result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        

        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {

            try
            {

                var Result = await _articleRepo.GetAllCategories();
                var mappedCategory = _mapper.Map<List<CategoryDTO>>(Result);

                if (Result == null)
                {
                    return NotFound("nothing found");
                }


                return Ok(mappedCategory);
            }
         
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ex.Message}");
            }
        }


        #endregion

        #region update 
        [HttpPut("UpdateArticle")]
        public async Task<IActionResult> UpdateArticle([FromBody] ArticleDTO article, [FromQuery] int articleId)
        {
            try
            {

                var validationResult = ValidateUser(article.UserId);
                if (validationResult != null) return validationResult;


                var selectedArticle = await _articleAction.GetByIdAsync(articleId);
                if (selectedArticle == null)
                {
                    return NotFound($"article not found.");
                }
                
                _mapper.Map(selectedArticle,articleId);      
                await _articleAction.Update(selectedArticle);

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
        public async Task<IActionResult> DeleteArticle([FromQuery] int articleId)
        {
            try
            {

                

                var selectedArticle = await _articleAction.GetByIdAsync(articleId);

                var validationResult = ValidateUser(selectedArticle.UserId);
                if (validationResult != null) return validationResult;


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

        [HttpDelete("RemoveArticleLike")]
        public async Task<IActionResult> DeleteLikeFromArticle([FromQuery] ArticleLikeDTO articleLikeDTO)
        {
            try
            {
                var validationResult = ValidateUser(articleLikeDTO.UserId);
                if (validationResult != null) return validationResult;

                await _articleRepo.RemoveLike(articleLikeDTO);

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

        [HttpDelete("RemoveSavedArticle")]
        public async Task<IActionResult> RemoveSavedArticle([FromQuery] SaveArticleDTO saveArticleDTO)
        {
            try
            {
                var validationResult = ValidateUser(saveArticleDTO.UserId);
                if (validationResult != null) return validationResult;

                await _articleRepo.RemoveSave(saveArticleDTO);

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

        #endregion




    }
}
