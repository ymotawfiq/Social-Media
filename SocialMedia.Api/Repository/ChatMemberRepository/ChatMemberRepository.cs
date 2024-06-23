using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.ChatRepository;

namespace SocialMedia.Api.Repository.ChatMemberRepository
{
    public class ChatMemberRepository : IChatMemberRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ChatMemberRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<ChatMember> AddAsync(ChatMember t)
        {
            await _dbContext.ChatMember.AddAsync(t);
            await SaveChangesAsync();
            return ChatMember(t);
        }

        public async Task<ChatMember> DeleteByIdAsync(string id)
        {
            var chatMember = await GetByIdAsync(id);
            _dbContext.ChatMember.Remove(chatMember);
            await SaveChangesAsync();
            return ChatMember(chatMember);
        }

        public async Task<IEnumerable<ChatMember>> GetAllAsync()
        {
            return await _dbContext.ChatMember.Select(e => new ChatMember
            {
                Id = e.Id,
                ChatId = e.ChatId,
                IsMember = e.IsMember,
                MemberId = e.MemberId
            }).ToListAsync();
        }

        public async Task<ChatMember> GetByIdAsync(string id)
        {
            var chatMember = await _dbContext.ChatMember.Select(e => new ChatMember
            {
                Id = e.Id,
                ChatId = e.ChatId,
                IsMember = e.IsMember,
                MemberId = e.MemberId
            }).Where(e => e.Id == id).FirstOrDefaultAsync();
            return chatMember!;
        }

        public async Task<IEnumerable<ChatMember>> GetChatMembersAsync(string chatId)
        {
            return
                from t in await GetAllAsync()
                where t.ChatId == chatId && t.IsMember
                select (new ChatMember
                {
                    ChatId = t.ChatId,
                    Id = t.Id,
                    IsMember = t.IsMember,
                    MemberId = t.MemberId,
                });
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ChatMember> UpdateAsync(ChatMember t)
        {
            var chatMember = await GetByIdAsync(t.Id);
            chatMember.IsMember = t.IsMember;
            _dbContext.Update(chatMember);
            await SaveChangesAsync();
            return ChatMember(chatMember);
        }


        public async Task<ChatMember> GetByMemberAndChatIdAsync(string chatId, string MemberId)
        {
            return (await _dbContext.ChatMember.Select(e => new ChatMember
            {
                Id = e.Id,
                ChatId = e.ChatId,
                IsMember = e.IsMember,
                MemberId = e.MemberId
            }).Where(e => e.ChatId == chatId).Where(e=>e.MemberId == MemberId).FirstOrDefaultAsync())!;
        }



        public async Task<IEnumerable<ChatMember>> GetGroupChatJoinRequestsAsync(string chatId)
        {
            return
                from t in await GetAllAsync()
                where t.ChatId == chatId && !t.IsMember
                select (new ChatMember
                {
                    ChatId = t.ChatId,
                    Id = t.Id,
                    IsMember = t.IsMember,
                    MemberId = t.MemberId,
                });
        }

        public async Task<IEnumerable<ChatMember>> GetNotAcceptedGroupChatRequestsAsync(string userId)
        {
            return from c in await GetAllAsync()
                   where c.MemberId == userId && !c.IsMember
                   select (new ChatMember
                   {
                       ChatId = c.ChatId,
                       Id = c.Id,
                       IsMember = c.IsMember,
                       MemberId = c.MemberId,
                   });
        }

        private ChatMember ChatMember(ChatMember t)
        {
            if (t != null)
            {
                return new ChatMember
                {
                    ChatId = t.ChatId,
                    Id = t.Id,
                    IsMember = t.IsMember,
                    MemberId = t.MemberId,
                    Chat = Chat(t.ChatId),
                    User = User(t.MemberId),
                };
            }
            return null!;
        }

        private Chat Chat(string chatId)
        {
            if (chatId != null)
            {
                return (_dbContext.Chat.Select(e => new Chat
                {
                    Id = e.Id,
                    CreatorId = e.CreatorId,
                    Name = e.Name,
                    Description = e.Description,
                    PolicyId = e.PolicyId,
                }).FirstOrDefault(e => e.Id == chatId))!;
            }
            return null!;
        }

        private SiteUser User(string userId)
        {
            if (userId != null)
            {
                var user = _dbContext.Users.FirstOrDefault(e => e.Id == userId)!;
                return new SiteUser
                {
                    Id = userId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DisplayName = user.DisplayName
                };
            }
            return null!;
        }


    }
}
