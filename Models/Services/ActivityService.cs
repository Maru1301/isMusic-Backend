using api.iSMusic.Models.DTOs.ActivityDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using static api.iSMusic.Controllers.ActivitiesController;

namespace api.iSMusic.Models.Services
{
    public class ActivityService
    {
        private readonly IActivityRepository _activityRepository;

        private readonly IMemberRepository _memberRepository;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private string _searchInput = string.Empty;

        private string _searchSort = string.Empty;
        
        public ActivityService(IActivityRepository activityRepository, IMemberRepository memberRepository, IWebHostEnvironment webHostEnvironment)
        {
            _activityRepository = activityRepository;
            _memberRepository = memberRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IEnumerable<ActivityIndexDTO> GetMainPageActivities()
        {
            var latest = _activityRepository.Search();

            _searchSort = "Popular";
            var ranking = _activityRepository.Search(_searchInput, _searchSort);

            var result = latest.Concat(ranking);

            return result;
        }

        public IEnumerable<ActivityIndexDTO> GetActivities()
        {       
            var result = _activityRepository.GetActivities();

            return result;
        }

        public ActivityIndexDTO GetSingleActivity(int id)
        {
            if (!CheckActivityExistence(id))return null;
            var result = _activityRepository.GetSingleActivity(id);
        

            return result;
        }
        //public (bool Success, string Message, IEnumerable<ActivityIndexDTO> Dtos) GetActivityByName(string value, SearchQuery query)

        public (bool Success, string Message, IEnumerable<ActivityIndexDTO> Dtos) GetActivityByName(string value, string sort, int typeId)
        {
            if(sort != "Latest" && sort != "Popular")
            {
                return (false, "排序條件錯誤", new List<ActivityIndexDTO>());
            }

            if(CheckTypeExistence(typeId) == false) return (false, "活動類型不存在", new List<ActivityIndexDTO>());

            var dtos = _activityRepository.Search(value,sort,typeId);

            return (true, string.Empty, dtos);
        }

        public (bool Success, string Message) AddNewActivity(int memberId, ActivityCreateDTO dto)
        {
            if (CheckMemberExistence(memberId) == false) return (false, "會員不存在");

            if(dto.ActivityImage == null || dto.ActivityImage.Length == 0)
            {
                dto.ActivityImagePath = string.Empty;
            }
            else
            {
                var parentPath = Directory.GetParent(_webHostEnvironment.ContentRootPath)!.FullName;
                var coverPath = Path.Combine(parentPath, "iSMusic.ServerSide/iSMusic/Uploads/Covers");

                var fileName = Path.GetFileName(dto.ActivityImage.FileName);
                string newFileName = GetNewFileName(coverPath, fileName);
                var fullPath = Path.Combine(coverPath, newFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    dto.ActivityImage.CopyTo(stream);
                }

                dto.ActivityImagePath = newFileName;
            }
            dto.PublishedStatus = false;
            dto.ActivityOrganizerId = memberId;
            dto.Updated = DateTime.Now;

            _activityRepository.AddNewActivity(dto);

            return (true, "新增成功");
        }

        private string GetNewFileName(string path, string fileName)
        {
            string ext = System.IO.Path.GetExtension(fileName); // 取得副檔名,例如".jpg"
            string newFileName;
            string fullPath;
            // todo use song name + artists name instead of guid, so when uploading the new file it will replace the old one.
            do
            {
                newFileName = Guid.NewGuid().ToString("N") + ext;
                fullPath = System.IO.Path.Combine(path, newFileName);
            } while (System.IO.File.Exists(fullPath) == true); // 如果同檔名的檔案已存在,就重新再取一個新檔名

            return newFileName;
        }

        private bool CheckMemberExistence(int memberId)
        {
            var member = _memberRepository.GetMemberById(memberId);

            return member != null;
        }

        private bool CheckTypeExistence(int typeId)
        {
            var type = _activityRepository.GetTypeByIdForCheck(typeId);

            return type != null;
        }

        private bool CheckActivityExistence(int id)
        {
            var activity = _activityRepository.CheckActivityByIdForCheck(id);

            return activity != null;
        }

        public IEnumerable<ActivityIndexDTO> GetActivitiesBySearch(ActivityQueryParameters queryParameters)
        {
            return _activityRepository.GetActivitiesBySearch(queryParameters);
        }

    }
}
