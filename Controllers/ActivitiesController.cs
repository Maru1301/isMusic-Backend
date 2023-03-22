using api.iSMusic.Models;
using api.iSMusic.Models.Services;
using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.ViewModels.ActivityVMs;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        public IActionResult GetMainPageActivities()
        {
            var dtos = _service.GetMainPageActivities();

            return Ok(dtos.Select(dto => dto.ToIndexVM()));
        }

        //有用到
        [HttpGet]
        public IActionResult GetActivities()
        {
            var dtos = _service.GetActivities();

            return Ok(dtos.Select(dto => dto.ToIndexVM()));
        }

        //有用到
        [HttpGet]
        [Route("single/{id}")]
        [AllowAnonymous]
        public IActionResult GetSingleActivity([FromRoute] int id)
        {

            var result = _service.GetSingleActivity(id);
            if (result == null)
            {
                return BadRequest(result);
            }
            return Ok(result.ToIndexVM());

        }


        //做成按鈕試試看
        [HttpGet]
        [Route("{value}")]
        public IActionResult GetActivityByName([FromRoute] string value, [FromQuery] string sort = "Latest", [FromQuery] int typeId = 0)
        {
            var result = _service.GetActivityByName(value, sort, typeId);
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

        //搜尋用
        [HttpGet]
        [Route("search")]
        public IActionResult GetActivitiesBySearch([FromQuery]ActivityQueryParameters queryParameters)
        {
            var result = _service.GetActivitiesBySearch(queryParameters);

            return Ok(result.Select(dto => dto.ToIndexVM()));
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
