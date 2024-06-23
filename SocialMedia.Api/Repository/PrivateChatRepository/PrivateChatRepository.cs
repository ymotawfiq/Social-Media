using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Repository.PrivateChatRepository
{
    public class PrivateChatRepository : IPrivateChatRepository
    {

        private readonly ApplicationDbContext _dbContext;
        public PrivateChatRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<PrivateChat> AddAsync(PrivateChat t)
        {
            await _dbContext.PrivateChat.AddAsync(t);
            await SaveChangesAsync();
            return new PrivateChat
            {
                ChatId = t.ChatId,
                Id = t.Id,
                IsAccepted = t.IsAccepted,
                IsBlocked = t.IsBlocked,
                IsBlockedByUser1 = t.IsBlockedByUser1,
                IsBlockedByUser2 = t.IsBlockedByUser2,
                User1Id = t.User1Id,
                User2Id = t.User2Id
            };
        }

        public async Task<PrivateChat> DeleteByIdAsync(string id)
        {
            var chat = await GetByIdAsync(id);
            _dbContext.PrivateChat.Remove(chat);
            await SaveChangesAsync();
            return chat;
        }

        public async Task<IEnumerable<PrivateChat>> GetAllAsync()
        {
            return await _dbContext.PrivateChat.Select(e => new PrivateChat
            {
                ChatId = e.ChatId,
                Id = e.Id,
                IsAccepted = e.IsAccepted,
                IsBlocked = e.IsBlocked,
                IsBlockedByUser1 = e.IsBlockedByUser1,
                IsBlockedByUser2 = e.IsBlockedByUser2,
                User1Id = e.User1Id,
                User2Id = e.User2Id
            }).ToListAsync();
        }

        public async Task<PrivateChat> GetByChatIdAsync(string chatId)
        {
            return (await _dbContext.PrivateChat.Select(e => new PrivateChat
            {
                ChatId = e.ChatId,
                Id = e.Id,
                IsAccepted = e.IsAccepted,
                IsBlocked = e.IsBlocked,
                IsBlockedByUser1 = e.IsBlockedByUser1,
                IsBlockedByUser2 = e.IsBlockedByUser2,
                User1Id = e.User1Id,
                User2Id = e.User2Id
            }).Where(e => e.ChatId == chatId).FirstOrDefaultAsync())!;
        }

        public async Task<PrivateChat> GetByIdAsync(string id)
        {
            return (await _dbContext.PrivateChat.Select(e => new PrivateChat
            {
                ChatId = e.ChatId,
                Id = e.Id,
                IsAccepted = e.IsAccepted,
                IsBlocked = e.IsBlocked,
                IsBlockedByUser1 = e.IsBlockedByUser1,
                IsBlockedByUser2 = e.IsBlockedByUser2,
                User1Id = e.User1Id,
                User2Id = e.User2Id
            }).Where(e => e.Id == id).FirstOrDefaultAsync())!;
        }

        public async Task<PrivateChat> GetByMember1AndMember2IdAsync(string member1Id, string member2Id)
        {
            var case1 =
            (await _dbContext.PrivateChat.Select(e => new PrivateChat
            {
                ChatId = e.ChatId,
                Id = e.Id,
                IsAccepted = e.IsAccepted,
                IsBlocked = e.IsBlocked,
                IsBlockedByUser1 = e.IsBlockedByUser1,
                IsBlockedByUser2 = e.IsBlockedByUser2,
                User1Id = e.User1Id,
                User2Id = e.User2Id
            }).Where(e => e.User1Id == member1Id).Where(e=>e.User2Id==member2Id).FirstOrDefaultAsync())!;
            var case2 =
            (await _dbContext.PrivateChat.Select(e => new PrivateChat
            {
                ChatId = e.ChatId,
                Id = e.Id,
                IsAccepted = e.IsAccepted,
                IsBlocked = e.IsBlocked,
                IsBlockedByUser1 = e.IsBlockedByUser1,
                IsBlockedByUser2 = e.IsBlockedByUser2,
                User1Id = e.User1Id,
                User2Id = e.User2Id
            }).Where(e => e.User1Id == member2Id).Where(e => e.User2Id == member1Id).FirstOrDefaultAsync())!;
            return case1 == null ? case2! : case1;
        }

        public async Task<PrivateChat> GetByMemberAndChatIdAsync(string chatId, string memberId)
        {
            var case1 =  (await _dbContext.PrivateChat.Select(e => new PrivateChat
            {
                ChatId = e.ChatId,
                Id = e.Id,
                IsAccepted = e.IsAccepted,
                IsBlocked = e.IsBlocked,
                IsBlockedByUser1 = e.IsBlockedByUser1,
                IsBlockedByUser2 = e.IsBlockedByUser2,
                User1Id = e.User1Id,
                User2Id = e.User2Id
            }).Where(e => e.ChatId == chatId).Where(e => e.User2Id == memberId).FirstOrDefaultAsync())!;
            var case2 = (await _dbContext.PrivateChat.Select(e => new PrivateChat
            {
                ChatId = e.ChatId,
                Id = e.Id,
                IsAccepted = e.IsAccepted,
                IsBlocked = e.IsBlocked,
                IsBlockedByUser1 = e.IsBlockedByUser1,
                IsBlockedByUser2 = e.IsBlockedByUser2,
                User1Id = e.User1Id,
                User2Id = e.User2Id
            }).Where(e => e.ChatId == chatId).Where(e => e.User1Id == memberId).FirstOrDefaultAsync())!;
            return case1 == null ? case2! : case1;
        }

        public async Task<IEnumerable<PrivateChat>> GetReceivedChatRequestsAsync(string user2Id)
        {
            return
                from p in await GetAllAsync()
                where p.User2Id == user2Id && !p.IsAccepted
                select (new PrivateChat
                {
                    ChatId = p.ChatId,
                    Id = p.Id,
                    IsAccepted = p.IsAccepted,
                    IsBlocked = p.IsBlocked,
                    IsBlockedByUser1 = p.IsBlockedByUser1,
                    IsBlockedByUser2 = p.IsBlockedByUser2,
                    User1Id = p.User1Id,
                    User2Id = p.User2Id
                });
        }

        public async Task<IEnumerable<PrivateChat>> GetSentChatRequestsAsync(string user1Id)
        {
            return
                from p in await GetAllAsync()
                where p.User1Id == user1Id && !p.IsAccepted
                select (new PrivateChat
                {
                    ChatId = p.ChatId,
                    Id = p.Id,
                    IsAccepted = p.IsAccepted,
                    IsBlocked = p.IsBlocked,
                    IsBlockedByUser1 = p.IsBlockedByUser1,
                    IsBlockedByUser2 = p.IsBlockedByUser2,
                    User1Id = p.User1Id,
                    User2Id = p.User2Id
                });
        }

        public async Task<IEnumerable<PrivateChat>> GetUserChatsAsync(string userId)
        {
            return
                from p in await GetAllAsync()
                where (p.User1Id == userId || p.User2Id == userId) && p.IsAccepted
                select (new PrivateChat
                {
                    ChatId = p.ChatId,
                    Id = p.Id,
                    IsAccepted = p.IsAccepted,
                    IsBlocked = p.IsBlocked,
                    IsBlockedByUser1 = p.IsBlockedByUser1,
                    IsBlockedByUser2 = p.IsBlockedByUser2,
                    User1Id = p.User1Id,
                    User2Id = p.User2Id
                });
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PrivateChat> UpdateAsync(PrivateChat t)
        {
            var chat = await GetByIdAsync(t.Id);
            chat.IsAccepted = t.IsAccepted;
            chat.IsBlocked = t.IsBlocked;
            chat.IsBlockedByUser1 = t.IsBlockedByUser1;
            chat.IsBlockedByUser2 = t.IsBlockedByUser2;
            await SaveChangesAsync();
            return new PrivateChat
            {
                ChatId = chat.ChatId,
                Id = chat.Id,
                IsAccepted = chat.IsAccepted,
                IsBlocked = chat.IsBlocked,
                IsBlockedByUser1 = chat.IsBlockedByUser1,
                IsBlockedByUser2 = chat.IsBlockedByUser2,
                User1Id = chat.User1Id,
                User2Id = chat.User2Id
            };
        }
    }
}
