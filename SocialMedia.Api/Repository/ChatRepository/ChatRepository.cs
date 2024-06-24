using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.PolicyRepository;
using System;

namespace SocialMedia.Api.Repository.ChatRepository
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ChatRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<Chat> AddAsync(Chat t)
        {
            await _dbContext.Chat.AddAsync(t);
            await SaveChangesAsync();
            return new Chat
            {
                Id = t.Id,
                CreatorId = t.CreatorId,
                Description = t.Description,
                Name = t.Name,
                PolicyId = t.PolicyId,
                Policy = t.Policy,
                User = t.User
            };
        }

        public async Task<Chat> DeleteByIdAsync(string id)
        {
            var chat = await GetByIdAsync(id);
            _dbContext.Chat.Remove(chat);
            await SaveChangesAsync();
            return new Chat
            {
                Id = chat.Id,
                CreatorId = chat.CreatorId,
                Description = chat.Description,
                Name = chat.Name,
                PolicyId = chat.PolicyId
            };
        }

        public async Task<IEnumerable<Chat>> GetAllAsync()
        {
            return await _dbContext.Chat.Select(e=>new Chat
            {
                PolicyId = e.PolicyId,
                CreatorId = e.CreatorId,
                Description = e.Description,
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();
        }

        public async Task<Chat> GetByIdAsync(string id)
        {
            var chat = (await _dbContext.Chat.Select(e => new Chat
            {
                PolicyId = e.PolicyId,
                CreatorId = e.CreatorId,
                Description = e.Description,
                Id = e.Id,
                Name = e.Name
            }).Where(e => e.Id == id).FirstOrDefaultAsync())!;
            return chat;
        }

        public async Task<IEnumerable<Chat>> GetUserCreatedChatsAsync(string userId)
        {
            return
                from c in await GetAllAsync()
                where c.CreatorId == userId
                select (new Chat
                {
                    Id = c.Id,
                    CreatorId = c.CreatorId,
                    Description = c.Description,
                    Name = c.Name,
                    PolicyId = c.PolicyId
                });
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Chat> UpdateAsync(Chat t)
        {
            var chat = await GetByIdAsync(t.Id);
            chat.Name = t.Name;
            chat.Description = t.Description;
            await SaveChangesAsync();
            return new Chat
            {
                Id = chat.Id,
                CreatorId = chat.CreatorId,
                Description = chat.Description,
                Name = chat.Name,
                PolicyId = chat.PolicyId
            };
        }


    }
}
