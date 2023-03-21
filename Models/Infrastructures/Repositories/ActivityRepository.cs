using api.iSMusic.Models.DTOs.ActivityDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime;
using static api.iSMusic.Controllers.ActivitiesController;

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
				.Include(activity => activity.ActivityOrganizer)
				.Include(activity => activity.ActivityType)
				.Where(activity => activity.ActivityEndTime > DateTime.Now && activity.PublishedStatus != false);

			if (!string.IsNullOrEmpty(value))
			{
				activities = activities.Where(activity => activity.ActivityName.Contains(value) || activity.ActivityLocation.Contains(value));
			}

			if (typeId != 0)
			{
				activities = activities.Where(activity => activity.ActivityTypeId == typeId);
			}

			switch (sort)
			{
				case "Latest":
					activities = activities.OrderBy(activity => activity.ActivityEndTime).ToList();
					break;
				case "Popular":
					activities = activities.OrderByDescending(activity => activity.ActivityFollows.Count()).ToList();
					break;
			}

			return activities.Select(activity => activity.ToIndexDTO());
		}

		public void AddNewActivity(ActivityCreateDTO dto)
		{
			var activity = new Activity
			{
				ActivityName = dto.ActivityName,
				ActivityStartTime = dto.ActivityStartTime,
				ActivityEndTime = dto.ActivityEndTime,
				ActivityLocation = dto.ActivityLocation,
				ActivityTypeId = dto.ActivityTypeId,
				ActivityInfo = dto.ActivityInfo,
				ActivityOrganizerId = dto.ActivityOrganizerId,
				ActivityImagePath = dto.ActivityImagePath,
				PublishedStatus = dto.PublishedStatus,
				CheckedById = null,
				Updated = dto.Updated,
			};

			_db.Activities.Add(activity);
			_db.SaveChanges();
		}

		public Activity? GetActivityByIdForCheck(int activityId)
		{
			return _db.Activities.Find(activityId);
		}

		public void FollowActivity(int memberId, int activityId, DateTime attendDate)
		{
			var followActivity = new ActivityFollow
			{
				MemberId = memberId,
				ActivityId = activityId,
				AttendDate = attendDate,
			};

			_db.ActivityFollows.Add(followActivity);
			_db.SaveChanges();
		}

		public void UnfollowActivity(int memberId, int activityId)
		{
			var followActivity = _db.ActivityFollows.Single(af => af.MemberId == memberId && af.ActivityId == activityId);

			_db.ActivityFollows.Remove(followActivity);
			_db.SaveChanges();
		}

		public IEnumerable<ActivityIndexDTO> GetMemberFollowedActivities(int memberId)
		{

			return _db.ActivityFollows
				.Where(af => af.MemberId == memberId && af.AttendDate.Year == DateTime.Now.Year)
				.Include(af => af.Activity)
				.ThenInclude(a=>a.ActivityType)
				.Include(af=>af.Activity)
				.ThenInclude(a=>a.ActivityOrganizer)
				.Select(af => af.Activity)
				.Select(activity => activity.ToIndexDTO());
		}

		public IEnumerable<ActivityIndexDTO> GetActivities()
		{
			return _db.Activities
				.Include(x => x.ActivityType)
				.Include(x => x.ActivityOrganizer)
				.OrderBy(x => x.ActivityStartTime)
				.Select(x => x.ToIndexDTO()).ToList();
		}

		public ActivityIndexDTO GetSingleActivity(int id)
		{
			return _db.Activities
				.Include(activityType => activityType.ActivityType)
				.Include(activity => activity.ActivityOrganizer)
				.Where(x => x.Id == id).SingleOrDefault()!.ToIndexDTO();
		}
		public Activity? CheckActivityByIdForCheck(int activityId)
		{
			return _db.Activities.Find(activityId);
		}

		public IEnumerable<ActivityIndexDTO> GetActivitiesBySearch(ActivityQueryParameters queryParameters)
		{
			var filteredActivities = _db.Activities
										.Where(a => (
														(string.IsNullOrEmpty(queryParameters.ActivityName) || a.ActivityName.Contains(queryParameters.ActivityName)) ||
														(string.IsNullOrEmpty(queryParameters.ActivityLocation) || a.ActivityLocation.Contains(queryParameters.ActivityLocation)) ||
														(string.IsNullOrEmpty(queryParameters.ActivityTypeName) || a.ActivityType.TypeName.Contains(queryParameters.ActivityTypeName))
													) &&
														(a.ActivityEndTime > DateTime.Now)
													);

			var activityIndexDTOs = filteredActivities.Select(a => new ActivityIndexDTO
			{
				Id = a.Id,
				ActivityName = a.ActivityName,
				ActivityStartTime = a.ActivityStartTime,
				ActivityEndTime = a.ActivityEndTime,
				ActivityLocation = a.ActivityLocation,
				ActivityType = a.ActivityType.TypeName,
				ActivityOrganizer = a.ActivityOrganizer.MemberNickName,
				ActivityTypeId = a.ActivityTypeId,
				ActivityInfo = a.ActivityInfo,
				ActivityOrganizerId = a.ActivityOrganizerId,
				ActivityImagePath = a.ActivityImagePath,
				Updated = a.Updated,
				TotalFollows = a.ActivityFollows.Count,
			});

			return activityIndexDTOs.ToList();
		}

	}
}
