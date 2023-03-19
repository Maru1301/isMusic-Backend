using api.iSMusic.Models.DTOs.ActivityDTOs;
using api.iSMusic.Models.EFModels;
using System;

namespace api.iSMusic.Models.Services.Interfaces
{
    public interface IActivityRepository
    {
        IEnumerable<ActivityIndexDTO> Search(string value = "", string sort = "Latest", int typeId = 0);

        IEnumerable<ActivityIndexDTO> GetMemberFollowedActivities(int memberId);

        Activity? GetActivityByIdForCheck(int activityId);

        ActivityType? GetTypeByIdForCheck(int typeId);

        IEnumerable<ActivityType> GetActivityTypes();

        void AddNewActivity(ActivityCreateDTO dto);

        void FollowActivity(int memberId, int activityId, DateTime attendDate);

        void UnfollowActivity(int memberId, int activityId);

        IEnumerable<ActivityIndexDTO> GetActivities();

        ActivityIndexDTO GetSingleActivity(int id);

        Activity? CheckActivityByIdForCheck(int activityId);

        IEnumerable<ActivityIndexDTO> GetActivitiesBySearch(ActivityQueryParameters queryParameters);


	}
}
