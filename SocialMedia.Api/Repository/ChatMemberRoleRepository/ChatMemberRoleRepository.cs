using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Repository.ChatMemberRoleRepository
{
    public class ChatMemberRoleRepository : IChatMemberRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ChatMemberRoleRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<ChatMemberRole> AddAsync(ChatMemberRole t)
        {
            await _dbContext.ChatMemberRole.AddAsync(t);
            await SaveChangesAsync();
            return new ChatMemberRole
            {
                Id = t.Id,
                RoleId = t.RoleId,
                ChatMemberId = t.ChatMemberId
            };
        }

        public async Task<ChatMemberRole> DeleteByIdAsync(string id)
        {
            var chatMemberRole = await GetByIdAsync(id);
            _dbContext.ChatMemberRole.Remove(chatMemberRole);
            await SaveChangesAsync();
            return new ChatMemberRole
            {
                Id = chatMemberRole.Id,
                RoleId = chatMemberRole.RoleId,
                ChatMemberId = chatMemberRole.ChatMemberId
            };
        }

        public async Task<IEnumerable<ChatMemberRole>> GetAllAsync()
        {
            return await _dbContext.ChatMemberRole.Select(e=>new ChatMemberRole
            {
                Id = e.Id,
                RoleId = e.RoleId,
                ChatMemberId = e.ChatMemberId
            }).ToListAsync();
        }

        public async Task<ChatMemberRole> GetByChatAndRoleIdAsync(
            string chatMemberId, string roleId)
        {
            var chatMemberRole = await _dbContext.ChatMemberRole.Select(e => new ChatMemberRole
            {
                Id = e.Id,
                RoleId = e.RoleId,
                ChatMemberId = e.ChatMemberId
            }).Where(e => e.RoleId == roleId).Where(e=>e.ChatMemberId == chatMemberId).FirstOrDefaultAsync();
            return chatMemberRole!;
        }

        public async Task<ChatMemberRole> GetByIdAsync(string id)
        {
            var chatMemberRole = await _dbContext.ChatMemberRole.Select(e => new ChatMemberRole
            {
                Id = e.Id,
                RoleId = e.RoleId,
                ChatMemberId = e.ChatMemberId
            }).Where(e => e.Id == id).FirstOrDefaultAsync();
            return chatMemberRole!;
        }

        public async Task<IEnumerable<ChatMemberRole>> GetMemberRolesAsync(string chatMemberId)
        {
            return 
                from c in await GetAllAsync()
                where c.ChatMemberId == chatMemberId
                select(new ChatMemberRole
                {
                    ChatMemberId = c.ChatMemberId,
                    RoleId = c.RoleId,
                    Id = c.Id
                });
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ChatMemberRole> UpdateAsync(ChatMemberRole t)
        {
            return await DeleteByIdAsync(t.Id);
        }



    }
}
