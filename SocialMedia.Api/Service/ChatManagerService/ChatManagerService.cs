using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.ChatMemberRepository;
using SocialMedia.Api.Repository.ChatMemberRoleRepository;
using SocialMedia.Api.Repository.ChatRepository;
using SocialMedia.Api.Repository.PrivateChatRepository;
using SocialMedia.Api.Service.BlockService;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.PolicyService;
using SocialMedia.Api.Service.RolesService;
using System;

namespace SocialMedia.Api.Service.ChatManagerService
{
    public class ChatManagerService : IChatManagerService
    {
        private readonly IChatMemberRepository _chatMemberRepository;
        private readonly IChatRepository _chatRepository;
        private readonly UserManagerReturn _userManagerReturn;
        private readonly IChatMemberRoleRepository _chatMemberRoleRepository;
        private readonly IRolesService _rolesService;
        private readonly IPolicyService _policyService;
        private readonly IBlockService _blockService;
        private readonly IPrivateChatRepository _privateChatRepository;
        public ChatManagerService(IChatMemberRepository _chatMemberRepository, IPolicyService _policyService,
            IChatRepository _chatRepository, UserManagerReturn _userManagerReturn,
            IBlockService _blockService, IPrivateChatRepository _privateChatRepository, 
            IChatMemberRoleRepository _chatMemberRoleRepository, IRolesService _rolesService)
        {
            this._chatMemberRepository = _chatMemberRepository;
            this._chatRepository = _chatRepository;
            this._userManagerReturn = _userManagerReturn;
            this._chatMemberRoleRepository = _chatMemberRoleRepository;
            this._rolesService = _rolesService;
            this._policyService = _policyService;
            this._blockService = _blockService;
            this._privateChatRepository = _privateChatRepository;
        }



        #region Private Chat
        public async Task<ApiResponse<PrivateChat>> UnSendPrivateChatRequestAsync(string privateChatId, 
            SiteUser user)
        {
            var privateChat = await _privateChatRepository.GetByIdAsync(privateChatId);
            if (privateChat != null)
            {
                if (!privateChat.IsAccepted)
                {
                    if (privateChat.User1Id == user.Id)
                    {
                        await _privateChatRepository.DeleteByIdAsync(privateChatId);
                        await _chatRepository.DeleteByIdAsync(privateChat.ChatId);
                        return StatusCodeReturn<PrivateChat>
                            ._200_Success("Private chat request unsent successfully", privateChat);
                    }
                    return StatusCodeReturn<PrivateChat>
                            ._403_Forbidden();
                }
                return StatusCodeReturn<PrivateChat>
                            ._403_Forbidden("Chat request already accepted");
            }
            return StatusCodeReturn<PrivateChat>
                        ._404_NotFound("No sent request ound");
        }

        public async Task<ApiResponse<PrivateChat>> AddPrivateChatRequestAsync(
            string userIdOrNameOrEmail, SiteUser user1)
        {
            var user2 = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(userIdOrNameOrEmail);
            var policy = await _policyService.GetPolicyByIdOrNameAsync("private");
            var abiliy = await IsAbleToSendPrivateChatRequestAsync<PrivateChat>(user1, userIdOrNameOrEmail);
            if (abiliy.IsSuccess)
            {
                var chat = await _chatRepository.AddAsync(ChatObject(null!, null!, 
                    policy.ResponseObject!.Id));

                var privateChat = await _privateChatRepository.AddAsync(AddPrivateChat(
                    chat.Id, user1.Id, user2.Id));
                return StatusCodeReturn<PrivateChat>
                    ._201_Created("Chat request sent successfully", privateChat);
            }
            return abiliy;
        }

        public async Task<ApiResponse<PrivateChat>> AcceptPrivateChatRequestAsync(string chatId, SiteUser user)
        {
            var chat = await _chatRepository.GetByIdAsync(chatId);
            if (chat != null)
            {
                var privateChat = await _privateChatRepository.GetByMemberAndChatIdAsync(chat.Id, user.Id);
                if (privateChat != null)
                {
                    if (privateChat.User2Id == user.Id)
                    {
                        privateChat.IsAccepted = true;
                        await _privateChatRepository.UpdateAsync(privateChat);
                        return StatusCodeReturn<PrivateChat>
                            ._200_Success("Chat request accepted successfully", privateChat);
                    }
                    return StatusCodeReturn<PrivateChat>
                        ._403_Forbidden();
                }
                return StatusCodeReturn<PrivateChat>
                        ._404_NotFound("No request found");
            }
            return StatusCodeReturn<PrivateChat>
                        ._404_NotFound("Chat not found");
        }

