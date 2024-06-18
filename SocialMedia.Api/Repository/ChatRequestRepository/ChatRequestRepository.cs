

using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Repository.ChatRequestRepository
{
    public class ChatRequestRepository : IChatRequestRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ChatRequestRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<ChatRequest> AddAsync(ChatRequest t)
        {
            await _dbContext.ChatRequests.AddAsync(t);
            await SaveChangesAsync();
            return new ChatRequest
            {
                Id = t.Id,
                UserWhoReceivedRequestId = t.UserWhoReceivedRequestId,
                UserWhoSentRequestId = t.UserWhoSentRequestId,
                SentAt = t.SentAt
            };
        }

        public async Task<ChatRequest> DeleteByIdAsync(string id)
        {
            var chatRequest = await GetByIdAsync(id);
            _dbContext.ChatRequests.Remove(chatRequest);
            await SaveChangesAsync();
            return chatRequest;
        }

        public async Task<IEnumerable<ChatRequest>> GetAllAsync()
        {
            return await _dbContext.ChatRequests.Select(e => new ChatRequest
            {
                Id = e.Id,
                UserWhoReceivedRequestId = e.UserWhoReceivedRequestId,
                UserWhoSentRequestId = e.UserWhoSentRequestId,
                SentAt = e.SentAt
            }).ToListAsync();
        }

        public async Task<ChatRequest> GetByIdAsync(string id)
        {
            return (await _dbContext.ChatRequests.Select(e => new ChatRequest
            {
                UserWhoSentRequestId = e.UserWhoSentRequestId,
                Id = e.Id,
                UserWhoReceivedRequestId = e.UserWhoReceivedRequestId,
                SentAt = e.SentAt
            }).Where(e => e.Id == id).FirstOrDefaultAsync())!;
        }

        public async Task<IEnumerable<ChatRequest>> GetReceivedChatRequestsAsync(SiteUser user)
        {
            return from c in await GetAllAsync()
                   where c.UserWhoReceivedRequestId == user.Id
                   select (new ChatRequest
                   {
                       Id = c.Id,
                       UserWhoReceivedRequestId = c.UserWhoReceivedRequestId,
                       UserWhoSentRequestId = c.UserWhoSentRequestId
                   });
        }

        public async Task<ChatRequest> GetChatRequestAsync(SiteUser user1, SiteUser user2)
        {
            var case1 = (await _dbContext.ChatRequests.Select(e => new ChatRequest
            {
                UserWhoSentRequestId = e.UserWhoSentRequestId,
                Id = e.Id,
                UserWhoReceivedRequestId = e.UserWhoReceivedRequestId,
                SentAt = e.SentAt
            }).Where(e => e.UserWhoReceivedRequestId == user1.Id)
            .Where(e => e.UserWhoSentRequestId == user2.Id).FirstOrDefaultAsync())!;
            var case2 = (await _dbContext.ChatRequests.Select(e => new ChatRequest
            {
                UserWhoSentRequestId = e.UserWhoSentRequestId,
                Id = e.Id,
                UserWhoReceivedRequestId = e.UserWhoReceivedRequestId,
                SentAt = e.SentAt
            }).Where(e => e.UserWhoReceivedRequestId == user2.Id)
            .Where(e => e.UserWhoSentRequestId == user1.Id).FirstOrDefaultAsync())!;
            return case1 == null ? case2! : case1;
        }

        public async Task<IEnumerable<ChatRequest>> GetSentChatRequestsAsync(SiteUser user)
        {
            return from c in await GetAllAsync()
                   where c.UserWhoSentRequestId == user.Id
                   select (new ChatRequest
                   {
                       Id = c.Id,
                       UserWhoReceivedRequestId = c.UserWhoReceivedRequestId,
                       UserWhoSentRequestId = c.UserWhoSentRequestId,
                       SentAt = c.SentAt
                   });
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ChatRequest> UpdateAsync(ChatRequest t)
        {
            return await DeleteByIdAsync(t.Id);
        }
    }
}
