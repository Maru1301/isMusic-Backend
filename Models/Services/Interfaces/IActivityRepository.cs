using api.iSMusic.Models.DTOs.ActivityDTOs;
using api.iSMusic.Models.EFModels;
using System;

namespace api.iSMusic.Models.Services.Interfaces
{
    public interface IActivityRepository
    {
        IEnumerable<ActivityIndexDTO> Search(string value = "", string sort = "Latest", int typeId = 0);

        ActivityType? GetTypeByIdForCheck(int typeId);

        IEnumerable<ActivityType> GetActivityTypes();
    }
}
