using ArtcilesServer.DTO;
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
    public class ReportController : ControllerBase
    {
        public class CommentController : ControllerBase
        {
            private readonly IMapper _mapper;

            private readonly ReportRepo _reportRepo;
            private readonly GenericRepository<Report> _reportAction;

            public CommentController(IMapper mapper, ReportRepo reportRepo, GenericRepository<Report> reportActions)
            {

                _reportRepo= reportRepo;
                _mapper = mapper;
                _reportAction = reportActions;

            }

            [HttpPost("CreateReport")]
            public async Task<IActionResult> CreateReport([FromBody] ReportDTO report)
            {
                try
                {
                    var Report= _mapper.Map<Report>(report);

                    await _reportAction.AddAsync(Report);

                    return Ok("report submited.");
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred : {ex.Message}");
                }
            }


            //[HttpGet("GetReportByUserId")]
            //public async Task<IActionResult> GetReportByUserId([FromQuery]int userid)
            //{
            //    var selectedReport = await _reportAction.GetByIdAsync(reportId);

            //}




            [HttpDelete("DeleteReport")]
            public async Task<IActionResult> DeleteReport([FromQuery] int reportId)
            {
                try
                {

                    var selectedReport= await _reportAction.GetByIdAsync(reportId);
                    if (selectedReport== null)
                    {
                        return NotFound("report not found.");
                    }
                    await _reportAction.Delete(selectedReport);

                    return Ok("deleted successfully.");
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred : {ex.Message}");
                }
            }

        }
    }
}