        public async Task<ApiResponse<IEnumerable<PrivateChat>>> GetReceivedPrivateChatRequestsAsync(
            SiteUser user)
        {
            var chatRequests = await _privateChatRepository.GetReceivedChatRequestsAsync(user.Id);
            if (chatRequests.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<PrivateChat>>
                    ._200_Success("No received chat requests found", chatRequests);
            }
            return StatusCodeReturn<IEnumerable<PrivateChat>>
                    ._200_Success("Received chat requests found successfully", chatRequests);
        }

        public async Task<ApiResponse<IEnumerable<PrivateChat>>> GetSentPrivateChatRequestsAsync(
            SiteUser user)
        {
            var chatRequests = await _privateChatRepository.GetSentChatRequestsAsync(user.Id);
            if (chatRequests.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<PrivateChat>>
                    ._200_Success("No sent chat requests found", chatRequests);
            }
            return StatusCodeReturn<IEnumerable<PrivateChat>>
                    ._200_Success("Sent Chat requests found successfully", chatRequests);
        }

        public async Task<ApiResponse<IEnumerable<PrivateChat>>> GetPrivateChatsAsync(SiteUser user)
        {
            var chatRequests = await _privateChatRepository.GetUserChatsAsync(user.Id);
            if (chatRequests != null)
            {
                if (chatRequests.ToList().Count == 0)
                {
                    return StatusCodeReturn<IEnumerable<PrivateChat>>
                        ._200_Success("No private chats found", chatRequests);
                }
                return StatusCodeReturn<IEnumerable<PrivateChat>>
                        ._200_Success("Private chats found successfully", chatRequests);
            }
            return StatusCodeReturn<IEnumerable<PrivateChat>>
                        ._404_NotFound("No private chat requests found");
        }

        private async Task<ApiResponse<T>> IsAbleToSendPrivateChatRequestAsync<T>(
            SiteUser user1, string userIdOrNameOrEmail)
        {
            var user2 = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(userIdOrNameOrEmail);
            if (user2 != null)
            {
                var isBlocked = await _blockService.GetBlockByUserIdAndBlockedUserIdAsync(user1.Id, user2.Id);
                if (isBlocked.ResponseObject == null && user1.Id != user2.Id)
                {
                    var policy = await _policyService.GetPolicyByIdOrNameAsync("private");
                    if (policy != null && policy.ResponseObject != null)
                    {
                        var chatMember = await _privateChatRepository.GetByMember1AndMember2IdAsync(user1.Id,
                            user2.Id);
                        if (chatMember != null)
                        {
                            if (chatMember.IsAccepted)
                            {
                                return StatusCodeReturn<T>
                                    ._403_Forbidden("You already has private chat with this user");
                            }
                            return StatusCodeReturn<T>
                                    ._403_Forbidden("Please wait untill accept your chat request");
                        }
                        return StatusCodeReturn<T>
                                    ._200_Success("Allowed");
                    }
                    return StatusCodeReturn<T>
                        ._404_NotFound("Policy not found");
                }
                return StatusCodeReturn<T>
                                     ._403_Forbidden();
            }
            return StatusCodeReturn<T>
                    ._404_NotFound("User you want to send chat request not found");
        }

        public async Task<ApiResponse<PrivateChat>> BlockChatByChatIdAsync(string chatId, SiteUser user)
        {
            var privateChat = await _privateChatRepository.GetByMemberAndChatIdAsync(chatId, user.Id);
            if (privateChat != null)
            {
                if(privateChat.User1Id == user.Id)
                {
                    privateChat.IsBlockedByUser1 = true;
                }
                else
                {
                    privateChat.IsBlockedByUser2 = true;
                }
                privateChat.IsBlocked = true;
                await _privateChatRepository.UpdateAsync(privateChat);
                return StatusCodeReturn<PrivateChat>
                    ._200_Success("Blocked successfully", privateChat);
            }
            return StatusCodeReturn<PrivateChat>
                ._404_NotFound("Chat not found");
        }

        public async Task<ApiResponse<PrivateChat>> BlockChatByPrivateChatIdAsync(string privateChatId,
            SiteUser user)
        {
            var privateChat = await _privateChatRepository.GetByIdAsync(privateChatId);
            return await BlockChatByChatIdAsync(privateChat.ChatId, user);
        }

        public async Task<ApiResponse<PrivateChat>> UnBlockChatByChatIdAsync(string chatId, SiteUser user)
        {
            var privateChat = await _privateChatRepository.GetByMemberAndChatIdAsync(chatId, user.Id);
            if (privateChat != null)
            {
                if (privateChat.User1Id == user.Id)
                {
                    privateChat.IsBlockedByUser1 = false;
                }
                else
                {
                    privateChat.IsBlockedByUser2 = false;
                }
                if(!privateChat.IsBlockedByUser1 && !privateChat.IsBlockedByUser2)
                {
                    privateChat.IsBlocked = false;
                }
                await _privateChatRepository.UpdateAsync(privateChat);
                return StatusCodeReturn<PrivateChat>
                    ._200_Success("Unblocked successfully", privateChat);
            }
            return StatusCodeReturn<PrivateChat>
                ._404_NotFound("Chat not found");
        }

