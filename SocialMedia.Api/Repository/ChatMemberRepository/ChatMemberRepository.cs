using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;

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
            return new ChatMember
            {
                ChatId = t.ChatId,
                Id = t.Id,
                IsMember = t.IsMember,
                Member1Id = t.Member1Id,
                Member2Id = t.Member2Id
            };
        }

        public async Task<ChatMember> DeleteByIdAsync(string id)
        {
            var chatMember = await GetByIdAsync(id);
            _dbContext.ChatMember.Remove(chatMember);
            await SaveChangesAsync();
            return new ChatMember
            {
                Id = chatMember.Id,
                ChatId = chatMember.ChatId,
                IsMember = chatMember.IsMember,
                Member1Id = chatMember.Member1Id,
                Member2Id = chatMember.Member2Id
            };
        }

        public async Task<IEnumerable<ChatMember>> GetAllAsync()
        {
            return await _dbContext.ChatMember.Select(e => new ChatMember
            {
                ChatId = e.ChatId,
                Member2Id = e.Member2Id,
                Member1Id = e.Member1Id,
                IsMember = e.IsMember,
                Id = e.Id
            }).ToListAsync();
        }

        public async Task<ChatMember> GetByIdAsync(string id)
        {
            var chatMember = await _dbContext.ChatMember.Select(e => new ChatMember
            {
                ChatId = e.ChatId,
                Member2Id = e.Member2Id,
                Member1Id = e.Member1Id,
                IsMember = e.IsMember,
                Id = e.Id
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
                    Member1Id = t.Member1Id,
                    Member2Id = t.Member2Id
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
            return new ChatMember
            {
                ChatId = chatMember.ChatId,
                Id = chatMember.Id,
                IsMember = chatMember.IsMember,
                Member1Id = chatMember.Member1Id,
                Member2Id = chatMember.Member2Id
            };
        }


        public async Task<ChatMember> GetByMemberAndChatIdAsync(string chatId, string MemberId)
        {
            var chatMember1 = await _dbContext.ChatMember.Select(e=>new ChatMember
            {
                ChatId = e.ChatId,
                Member2Id = e.Member2Id,
                Member1Id = e.Member1Id,
                IsMember = e.IsMember,
                Id = e.Id
            }).Where(e => e.ChatId == chatId).Where(e=>e.Member1Id == MemberId).FirstOrDefaultAsync();
            var chatMember2 = await _dbContext.ChatMember.Select(e => new ChatMember
            {
                ChatId = e.ChatId,
                Member2Id = e.Member2Id,
                Member1Id = e.Member1Id,
                IsMember = e.IsMember,
                Id = e.Id
            }).Where(e => e.ChatId == chatId).Where(e => e.Member2Id == MemberId).FirstOrDefaultAsync();
            chatMember1 = chatMember1 == null ? chatMember2 : chatMember1;
            return chatMember1!;
        }

        public async Task<bool> AddRangeAsync(List<ChatMember> members)
        {
            var list = new List<ChatMember>();
            for(int i=0; i<members.Count; i++)
            {
                list.Add(members.ElementAt(i));
            }
            await _dbContext.AddRangeAsync(list);
            await SaveChangesAsync();
            return true;
        }

        public async Task<ChatMember> GetByMember1AndMember2IdAsync(string member1Id,
            string member2Id)
        {
            var chatMember1 = await _dbContext.ChatMember.Select(e => new ChatMember
            {
                ChatId = e.ChatId,
                Member2Id = e.Member2Id,
                Member1Id = e.Member1Id,
                IsMember = e.IsMember,
                Id = e.Id
            }).Where(e => e.Member1Id == member1Id).Where(e => e.Member2Id == member2Id).FirstOrDefaultAsync();
            var chatMember2 = await _dbContext.ChatMember.Select(e => new ChatMember
            {
                ChatId = e.ChatId,
                Member2Id = e.Member2Id,
                Member1Id = e.Member1Id,
                IsMember = e.IsMember,
                Id = e.Id
            }).Where(e => e.Member1Id == member2Id).Where(e => e.Member2Id == member1Id).FirstOrDefaultAsync();
            chatMember1 = chatMember1 == null ? chatMember2 : chatMember1;
            return chatMember1!;
        }

        public async Task<IEnumerable<ChatMember>> GetPrivateChatRequestsAsync(string member2Id)
        {
            return
                from t in await GetAllAsync()
                where t.Member2Id == member2Id && !t.IsMember
                select (new ChatMember
                {
                    ChatId = t.ChatId,
                    Id = t.Id,
                    IsMember = t.IsMember,
                    Member1Id = t.Member1Id,
                    Member2Id = t.Member2Id
                });
        }

        public async Task<IEnumerable<ChatMember>> GetPrivateChatsAsync(string userId)
        {
            return
                from t in await GetAllAsync()
                where (t.Member1Id == userId || t.Member2Id == userId)  && t.IsMember
                select (new ChatMember
                {
                    ChatId = t.ChatId,
                    Id = t.Id,
                    IsMember = t.IsMember,
                    Member1Id = t.Member1Id,
                    Member2Id = t.Member2Id
                });
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
                    Member1Id = t.Member1Id,
                    Member2Id = t.Member2Id
                });
        }


    }
}
