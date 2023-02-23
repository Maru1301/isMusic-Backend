using api.iSMusic.Models.DTOs.ActivityDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Runtime;

namespace api.iSMusic.Models.Infrastructures.Repositories
{
    public class ActivityRepository : IRepository, IActivityRepository
    {
        private readonly AppDbContext _db;

        public ActivityRepository(AppDbContext db)
        {
            _db = db;
        }

        public IEnumerable<ActivityType> GetActivityTypes()
        {
            return _db.ActivityTypes.ToList();
        }

        public ActivityType? GetTypeByIdForCheck(int typeId)
        {
            return _db.ActivityTypes.Find(typeId);
        }

        public IEnumerable<ActivityIndexDTO> Search(string value = "", string sort = "Latest", int typeId = 0)
        {
            IEnumerable<Activity> activities = _db.Activities
                .Include(activity => activity.ActivityFollows)
                .Where(activity => activity.ActivityEndTime < DateTime.Now && activity.PublishedStatus != false);

            if(!string.IsNullOrEmpty(value)) 
            { 
                activities = activities.Where(activity => activity.ActivityName.Contains(value) || activity.ActivityLocation.Contains(value));
            }

            if(typeId != 0)
            {
                activities = activities.Where(activity => activity.ActivityTypeId == typeId);
            }

            switch (sort)
            {
                case "Latest":
                    activities = activities.OrderBy(activity => activity.ActivityEndTime).ToList(); 
                    break;
                case "Popular":
                    activities = activities.OrderByDescending(activity =>       activity.ActivityFollows.Count()).ToList();
                    break;
            }

            return activities.Select(activity => activity.ToIndexDTO());
        }
    }
}
