using api.iSMusic.Models.DTOs.ActivityDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Services.Interfaces;
using static api.iSMusic.Controllers.ActivitiesController;

namespace api.iSMusic.Models.Services
{
    public class ActivityService
    {
        private readonly IActivityRepository _activityRepository;

        private string _searchInput = string.Empty;

        private string _searchSort = string.Empty;
        
        private int _searchType = 0;

        public ActivityService(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public IEnumerable<ActivityIndexDTO> GetMainPageActivities()
        {
            var latest = _activityRepository.Search();

            _searchSort = "Popular";
            var ranking = _activityRepository.Search(_searchInput, _searchSort);

            var result = latest.Concat(ranking);

            return result;
        }

        public (bool Success, string Message, IEnumerable<ActivityIndexDTO> Dtos) GetActivityByName(string value, SearchQuery query)
        {
            if(query.Sort != "Latest" || query.Sort != "Popular")
            {
                return (false, "排序條件錯誤", new List<ActivityIndexDTO>());
            }

            if(CheckTypeExistence(query.TypeId) == false) return (false, "活動類型不存在", new List<ActivityIndexDTO>());

            var dtos = _activityRepository.Search(value,query.Sort,query.TypeId);

            return (true, string.Empty, dtos);
        }

        private bool CheckTypeExistence(int typeId)
        {
            var type = _activityRepository.GetTypeByIdForCheck(typeId);

            return type != null;
        }
    }
}
