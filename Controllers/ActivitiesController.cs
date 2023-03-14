using api.iSMusic.Models.Services;
using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.ViewModels.ActivityVMs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api.iSMusic.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityRepository _activityRepository;

        private readonly ActivityService _service;

        public ActivitiesController(IActivityRepository activityRepository, IMemberRepository memberRepository, IWebHostEnvironment webHostEnvironment)
        {
            _activityRepository = activityRepository;
            _service = new ActivityService(activityRepository, memberRepository, webHostEnvironment);
        }

        [HttpGet]
        [Route("Main")]
        public IActionResult GetMainPageActivities()
        {
            var dtos = _service.GetMainPageActivities();

            return Ok(dtos.Select(dto => dto.ToIndexVM()));
        }

        [HttpGet]
        [Route("{value}")]
        public IActionResult GetActivityByName(string value, [FromQuery] SearchQuery query)
        {
            var result = _service.GetActivityByName(value, query);
            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Dtos.Select(dto => dto.ToIndexVM()));
        }

        public class SearchQuery
        {
            public string Sort = "Latest";

            public int TypeId = 0;
        }

        [HttpGet]
        [Route("Types")]
        public IActionResult GetActivityTypes()
        {
            var types = _activityRepository.GetActivityTypes();

            return Ok(types);
        }

        [HttpPost]
        [Route("Organizers/{memberId}")]
        public IActionResult AddNewActivity(int memberId, [FromForm] ActivityCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity("存在非法欄位");
            }

            var result = _service.AddNewActivity(memberId, model.ToDTO());
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }
    }
}
