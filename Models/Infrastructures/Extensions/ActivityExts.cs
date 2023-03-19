using api.iSMusic.Models.DTOs.ActivityDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.ActivityVMs;
using NAudio.MediaFoundation;

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
            ActivityOrganizer = source.ActivityOrganizer.MemberNickName,
            ActivityType = source.ActivityType.TypeName,
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
            ActivityOrganizer = source.ActivityOrganizer,
            ActivityType = source.ActivityType
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
