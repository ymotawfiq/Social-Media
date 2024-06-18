
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Extensions;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.SarehneRepository;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.PolicyService;

namespace SocialMedia.Api.Service.SarehneService
{
    public class SarehneService : ISarehneService
    {
        private readonly Policies policies = new();
        private readonly ISarehneRepository _sarehneRepository;
        private readonly UserManagerReturn _userManagerReturn;
        private readonly IPolicyService _policyService;
        public SarehneService(ISarehneRepository _sarehneRepository, UserManagerReturn _userManagerReturn,
            IPolicyService _policyService)
        {
            this._sarehneRepository = _sarehneRepository;
            this._userManagerReturn = _userManagerReturn;
            this._policyService = _policyService;
        }

        public async Task<ApiResponse<SarehneMessage>> DeleteMessageAsync(string messageId, SiteUser user)
        {
            var message = await _sarehneRepository.GetByIdAsync(messageId);
            if (message != null)
            {
                if (message.ReceiverId == user.Id)
                {
                    await _sarehneRepository.DeleteByIdAsync(messageId);
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
            var message = await _sarehneRepository.GetByIdAsync(messageId);
            if (message != null)
            {
                var policy = await _policyService.GetPolicyByNameAsync("public");
                if (policy != null && policy.ResponseObject != null)
                {
                    if (message.ReceiverId == user.Id 
                    || message.MessagePolicyId == policy.ResponseObject.Id)
                    {
                        return StatusCodeReturn<SarehneMessage>
                            ._200_Success("Message found successfully", message);
                    }
                    return StatusCodeReturn<SarehneMessage>
                        ._403_Forbidden();
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
                var messages = await _sarehneRepository.GetMessagesAsync(user.Id, policy.ResponseObject.Id);
                if (messages.ToList().Count == 0)
                {
                    return StatusCodeReturn<IEnumerable<SarehneMessage>>
                        ._200_Success("No public messages found", messages);
                }
                return StatusCodeReturn<IEnumerable<SarehneMessage>>
                        ._200_Success("Public messages found successfully", messages);
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
                if(policy != null && policy.ResponseObject != null)
                {
                    var newMessage = await _sarehneRepository.AddAsync(ConvertFromDto
                    .ConvertFromSendSarehneMessageDto(sendSarahaMessageDto, user!, receiver, 
                    policy.ResponseObject));
                    return StatusCodeReturn<SarehneMessage>
                        ._201_Created("Message sent successfully", newMessage);
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
            var message = await _sarehneRepository.GetByIdAsync(updateSarehneMessagePolicyDto.MessageId);
            if (message != null)
            {
                var policy = await _policyService.GetPolicyByIdOrNameAsync(
                    updateSarehneMessagePolicyDto.PolicyIdOrName);
                if(policy != null && policy.ResponseObject != null)
                {
                    if (policies.SarehneMessagePolicies.Contains(policy.ResponseObject.PolicyType))
                    {
                        if (user.Id == message.ReceiverId)
                        {
                            updateSarehneMessagePolicyDto.PolicyIdOrName = policy.ResponseObject.Id;
                            var updatedMessage = await _sarehneRepository.UpdateAsync(
                                ConvertFromDto.ConvertFromUpdateSarehneMessagePolicyDto(
                                    updateSarehneMessagePolicyDto, message));
                            return StatusCodeReturn<SarehneMessage>
                                ._200_Success("Policy updated successfully", updatedMessage);
                        }
                        return StatusCodeReturn<SarehneMessage>
                            ._403_Forbidden();
                    }
                    return StatusCodeReturn<SarehneMessage>
                            ._403_Forbidden("Invalid policy");
                }
                return StatusCodeReturn<SarehneMessage>
                            ._404_NotFound("Policy not found");
            }
            return StatusCodeReturn<SarehneMessage>
                            ._404_NotFound("Message not found");
        }



    }
}
