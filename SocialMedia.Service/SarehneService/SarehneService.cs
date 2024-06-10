
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.SarehneMessagePolicyRepository;
using SocialMedia.Repository.SarehneRepository;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.PolicyService;

namespace SocialMedia.Service.SarehneService
{
    public class SarehneService : ISarehneService
    {
        private readonly ISarehneRepository _sarehneRepository;
        private readonly UserManagerReturn _userManagerReturn;
        private readonly IPolicyService _policyService;
        private readonly ISarehneMessagePolicyRepository _sarehneMessagePolicyRepository;
        public SarehneService(ISarehneRepository _sarehneRepository, UserManagerReturn _userManagerReturn,
            IPolicyService _policyService, ISarehneMessagePolicyRepository _sarehneMessagePolicyRepository)
        {
            this._sarehneRepository = _sarehneRepository;
            this._userManagerReturn = _userManagerReturn;
            this._policyService = _policyService;
            this._sarehneMessagePolicyRepository = _sarehneMessagePolicyRepository;
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
                var policy = await _policyService.GetPolicyByNameAsync("public");
                if (policy != null && policy.ResponseObject != null)
                {
                    var messagePolicy = await _sarehneMessagePolicyRepository.GetPolicyByPolicyIdAsync(
                    policy.ResponseObject.Id);
                    if (messagePolicy != null)
                    {
                        if (message.ReceiverId == user.Id || message.MessagePolicyId == messagePolicy.Id)
                        {
                            SetNull(message);
                            return StatusCodeReturn<SarehneMessage>
                                ._200_Success("Message found successfully", message);
                        }
                        return StatusCodeReturn<SarehneMessage>
                            ._403_Forbidden();
                    }
                    return StatusCodeReturn<SarehneMessage>
                    ._404_NotFound("Message policy not found");
                }
                return StatusCodeReturn<SarehneMessage>
                    ._404_NotFound("Policy not found");
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

        public async Task<ApiResponse<IEnumerable<SarehneMessage>>> GetPublicMessagesAsync(SiteUser user)
        {
            var policy = await _policyService.GetPolicyByNameAsync("public");
            if(policy!=null && policy.ResponseObject != null)
            {
                var messagePolicy = await _sarehneMessagePolicyRepository.GetPolicyByPolicyIdAsync(
                    policy.ResponseObject.Id);
                if (messagePolicy != null)
                {
                    var messages = await _sarehneRepository.GetMessagesAsync(user.Id, messagePolicy.Id);
                    foreach(var m in messages)
                    {
                        SetNull(m);
                    }
                    if (messages.ToList().Count == 0)
                    {
                        return StatusCodeReturn<IEnumerable<SarehneMessage>>
                            ._200_Success("No public messages found", messages);
                    }
                    return StatusCodeReturn<IEnumerable<SarehneMessage>>
                            ._200_Success("Public messages found successfully", messages);
                }
                return StatusCodeReturn<IEnumerable<SarehneMessage>>
                            ._404_NotFound("Message policy not found");
            }
            return StatusCodeReturn<IEnumerable< SarehneMessage >>
                            ._404_NotFound("Policy not found");
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
                var policy = await _policyService.GetPolicyByNameAsync("private");
                if(policy!=null && policy.ResponseObject != null)
                {
                    var messagePolicy = await _sarehneMessagePolicyRepository.GetPolicyByPolicyIdAsync(
                    policy.ResponseObject.Id);
                    if (messagePolicy != null)
                    {
                        var newMessage = await _sarehneRepository.SendMessageAsync(ConvertFromDto
                        .ConvertFromSendSarehneMessageDto(sendSarahaMessageDto, user!, receiver, messagePolicy));
                        SetNull(newMessage);
                        return StatusCodeReturn<SarehneMessage>
                            ._201_Created("Message sent successfully", newMessage);
                    }
                    return StatusCodeReturn<SarehneMessage>
                        ._404_NotFound("Message policy not found");
                }
                return StatusCodeReturn<SarehneMessage>
                        ._404_NotFound("Policy not found");
            }
            return StatusCodeReturn<SarehneMessage>
                ._404_NotFound("User you want to send message not found");
        }

        public async Task<ApiResponse<SarehneMessage>> UpdateMessagePolicyAsync(
            UpdateSarehneMessagePolicyDto updateSarehneMessagePolicyDto, SiteUser user)
        {
            var message = await _sarehneRepository.GetMessageAsync(updateSarehneMessagePolicyDto.MessageId);
            if (message != null)
            {
                var policy = await _policyService.GetPolicyByIdOrNameAsync(
                    updateSarehneMessagePolicyDto.PolicyIdOrName);
                if(policy!=null && policy.ResponseObject != null)
                {
                    var messagePolicy = await _sarehneMessagePolicyRepository.GetPolicyByPolicyIdAsync(
                        policy.ResponseObject.Id);
                    if (messagePolicy != null)
                    {
                        if(user.Id == message.ReceiverId)
                        {
                            updateSarehneMessagePolicyDto.PolicyIdOrName = messagePolicy.Id;
                            var updatedMessage = await _sarehneRepository.UpdateMessagePolicyAsync(
                                ConvertFromDto.ConvertFromUpdateSarehneMessagePolicyDto(
                                    updateSarehneMessagePolicyDto, message));
                            SetNull(updatedMessage);
                            return StatusCodeReturn<SarehneMessage>
                                ._200_Success("Policy updated successfully", updatedMessage);
                        }
                        return StatusCodeReturn<SarehneMessage>
                            ._403_Forbidden();
                    }
                    return StatusCodeReturn<SarehneMessage>
                            ._404_NotFound("Message policy not found");
                }
                return StatusCodeReturn<SarehneMessage>
                            ._404_NotFound("Policy not found");
            }
            return StatusCodeReturn<SarehneMessage>
                            ._404_NotFound("Message not found");
        }

        private void SetNull(SarehneMessage sarehneMessage)
        {
            sarehneMessage.User = null;
            sarehneMessage.SarehneMessagePolicy = null;
        }


    }
}
