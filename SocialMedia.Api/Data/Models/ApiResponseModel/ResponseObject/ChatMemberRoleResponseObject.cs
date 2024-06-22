namespace SocialMedia.Api.Data.Models.ApiResponseModel.ResponseObject
{
    public class ChatMemberRoleResponseObject
    {
        public ChatMemberResponseObject ChatMemberResponseObject { get; set; } = null!;
        public Role Role { get; set; } = null!;
        public ChatMemberRole ChatMemberRole { get; set; } = null!;
    }
}