        public async Task<ApiResponse<PrivateChat>> UnBlockChatByPrivateChatIdAsync(string privateChatId,
            SiteUser user)
        {
            var privateChat = await _privateChatRepository.GetByIdAsync(privateChatId);
            return await UnBlockChatByChatIdAsync(privateChat.ChatId, user);
        }


        #endregion

        public async Task<ApiResponse<Chat>> GetGroupChatByIdAsync(string chatId, SiteUser user)
        {
            var chat = await _chatRepository.GetByIdAsync(chatId);
            if (chat != null)
            {
                var policy = await _policyService.GetPolicyByNameAsync("private");
                if (policy != null && policy.ResponseObject != null)
                {
                    if (policy.ResponseObject.Id != chat.PolicyId)
                    {
                        return StatusCodeReturn<Chat>
                            ._200_Success("Chat found successfully", chat);
                    }
                    return StatusCodeReturn<Chat>
                    ._403_Forbidden();
                }
                return StatusCodeReturn<Chat>
                    ._404_NotFound("Private policy not found");
            }
            return StatusCodeReturn<Chat>
                    ._404_NotFound("Chat not found");
        }

        public async Task<ApiResponse<Chat>> DeleteGroupChatByIdAsync(string chatId, SiteUser user)
        {
            var chat = await _chatRepository.GetByIdAsync(chatId);
            if (chat != null)
            {
                if(chat.CreatorId == user.Id)
                {
                    await _chatRepository.DeleteByIdAsync(chatId);
                    return StatusCodeReturn<Chat>
                            ._200_Success("Chat deleted successfully", chat);
                }
                return StatusCodeReturn<Chat>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<Chat>
                    ._404_NotFound("Chat not found");
        }

        public async Task<ApiResponse<IEnumerable<PrivateChat>>> GetNotAcceptedPrivateChatRequestsAsync(
            SiteUser user)
        {
            var chatRequests = await _privateChatRepository.GetSentChatRequestsAsync(user.Id);
            if (chatRequests.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<PrivateChat>>
                    ._200_Success("No private chat join requests sent", chatRequests);
            }
            return StatusCodeReturn<IEnumerable<PrivateChat>>
                    ._200_Success("Private chat join requests found successfully", chatRequests);
        }

        public async Task<ApiResponse<IEnumerable<ChatMember>>> GetNotAcceptedGroupChatRequestsAsync(
            SiteUser user)
        {
            var chatRequests = await _chatMemberRepository.GetNotAcceptedGroupChatRequestsAsync(user.Id);
            if (chatRequests.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<ChatMember>>
                    ._200_Success("No group chat join requests sent", chatRequests);
            }
            return StatusCodeReturn<IEnumerable<ChatMember>>
                    ._200_Success("Group chat join requests found successfully", chatRequests);
        }

        #region Group Chat

        public async Task<ApiResponse<IEnumerable<Chat>>> GetGroupChatsCreatedByUserAsync(SiteUser user)
        {
            var chats = await _chatRepository.GetUserCreatedChatsAsync(user.Id);
            if (chats.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<Chat>>
                    ._200_Success("No group chat Created by you", chats);
            }
            return StatusCodeReturn<IEnumerable<Chat>>
                    ._200_Success("Group chats found successfully", chats);
        }

        public async Task<ApiResponse<Chat>> UpdateGroupChatByIdAsync(UpdateChatDto updateChatDto,
            SiteUser user)
        {
            var chat = await _chatRepository.GetByIdAsync(updateChatDto.ChatId);
            if (chat != null)
            {
                if((await IsInRoleRoleAsync(updateChatDto.ChatId, "admin", user)).IsSuccess)
                {
                    chat.Name = updateChatDto.Name;
                    chat.Description = updateChatDto.Description;
                    var updatedChat = await _chatRepository.UpdateAsync(chat);
                    return StatusCodeReturn<Chat>
                        ._200_Success("Chat updated successfully", updatedChat);
                }
                return StatusCodeReturn<Chat>
                        ._403_Forbidden();
            }
            return StatusCodeReturn<Chat>
                    ._404_NotFound("Chat not found");
        }

        public async Task<ApiResponse<ChatMember>> UnSendGroupChatRequestAsync(string chatMemberId,
            SiteUser user)
        {
            var chatMember = await _chatMemberRepository.GetByIdAsync(chatMemberId);
            if (chatMember != null)
            {
                if (!chatMember.IsMember)
                {
                    if (chatMember.MemberId == user.Id)
                    {
                        await _chatMemberRepository.DeleteByIdAsync(chatMemberId);
                        return StatusCodeReturn<ChatMember>
                            ._200_Success("Group chat request unsent successfully", chatMember);
                    }
                    return StatusCodeReturn<ChatMember>
                            ._403_Forbidden();
                }
                return StatusCodeReturn<ChatMember>
                            ._403_Forbidden("Chat request already accepted");
            }
            return StatusCodeReturn<ChatMember>
                        ._404_NotFound("No sent request found");
        }

        public async Task<ApiResponse<Chat>> AddGroupChatAsync(AddChatDto addChatDto,
            SiteUser user)
        {
            var role = await _rolesService.GetRoleByRoleNameAsync("admin");
            if (role != null && role.ResponseObject != null)
            {
                var policy = await _policyService.GetPolicyByNameAsync("public");
                if (policy != null && policy.ResponseObject != null)
                {
                    var chat = await _chatRepository.AddAsync(ChatObject(addChatDto, user, 
                        policy.ResponseObject.Id));
                    var newChatMember = await _chatMemberRepository.AddAsync(
                        ChatMember(chat.Id, user.Id, null!, true));
                    await _chatMemberRoleRepository.AddAsync(ChatMemberRole(
                        newChatMember.Id, role.ResponseObject.Id));
                    return StatusCodeReturn<Chat>
                    ._201_Created("Group chat created successfully", chat);
                }
                return StatusCodeReturn<Chat>
                ._404_NotFound("Public policy not found");
            }
            return StatusCodeReturn<Chat>
                ._404_NotFound("Admin role not found");
        }

        private async Task<ApiResponse<ChatMember>> IsAbleToSendGroupChatJoinRequestAsync(
            string chatId, SiteUser user)
        {
            var chat = await _chatRepository.GetByIdAsync(chatId);
            if (chat != null)
            {
                var policy = await _policyService.GetPolicyByIdAsync(chat.PolicyId);
                if (policy.ResponseObject != null && policy.ResponseObject.PolicyType != "PRIVATE")
                {
                    if(user.Id != chat.CreatorId)
                    {
                        var chatMember = await _chatMemberRepository.GetByMemberAndChatIdAsync(
                            chat.Id, user.Id);
                        if (chatMember != null)
                        {
                            if (chatMember.IsMember)
                            {
                                return StatusCodeReturn<ChatMember>
                                    ._403_Forbidden("Already group member");
                            }
                            return StatusCodeReturn<ChatMember>
                                    ._403_Forbidden("Wait untill one of admins accept your chat request");
                        }
                        return StatusCodeReturn<ChatMember>
                                    ._200_Success("Allowed");
                    }
                    return StatusCodeReturn<ChatMember>
                    ._403_Forbidden();
                }
                return StatusCodeReturn<ChatMember>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<ChatMember>
                ._404_NotFound("Chat not found");
        }

        public async Task<ApiResponse<ChatMember>> AddChatJoinRequestAsync(string chatId,
            SiteUser user)
        {
            if((await IsAbleToSendGroupChatJoinRequestAsync(chatId, user)).IsSuccess)
            {
                var chatJoinRequest = await _chatMemberRepository.AddAsync(ChatMember(
                    chatId, user.Id, null!, false));
                return StatusCodeReturn<ChatMember>
                    ._201_Created("Chat join request sent successfully wait untill one of admins accept",
                        chatJoinRequest);
            }
            return await IsAbleToSendGroupChatJoinRequestAsync(chatId, user);
        }

        private async Task<ApiResponse<ChatMember>> IsAbleToAcceptChatJoinRequestAsync(
                ChatMember chatMember, SiteUser user)
        {
            if ((await IsInRoleRoleAsync(chatMember.ChatId, "admin", user)).IsSuccess)
            {
                if (!chatMember.IsMember)
                {
                    return StatusCodeReturn<ChatMember>
                        ._200_Success("Allowed");
                }
                return StatusCodeReturn<ChatMember>
                    ._403_Forbidden("Already group member");
            }
            return StatusCodeReturn<ChatMember>
                ._403_Forbidden("Admins only can accept chat join requests");
        }

        public async Task<ApiResponse<ChatMember>> AcceptChatJoinRequestAsync(
                string chatMemberId, SiteUser user)
        {
            var chatMember = await _chatMemberRepository.GetByIdAsync(chatMemberId);
            if (chatMember != null)
            {
                var userRole = await _rolesService.GetRoleByRoleNameAsync("user");
                if (userRole != null && userRole.ResponseObject != null)
                {
                    if ((await IsAbleToAcceptChatJoinRequestAsync(chatMember, user)).IsSuccess)
                    {
                        chatMember.IsMember = true;
                        var newMember = await _chatMemberRepository.UpdateAsync(chatMember);
                        await _chatMemberRoleRepository.AddAsync(ChatMemberRole(
                            chatMemberId, userRole.ResponseObject.Id));
                        return StatusCodeReturn<ChatMember>
                            ._200_Success("Request join accepted successfully", newMember);
                    }
                    return await IsAbleToAcceptChatJoinRequestAsync(chatMember, user);
                }
                return StatusCodeReturn<ChatMember>
                            ._404_NotFound("User role not found");
            }
            return StatusCodeReturn<ChatMember>
                            ._404_NotFound("Chat member not found");
        }

        public async Task<ApiResponse<IEnumerable<ChatMember>>> GetChatMembersAsync(
            string chatId, SiteUser user)
        {
            var chatMembers = await _chatMemberRepository.GetChatMembersAsync(chatId);
            var isChatMember = await _chatMemberRepository.GetByMemberAndChatIdAsync(chatId, user.Id);
            if (isChatMember != null && isChatMember.IsMember)
            {
                if (chatMembers.ToList().Count == 0)
                {
                    return StatusCodeReturn<IEnumerable<ChatMember>>
                        ._200_Success("No members found", chatMembers);
                }
                return StatusCodeReturn<IEnumerable<ChatMember>>
                            ._200_Success("Chat members found successfully", chatMembers);
            }
            return StatusCodeReturn<IEnumerable<ChatMember>>
                            ._403_Forbidden("Only group members can view other members");
        }

        public async Task<ApiResponse<IEnumerable<ChatMember>>> GetChatJoinRequestsAsync(
            string chatId, SiteUser user)
        {
            var requests = await _chatMemberRepository.GetGroupChatJoinRequestsAsync(chatId);
            var isAdmin = await IsInRoleRoleAsync(chatId, "admin", user);
            if (isAdmin.IsSuccess)
            {
                if (requests.ToList().Count == 0)
                {
                    return StatusCodeReturn<IEnumerable<ChatMember>>
                        ._200_Success("No join requests found", requests);
                }
                return StatusCodeReturn<IEnumerable<ChatMember>>
                        ._200_Success("Requests found successfully", requests);
            }
            return StatusCodeReturn<IEnumerable<ChatMember>>
                ._403_Forbidden("Admins only can get chat join request");
        }

        private async Task<ApiResponse<ChatMember>> IsAbleToAddMemberToChatAsync(
            Chat chat, SiteUser member, SiteUser admin)
        {
            var policy = await _policyService.GetPolicyByNameAsync("private");
            if (policy.ResponseObject != null)
            {
                if(chat.PolicyId != policy.ResponseObject.Id)
                {
                    if((await IsInRoleRoleAsync(chat.PolicyId, "admin", admin)).IsSuccess)
                    {
                        var isBlocked = await _blockService.GetBlockByUserIdAndBlockedUserIdAsync(
                            admin.Id, member.Id);
                        if (isBlocked.ResponseObject == null)
                        {
                            var isMember = await _chatMemberRepository.GetByMemberAndChatIdAsync(
                                chat.Id, member.Id);
                            if (isMember != null)
                            {
                                if (isMember.IsMember)
                                {
                                    return StatusCodeReturn<ChatMember>
                                        ._403_Forbidden("already group member");
                                }
                                return StatusCodeReturn<ChatMember>
                                            ._200_Success("Allowed", isMember);
                            }
                            return StatusCodeReturn<ChatMember>
                                            ._200_Success("Allowed");
                        }
                        return StatusCodeReturn<ChatMember>
                            ._403_Forbidden();
                    }
                    return StatusCodeReturn<ChatMember>
                            ._403_Forbidden("Admins only can add members");
                }
                return StatusCodeReturn<ChatMember>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<ChatMember>
                    ._404_NotFound("Private policy not found");
        }
        public async Task<ApiResponse<ChatMember>> AddChatMemberAsync(
                AddChatMemberDto addChatMemberDto, SiteUser user)
        {
            var chat = await _chatRepository.GetByIdAsync(addChatMemberDto.ChatId);
            if (chat != null)
            {
                var member = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        addChatMemberDto.UserIdOrNameOrEmail);
                if (member != null)
                {
                    var isAbleToAdd = await IsAbleToAddMemberToChatAsync(chat, user, member);
                    if (isAbleToAdd.IsSuccess)
                    {
                        if (isAbleToAdd.ResponseObject != null)
                        {
                            return await AcceptChatJoinRequestAsync(isAbleToAdd.ResponseObject.Id,
                                user);
                        }
                        var newMember = await _chatMemberRepository.AddAsync(ChatMember(
                            chat.Id, member.Id, null!, true));
                        return StatusCodeReturn<ChatMember>
                                ._201_Created("Member added successfully", newMember);
                    }
                    return isAbleToAdd;
                }
                return StatusCodeReturn<ChatMember>
                            ._404_NotFound("User you want to add to group not found");
            }
            return StatusCodeReturn<ChatMember>
                            ._404_NotFound("Chat not found");
        }

        public async Task<ApiResponse<ChatMember>> DeleteChatMemberAsync(
            string chatMemberId, SiteUser admin)
        {
            var chatMember = await _chatMemberRepository.GetByIdAsync(chatMemberId);
            if (chatMember != null)
            {
                var isAdmin = await IsInRoleRoleAsync(chatMember.ChatId, "admin", admin);
                if (isAdmin.IsSuccess)
                {
                    await _chatMemberRepository.DeleteByIdAsync(chatMemberId);
                    return StatusCodeReturn<ChatMember>
                        ._200_Success("Chat member deleted successfully", chatMember);
                }
                return StatusCodeReturn<ChatMember>
                    ._403_Forbidden("Only admins can kick out members from group");
            }
            return StatusCodeReturn<ChatMember>
                            ._404_NotFound("Chat member not found");
        }

        public async Task<ApiResponse<ChatMember>> GetChatMemberAsync(string chatMemberId,
                SiteUser user)
        {
            var chatMember = await _chatMemberRepository.GetByIdAsync(chatMemberId);
            if (chatMember != null)
            {
                var isChatMember = await _chatMemberRepository.GetByMemberAndChatIdAsync(
                    chatMember.ChatId, user.Id);
                if (isChatMember != null)
                {
                    return StatusCodeReturn<ChatMember>
                        ._200_Success("Chat member found successfully", chatMember);
                }
                return StatusCodeReturn<ChatMember>
                    ._403_Forbidden("Only group chat members can view each other");
            }
            return StatusCodeReturn<ChatMember>
                ._404_NotFound("Chat member not found");
        }

        #endregion



        #region Chat Member Role
        private async Task<ApiResponse<ChatMemberRole>> IsAbleToAssignRoleToMemberAsync(
            string chatId, string roleIdOrName, SiteUser admin, SiteUser member)
        {
            var role = await _rolesService.GetRoleByIdOrNameAsync(roleIdOrName);
            if ((await IsAbleToModifyRoleMemberAsync(chatId, roleIdOrName, member)).IsSuccess)
            {
                if ((await IsInRoleRoleAsync(chatId, "admin", admin)).IsSuccess)
                {
                    if (!(await IsInRoleRoleAsync(chatId, "admin", member)).IsSuccess)
                    {
                        var memberRole = await _chatMemberRoleRepository.GetByChatAndRoleIdAsync(
                            chatId, role.ResponseObject!.Id);
                        if (memberRole == null)
                        {
                            return StatusCodeReturn<ChatMemberRole>
                            ._200_Success("Allowed");
                        }
                        return StatusCodeReturn<ChatMemberRole>
                        ._403_Forbidden("Already in role");
                    }
                    return StatusCodeReturn<ChatMemberRole>
                        ._403_Forbidden("Can't assign roles to admin");
                }
                return await IsInRoleRoleAsync(chatId, "admin", admin);
            }
            return await IsAbleToModifyRoleMemberAsync(chatId, roleIdOrName, member);
        }

        public async Task<ApiResponse<ChatMemberRole>> AssigntMemberToRoleAsync(
            string chatMemberId, string roleIdOrName, SiteUser user)
        {
            var chatMember = await _chatMemberRepository.GetByIdAsync(chatMemberId);
            if (chatMember != null)
            {
                var member = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                    chatMember.MemberId);
                var role = await _rolesService.GetRoleByIdOrNameAsync(roleIdOrName);
                if((await IsAbleToAssignRoleToMemberAsync(chatMember.ChatId,
                    roleIdOrName, user, member)).IsSuccess)
                {
                    var memberRole = await _chatMemberRoleRepository.AddAsync(
                        ChatMemberRole(chatMemberId, role.ResponseObject!.Id));
                    return StatusCodeReturn<ChatMemberRole>
                        ._201_Created("Role assigned to user successfully", memberRole);
                }
                return await IsAbleToAssignRoleToMemberAsync(chatMember.ChatId,
                    roleIdOrName, user, member);
            }
            return StatusCodeReturn<ChatMemberRole>
                            ._404_NotFound("Chat member not found");
        }

        private async Task<ApiResponse<ChatMemberRole>> IsAbleToModifyRoleMemberAsync(
            string chatId, string roleIdOrName, SiteUser member)
        {
            var chat = await _chatRepository.GetByIdAsync(chatId);
            if (chat != null)
            {
                var chatMember = await _chatMemberRepository.GetByMemberAndChatIdAsync(chatId, member.Id);
                if (chatMember != null)
                {
                    var role = await _rolesService.GetRoleByIdOrNameAsync(roleIdOrName);
                    if (role.ResponseObject != null)
                    {
                        return StatusCodeReturn<ChatMemberRole>
                                ._200_Success("Allowed");
                    }
                    return StatusCodeReturn<ChatMemberRole>
                                ._404_NotFound("Role not found");
                }
                return StatusCodeReturn<ChatMemberRole>
                                ._403_Forbidden("Member not in group");
            }
            return StatusCodeReturn<ChatMemberRole>
                                ._404_NotFound("Chat not found");
        }

        private async Task<ApiResponse<ChatMemberRole>> IsAbleToDeleteRoleFromMemberAsync(
            string chatId, string roleIdOrName, SiteUser admin, SiteUser member)
        {
            var role = await _rolesService.GetRoleByIdOrNameAsync(roleIdOrName);
            if ((await IsAbleToModifyRoleMemberAsync(chatId, roleIdOrName, member)).IsSuccess)
            {
                if ((await IsInRoleRoleAsync(chatId, "admin", admin)).IsSuccess)
                {
                    var memberRoles = await GetMemberRolesByChatIdAsync(chatId, member);
                    if (memberRoles.ResponseObject != null)
                    {
                        if (memberRoles.ResponseObject.ToList().Count > 1)
                        {
                            var memberRole = await _chatMemberRoleRepository.GetByChatAndRoleIdAsync(
                                    chatId, role.ResponseObject!.Id);
                            if (memberRole != null)
                            {
                                return StatusCodeReturn<ChatMemberRole>
                                ._200_Success("Allowed", memberRole);
                            }
                            return StatusCodeReturn<ChatMemberRole>
                            ._403_Forbidden("Not in role");
                        }
                        return StatusCodeReturn<ChatMemberRole>
                            ._403_Forbidden();
                    }
                    return StatusCodeReturn<ChatMemberRole>
                            ._404_NotFound("Member has no roles");
                }
                return await IsInRoleRoleAsync(chatId, "admin", admin);
            }
            return await IsAbleToModifyRoleMemberAsync(chatId, roleIdOrName, member);
        }

        public async Task<ApiResponse<ChatMemberRole>> DeleteMemberFromRoleAsync(
            ChatMemberRoleDto chatMemberRoleDto, SiteUser user)
        {
            var chatMember = await _chatMemberRepository.GetByIdAsync(chatMemberRoleDto.ChatMemberId);
            if (chatMember != null)
            {
                var member = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                    chatMember.Id);
                if (member != null)
                {
                    var isAbleToDeleteRole = await IsAbleToDeleteRoleFromMemberAsync(chatMember.ChatId,
                        chatMemberRoleDto.RoleIdOrName, user, member);
                    if (isAbleToDeleteRole.IsSuccess && isAbleToDeleteRole.ResponseObject != null)
                    {
                        await _chatMemberRoleRepository.DeleteByIdAsync(isAbleToDeleteRole.ResponseObject.Id);
                        return StatusCodeReturn<ChatMemberRole>
                            ._200_Success("Role deleted successfully from user", 
                                    isAbleToDeleteRole.ResponseObject);
                    }
                    return isAbleToDeleteRole;
                }
                return StatusCodeReturn<ChatMemberRole>
                            ._404_NotFound("Member you want to delete role from not found");
            }
            return StatusCodeReturn<ChatMemberRole>
                            ._404_NotFound("Chat member not found");
        }

