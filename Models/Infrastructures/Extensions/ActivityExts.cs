using api.iSMusic.Models.DTOs.ActivityDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.ActivityVMs;

public static class ActivityExts
{
    public static ActivityIndexDTO ToIndexDTO(this Activity source)
        => new()
        {
            Id = source.Id,
            ActivityName = source.ActivityName,
            ActivityStartTime = source.ActivityStartTime,
            ActivityEndTime = source.ActivityEndTime,
            ActivityLocation = source.ActivityLocation,
            ActivityTypeId = source.ActivityTypeId,
            ActivityInfo = source.ActivityInfo,
            ActivityImagePath = source.ActivityImagePath,
            ActivityOrganizerId= source.ActivityOrganizerId,
            Updated= source.Updated,
            TotalFollows= source.ActivityFollows.Count,
        };

    public static ActivityIndexVM ToIndexVM(this ActivityIndexDTO source)
        => new()
        {
            Id = source.Id,
            ActivityName = source.ActivityName,
            ActivityStartTime = source.ActivityStartTime,
            ActivityEndTime = source.ActivityEndTime,
            ActivityLocation = source.ActivityLocation,
            ActivityTypeId = source.ActivityTypeId,
            ActivityInfo = source.ActivityInfo,
            ActivityImagePath = source.ActivityImagePath,
            ActivityOrganizerId = source.ActivityOrganizerId,
            Updated = source.Updated,
            TotalFollows = source.TotalFollows,
        };
}
