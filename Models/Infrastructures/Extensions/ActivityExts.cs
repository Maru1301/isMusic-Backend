using api.iSMusic.Models.DTOs.ActivityDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.ActivityVMs;

public static class ActivityExts
{
    private static readonly string webPicUrl = "https://localhost:44373/Uploads/Covers/";

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
            ActivityImagePath = webPicUrl + source.ActivityImagePath,
            ActivityOrganizerId = source.ActivityOrganizerId,
            Updated = source.Updated,
            TotalFollows = source.TotalFollows,
        };

    public static ActivityCreateDTO ToDTO(this ActivityCreateVM source)
        => new()
        {
            ActivityName = source.ActivityName,
            ActivityStartTime = source.ActivityStartTime,
            ActivityEndTime = source.ActivityEndTime,
            ActivityLocation = source.ActivityLocation,
            ActivityTypeId = source.ActivityTypeId,
            ActivityInfo = source.ActivityInfo,
            ActivityImagePath = source.ActivityImagePath,
            ActivityImage = source.ActivityImage,
        };
}