        public async Task<ApiResponse<ChatMemberRole>> DeleteMemberFromRoleAsync(
            string chatMemberRoleId, SiteUser user)
        {
            var chatMemberRole = await _chatMemberRoleRepository.GetByIdAsync(chatMemberRoleId);
            if (chatMemberRole != null)
            {
                return await DeleteMemberFromRoleAsync(new ChatMemberRoleDto
                {
                    ChatMemberId = chatMemberRole.ChatMemberId,
                    RoleIdOrName = chatMemberRole.RoleId
                }, user);
            }
            return StatusCodeReturn<ChatMemberRole>
                ._404_NotFound("Chat member role not found");
        }

        public async Task<ApiResponse<IEnumerable<ChatMemberRole>>> GetMemberRolesByMemberIdAsync(
            string chatMemberId, SiteUser user)
        {
            var memberRoles = await _chatMemberRoleRepository.GetMemberRolesAsync(chatMemberId);
            var chatMember = await _chatMemberRepository.GetByIdAsync(chatMemberId);
            if (chatMember != null)
            {
                if (chatMember.Id == chatMemberId)
                {
                    return StatusCodeReturn<IEnumerable<ChatMemberRole>>
                        ._200_Success("Member roles found successfully", memberRoles);
                }
                return StatusCodeReturn<IEnumerable<ChatMemberRole>>
                           ._403_Forbidden();
            }
            return StatusCodeReturn<IEnumerable<ChatMemberRole>>
                ._404_NotFound("Chat member not found");
        }

