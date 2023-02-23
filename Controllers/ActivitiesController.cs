using api.iSMusic.Models.Services;
using api.iSMusic.Models.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.iSMusic.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityRepository _activityRepository;

        private readonly ActivityService _service;

        public ActivitiesController(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
            _service = new ActivityService(activityRepository);
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
    }
}
