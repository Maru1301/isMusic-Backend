using Microsoft.AspNetCore.Mvc;

namespace api.iSMusic.Models.Infrastructures.Extensions
{
    public static class ControllerBaseExts
    {
        public static int GetMemberId(this ControllerBase controller)
        {
            return int.Parse(controller.User.Claims.First(claim => claim.Type == "MemberId").Value);
        }
    }
}
