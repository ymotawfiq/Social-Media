

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

        public async Task<GroupMember> AddAsync(GroupMember t)
        {
            try
            {
                await _dbContext.GroupMembers.AddAsync(t);
                await SaveChangesAsync();
                return new GroupMember
                {
                    Id = t.Id,
                    GroupId = t.GroupId,
                    MemberId = t.MemberId
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupMember> DeleteByIdAsync(string id)
        {
            try
            {
                var groupMember = await GetByIdAsync(id);
                _dbContext.GroupMembers.Remove(groupMember);
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


        public async Task<IEnumerable<GroupMember>> GetAllAsync()
        {
            return await _dbContext.GroupMembers.Select(e=>new GroupMember
            {
                GroupId = e.GroupId,
                Id = e.Id,
                MemberId = e.MemberId
            }).ToListAsync();
        }

        public async Task<GroupMember> GetByIdAsync(string id)
        {
            try
            {
                return (await _dbContext.GroupMembers.Select(e => new GroupMember
                {
                    GroupId = e.GroupId,
                    Id = e.Id,
                    MemberId = e.MemberId
                }).Where(e => e.Id == id).FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupMember> GetGroupMemberAsync(string userId, string groupId)
        {
            return (await _dbContext.GroupMembers.Select(e=>new GroupMember
            {
                GroupId = e.GroupId,
                Id = e.Id,
                MemberId = e.MemberId
            }).Where(e => e.GroupId == groupId)
                .Where(e => e.MemberId == userId).FirstOrDefaultAsync())!;
        }


        public async Task<IEnumerable<GroupMember>> GetGroupMembersAsync(string groupId)
        {
            return from g in await GetAllAsync()
                   where g.GroupId == groupId
                   select (new GroupMember
                   {
                       MemberId = g.MemberId,
                       Id = g.Id,
                       GroupId = g.GroupId
                   });
        }

        public async Task<IEnumerable<GroupMember>> GetUserGroupsAsync(string userId)
        {
            return from g in await GetAllAsync()
                   where g.MemberId == userId
                   select (new GroupMember
                   {
                       MemberId = g.MemberId,
                       Id = g.Id,
                       GroupId = g.GroupId
                   });
        }

        public async Task<IEnumerable<GroupMember>> GetUserJoinedGroupsAsync(string userId)
        {
            return from g in await GetAllAsync()
                   where g.MemberId == userId
                   select (new GroupMember
                   {
                       MemberId = g.MemberId,
                       Id = g.Id,
                       GroupId = g.GroupId
                   });
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<GroupMember> UpdateAsync(GroupMember t)
        {
            return await DeleteByIdAsync(t.Id);
        }
    }
}
