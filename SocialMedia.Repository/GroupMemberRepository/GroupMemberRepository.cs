

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;
using System.Text.RegularExpressions;

namespace SocialMedia.Repository.GroupMemberRepository
{
    public class GroupMemberRepository : IGroupMemberRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public GroupMemberRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<GroupMember> AddGroupMemberAsync(GroupMember groupMember)
        {
            try
            {
                await _dbContext.GroupMembers.AddAsync(groupMember);
                await SaveChangesAsync();
                return groupMember;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupMember> DeleteGroupMemberAsync(string userId, string groupId)
        {
            var groupMember = await GetGroupMemberAsync(userId, groupId);
            _dbContext.GroupMembers.Remove(groupMember);
            await SaveChangesAsync();
            return groupMember;
        }

        public async Task<GroupMember> DeleteGroupMemberAsync(string groupMemberId)
        {
            var groupMember = await GetGroupMemberAsync(groupMemberId);
            _dbContext.GroupMembers.Remove(groupMember);
            await SaveChangesAsync();
            return groupMember;
        }

        public async Task<GroupMember> GetGroupMemberAsync(string userId, string groupId)
        {
            return (await _dbContext.GroupMembers.Where(e => e.GroupId == groupId)
                .Where(e => e.MemberId == userId).FirstOrDefaultAsync())!;
        }

        public async Task<GroupMember> GetGroupMemberAsync(string groupMemberId)
        {
            return (await _dbContext.GroupMembers.Where(e => e.Id == groupMemberId).FirstOrDefaultAsync())!;
        }

        public async Task<IEnumerable<GroupMember>> GetGroupMembersAsync(string groupId)
        {
            return from g in await _dbContext.GroupMembers.Distinct().ToListAsync()
                   where g.GroupId == groupId
                   select g;
        }

        public async Task<IEnumerable<GroupMember>> GetUserGroupsAsync(string userId)
        {
            return from g in await _dbContext.GroupMembers.ToListAsync()
                   where g.MemberId == userId
                   select g;
        }

        public async Task<IEnumerable<GroupMember>> GetUserJoinedGroupsAsync(string userId)
        {
            return from g in await _dbContext.GroupMembers.ToListAsync()
                   where g.MemberId == userId
                   select g;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
