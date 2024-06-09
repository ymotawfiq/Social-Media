
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.SarehneRepository;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.UserAccountService;

namespace SocialMedia.Service.SarehneService
{
    public class SarehneService : ISarehneService
    {
        private readonly ISarehneRepository _sarehneRepository;
        private readonly UserManagerReturn _userManagerReturn;
        public SarehneService(ISarehneRepository _sarehneRepository, UserManagerReturn _userManagerReturn)
        {
            this._sarehneRepository = _sarehneRepository;
            this._userManagerReturn = _userManagerReturn;
        }

        public async Task<ApiResponse<SarehneMessage>> DeleteMessageAsync(string messageId, SiteUser user)
        {
            var message = await _sarehneRepository.GetMessageAsync(messageId);
            if (message != null)
            {
                if (message.ReceiverId == user.Id)
                {
                    await _sarehneRepository.DeleteMessageAsync(messageId);
                    SetNull(message);
                    return StatusCodeReturn<SarehneMessage>
                        ._200_Success("Message deleted successfully", message);
                }
                return StatusCodeReturn<SarehneMessage>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<SarehneMessage>
                    ._404_NotFound("Message not found");
        }

        public async Task<ApiResponse<SarehneMessage>> GetMessageAsync(string messageId, SiteUser user)
        {
            var message = await _sarehneRepository.GetMessageAsync(messageId);
            if (message != null)
            {
                if (message.ReceiverId == user.Id)
                {
                    SetNull(message);
                    return StatusCodeReturn<SarehneMessage>
                        ._200_Success("Message found successfully", message);
                }
                return StatusCodeReturn<SarehneMessage>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<SarehneMessage>
                    ._404_NotFound("Message not found");
        }

        public async Task<ApiResponse<IEnumerable<SarehneMessage>>> GetMessagesAsync(SiteUser user)
        {
            var messages = await _sarehneRepository.GetMessagesAsync(user.Id);
            foreach(var m in messages)
            {
                SetNull(m);
            }
            if (messages.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<SarehneMessage>>
                    ._200_Success("No messages found", messages);
            }
            return StatusCodeReturn<IEnumerable<SarehneMessage>>
                    ._200_Success("Messages found successfully", messages);
        }

        public async Task<ApiResponse<SarehneMessage>> SendMessageAsync(
            SendSarahaMessageDto sendSarahaMessageDto, SiteUser user)
        {
            var receiver = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                sendSarahaMessageDto.UserIdOrNameOrEmail);
            if (receiver != null)
            {
                if (user == null)
                {
                    sendSarahaMessageDto.ShareYourName = false;
                }
                var newMessage = await _sarehneRepository.SendMessageAsync(ConvertFromDto
                    .ConvertFromSendSarehneMessageDto(sendSarahaMessageDto, user!, receiver));
                SetNull(newMessage);
                return StatusCodeReturn<SarehneMessage>
                    ._201_Created("Message sent successfully", newMessage);
                
            }
            return StatusCodeReturn<SarehneMessage>
                ._404_NotFound("User you want to send message not found");
        }


        private void SetNull(SarehneMessage sarehneMessage)
        {
            sarehneMessage.User = null;
        }


    }
}