        public async Task<ApiResponse<IEnumerable<ChatMemberRole>>> GetMemberRolesByChatIdAsync(
            string chatId, SiteUser user)
        {
            var chatMember = await _chatMemberRepository.GetByMemberAndChatIdAsync(chatId, user.Id);
            if (chatMember != null)
            {
                var memberRoles = await _chatMemberRoleRepository.GetMemberRolesAsync(
                    chatMember.Id);
                return StatusCodeReturn<IEnumerable<ChatMemberRole>>
                    ._200_Success("Member roles found successfully", memberRoles);
            }
            return StatusCodeReturn<IEnumerable<ChatMemberRole>>
                ._404_NotFound("Chat member not found");
        }

        public async Task<ApiResponse<ChatMemberRole>> IsInRoleRoleAsync(
                string chatId, string roleIdOrName, SiteUser user)
        {
            var role = await _rolesService.GetRoleByIdOrNameAsync(roleIdOrName);
            if (role != null && role.ResponseObject != null)
            {
                var chat = await _chatRepository.GetByIdAsync(chatId);
                if (chat != null)
                {
                    var chatMember = await _chatMemberRepository.GetByMemberAndChatIdAsync(chatId, user.Id);
                    if (chatMember != null)
                    {
                        var chatMemberRole = await _chatMemberRoleRepository.GetByChatAndRoleIdAsync(
                            chatMember.Id, role.ResponseObject.Id);
                        if (chatMemberRole != null)
                        {
                            return StatusCodeReturn<ChatMemberRole>
                                ._200_Success("User is in role", chatMemberRole);
                        }
                        return StatusCodeReturn<ChatMemberRole>
                        ._404_NotFound("Not in role");
                    }
                    return StatusCodeReturn<ChatMemberRole>
                            ._404_NotFound("Not member of group");
                }
                return StatusCodeReturn<ChatMemberRole>
                ._404_NotFound("Chat not found");
            }
            return StatusCodeReturn<ChatMemberRole>
                ._404_NotFound("Role not found");
        }

