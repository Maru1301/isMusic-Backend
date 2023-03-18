using api.iSMusic.Models.DTOs.MemberDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.MemberVMs;

namespace api.iSMusic.Models.Infrastructures.Extensions
{
    public static class MemberExts
    {
        public static MemberDTO ToMemberDTO(this MemberEditVM source)
        {
            return new()
            {
                //Id = source.id,
                MemberNickName = source.MemberNickName!,                
                MemberAddress = source.MemberAddress,
                MemberCellphone = source.MemberCellPhone,
                MemberDateOfBirth = source.MemberDateOfBirth,
                AvatarId = source.AvatarId,
                MemberReceivedMessage = source.MemberReceivedMessage,
                MemberSharedData = source.MemberSharedData,
                LibraryPrivacy = source.LibraryPrivacy,
                CalenderPrivacy = source.CalenderPrivacy,                

            };
        }

        public static MemberRegisterDTO ToMemberDTO(this MemberRegisterVM source)
        {
            return new()
            {
                MemberNickName = source.NickName!,
                MemberAccount = source.Account!,
                MemberPassword = source.Password!,
                MemberEmail = source.Email!
            };
        }

        public static MemberDTO ToDTO(this Member entity)
        {
            return new MemberDTO
                {
                Id = entity.Id,
                MemberAccount = entity.MemberAccount,
                MemberEncryptedPassword = entity.MemberEncryptedPassword,
                MemberEmail = entity.MemberEmail,
                MemberNickName = entity.MemberNickName,
                MemberCellphone = entity.MemberCellphone,
                IsConfirmed = entity.IsConfirmed,
                ConfirmCode = entity.ConfirmCode,
                MemberAddress = entity.MemberAddress,
                MemberDateOfBirth = entity.MemberDateOfBirth,
                //AvatarId = entity.AvatarId,
                MemberReceivedMessage = entity.MemberReceivedMessage,
                MemberSharedData = entity.MemberSharedData,
                LibraryPrivacy = entity.LibraryPrivacy,
                CalenderPrivacy = entity.CalenderPrivacy,
                CreditCardId= entity.CreditCardId,
            };
        }

        public static MemberEditVM ToMemberEditVM(this MemberDTO source)
        {
            return new()
            {
                MemberNickName = source.MemberNickName!,                
                MemberAddress = source.MemberAddress,
                MemberCellPhone = source.MemberCellphone,
                MemberDateOfBirth = source.MemberDateOfBirth,
                MemberReceivedMessage = source.MemberReceivedMessage,
                MemberSharedData = source.MemberSharedData,

            };
        }

        public static MemberDTO ToLoginDTO(this MemberLoginVM source)
        {
            return new()
            {
                MemberAccount = source.MemberAccount!,
                MemberPassword = source.MemberPassword!,
            };
        }

        public static SubscriptionRecordsDTO ToSubscriptionRecordsDTO(this SubscriptionRecord dto)
        {
            return new()
            {
                Id = dto.Id,
                MemberId = dto.MemberId,
                SubscriptionPlanId = dto.SubscriptionPlanId,
                SubscribedTime= dto.SubscribedTime,
                SubscribedExpireTime= dto.SubscribedExpireTime,
            };
        }
    }
}