        #endregion


        #region Response Object

        private ChatMemberRole ChatMemberRole(string chatMemberId, string roleId)
        {
            return new ChatMemberRole
            {
                Id = Guid.NewGuid().ToString(),
                ChatMemberId = chatMemberId,
                RoleId = roleId
                
            };
        }

        private Chat ChatObject(AddChatDto addChatDto, SiteUser user, string policyId)
        {
            if (user != null && addChatDto != null)
            {
                return new Chat
                {
                    Name = addChatDto.Name,
                    Id = Guid.NewGuid().ToString(),
                    CreatorId = user.Id,
                    Description = addChatDto.Description,
                    PolicyId = policyId
                };
            }
            else if (user == null && addChatDto == null)
            {
                return new Chat
                {
                    Id = Guid.NewGuid().ToString(),
                    PolicyId = policyId
                };
            }
            else if (user == null && addChatDto != null)
            {
                return new Chat
                {
                    Name = addChatDto.Name,
                    Id = Guid.NewGuid().ToString(),
                    Description = addChatDto.Description,
                    PolicyId = policyId
                };
            }
            return new Chat
            {
                Name = addChatDto!.Name,
                Id = Guid.NewGuid().ToString(),
                Description = addChatDto.Description,
                PolicyId = policyId
            };
        }

        private ChatMember ChatMember(string chatId, string member1Id, string member2Id, bool b)
        {
            if (member2Id != null)
            {
                return new ChatMember
                {
                    Id = Guid.NewGuid().ToString(),
                    ChatId = chatId,
                    MemberId = member1Id,
                    IsMember = b
                };
            }
            return new ChatMember
            {
                Id = Guid.NewGuid().ToString(),
                ChatId = chatId,
                MemberId = member1Id,
                IsMember = b
            };
        }

        private PrivateChat AddPrivateChat(string chatId, string member1Id, string member2Id)
        {

            return new PrivateChat
            {
                Id = Guid.NewGuid().ToString(),
                ChatId = chatId,
                User1Id = member1Id,
                User2Id = member2Id,
                IsAccepted = false,
                IsBlocked = false,
                IsBlockedByUser1 = false,
                IsBlockedByUser2 = false
            };
        }

        #endregion


    }
}
