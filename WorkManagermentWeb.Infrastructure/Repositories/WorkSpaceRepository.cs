using Microsoft.EntityFrameworkCore;
using WorkManagermentWeb.Application.Constants;
using WorkManagermentWeb.Application.Contracts;
using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Domain.Entities;
using WorkManagermentWeb.Domain.Enums;
using WorkManagermentWeb.EventHandler.Events;
using WorkManagermentWeb.EventHandler.Services;
using WorkManagermentWeb.Infrastructure.Data;

namespace WorkManagermentWeb.Infrastructure.Repositories
{
    /// <summary>
    /// WorkSpaceRepository
    /// </summary>
    /// <param name="context"></param>
    /// <param name="eventPusher"></param>
    public class WorkSpaceRepository(AppDBContext context, IEventPusher eventPusher) : IWorkSpace
    {
        /// <summary>
        /// AddAsync
        /// </summary>
        /// <param name="workSpaceDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostPutResponse> AddAsync(WorkSpaceDTO workSpaceDTO)
        {
            ApplicationUser? foundOwner = await GetUserByIdAsync(workSpaceDTO.Owner);
            if (foundOwner is null)
            {
                return new PostPutResponse(false, HttpConstants.UserNotFound);
            }
            WorkSpace newWorkSpace = new WorkSpace()
            {
                Name = workSpaceDTO.Name,
                Description = workSpaceDTO.Description,
                Owner = foundOwner.FullName,
                WorkSpaceUsers = new List<WorkSpaceUser>()
                {
                    new WorkSpaceUser()
                    {
                        UserId = foundOwner.Id,
                        WorkSpaceId = workSpaceDTO.Id
                    }
                }
            };
            context.WorkSpaces.Add(newWorkSpace);
            await context.SaveChangesAsync();
            return new PostPutResponse(false, HttpConstants.WorkSpaceCreatedSuccessfully);
        }

        /// <summary>
        /// AddMemberAsync
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="memberId"></param>
        /// <param name="updaterId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostPutResponse> AddMemberAsync(Guid workspaceId, string memberId, string updaterId)
        {
            ApplicationUser? foundMember = await GetUserByIdAsync(memberId);
            if (foundMember is null)
            {
                return new PostPutResponse(false, HttpConstants.UserNotFound);
            }
            WorkSpace? foundWorkSpace = await GetRecordByIdAsync(workspaceId);
            if (foundWorkSpace is null)
            {
                return new PostPutResponse(false, HttpConstants.WorkSpaceNotFound);
            }
            ApplicationUser? foundUpdater = await GetUserByIdAsync(updaterId);

            if (!foundWorkSpace.WorkSpaceUsers.Any(_ => _.UserId == memberId))
            {
                WorkSpaceUser workSpaceUser = new WorkSpaceUser()
                {
                    UserId = memberId,
                    WorkSpaceId = workspaceId
                };
                context.WorkSpaceUsers.Add(workSpaceUser);
                await context.SaveChangesAsync();
                await eventPusher.Publish(new WorkSpaceMemberAdd
                {
                    ObjectId = workspaceId.ToString(),
                    WorkSpaceName = foundWorkSpace.Name,
                    UpdaterName = foundUpdater!.FullName,
                    RecieverId = memberId
                });
            }
            return new PostPutResponse(true, HttpConstants.WorkSpaceAddedMember);
        }

        /// <summary>
        /// GetByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetWorkspaceResponse> GetByIdAsync(Guid id)
        {
            WorkSpace? foundWorkSpace = await GetRecordByIdAsync(id);
            if (foundWorkSpace is null)
            {
                return new GetWorkspaceResponse(false, HttpConstants.WorkSpaceNotFound);
            }

            List<string> uids = foundWorkSpace.WorkSpaceUsers.Select(_ => _.UserId).ToList();
            List<UserDTO> users = await context.Users
                .Where(u => uids.Any(id => id == u.Id))
                .Select(_ => new UserDTO
                {
                    Id = _.Id,
                    Email = _.Email!,
                    FullName = _.FullName
                }).ToListAsync();
            WorkSpaceDTO workSpaceDTO = new WorkSpaceDTO()
            {
                Id = foundWorkSpace.Id,
                Name = foundWorkSpace.Name,
                Description = foundWorkSpace.Description,
                UpdatedAt = foundWorkSpace.UpdatedAt,
                CreatedAt = foundWorkSpace.CreatedAt,
                Status = (WorkSpaceStatus)foundWorkSpace.Status,
                Owner = foundWorkSpace.Owner,
                Members = users,
                Boards = foundWorkSpace.Boards.Select(_ => new BoardDTO
                {
                    Id = _.Id,
                    Name = _.Name,
                    Description = _.Description,
                    Status = (BoardStatus)_.Status,
                    UpdatedAt = _.UpdatedAt
                }).ToList()
            };
            return new GetWorkspaceResponse(true, string.Empty, workSpaceDTO);
        }

        /// <summary>
        /// GetListAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetWorkspacesResponse> GetListAsync(string userId)
        {
            IQueryable<WorkSpace> query = context.WorkSpaces
                .Include(_ => _.WorkSpaceUsers)
                .Where(w => w.WorkSpaceUsers.Any(wu => wu.UserId == userId))
                .AsQueryable();
            List<WorkSpaceDTO> workspaces = await query.OrderBy(_ => _.Name)
                .Select(_ => new WorkSpaceDTO
                {
                    Id = _.Id,
                    Name = _.Name,
                    Description = _.Description,
                    UpdatedAt = _.UpdatedAt,
                    CreatedAt = _.CreatedAt,
                    Status = (WorkSpaceStatus)_.Status,
                    Boards = _.Boards.Select(_ => new BoardDTO
                    {
                        Id = _.Id,
                        Name = _.Name,
                        Description = _.Description,
                        Status = (BoardStatus)_.Status,
                        UpdatedAt = _.UpdatedAt
                    }).ToList()
                })
                .ToListAsync();
            return new GetWorkspacesResponse(workspaces);
        }

        /// <summary>
        /// RemoveMemberAsync
        /// </summary>
        /// <param name="workspaceId"></param>
        /// <param name="memberId"></param>
        /// <param name="updaterId"></param>
        /// <returns></returns>
        public async Task<PostPutResponse> RemoveMemberAsync(Guid workspaceId, string memberId, string updaterId)
        {
            ApplicationUser? foundMember = await GetUserByIdAsync(memberId);
            if (foundMember is null)
            {
                return new PostPutResponse(false, HttpConstants.UserNotFound);
            }
            WorkSpace? foundWorkSpace = await GetRecordByIdAsync(workspaceId);
            if (foundWorkSpace is null)
            {
                return new PostPutResponse(false, HttpConstants.WorkSpaceNotFound);
            }
            ApplicationUser? foundUpdater = await GetUserByIdAsync(updaterId);

            WorkSpaceUser? workSpaceUser = foundWorkSpace.WorkSpaceUsers
                .FirstOrDefault(_ => _.UserId == memberId);

            if (workSpaceUser is not null)
            {
                context.WorkSpaceUsers.Remove(workSpaceUser);
                await context.SaveChangesAsync();
                await eventPusher.Publish(new WorkSpaceMemberRemove
                {
                    ObjectId = workspaceId.ToString(),
                    WorkSpaceName = foundWorkSpace.Name,
                    UpdaterName = foundUpdater!.FullName,
                    RecieverId = memberId
                });
            }
            return new PostPutResponse(true, HttpConstants.WorkSpaceRemovedMember);
        }

        /// <summary>
        /// UpdateAsync
        /// </summary>
        /// <param name="workSpaceDTO"></param>
        /// <returns></returns>
        public async Task<PostPutResponse> UpdateAsync(WorkSpaceDTO workSpaceDTO)
        {
            ApplicationUser? foundUpdater = await GetUserByIdAsync(workSpaceDTO.LastUpdateBy);

            if (foundUpdater is null)
            {
                return new PostPutResponse(false, HttpConstants.UserNotFound);
            }
            WorkSpace? foundWorkSpace = await GetRecordByIdAsync(workSpaceDTO.Id);
            if (foundWorkSpace is null)
            {
                return new PostPutResponse(false, HttpConstants.WorkSpaceNotFound);
            }
            foundWorkSpace.Name = workSpaceDTO.Name;
            foundWorkSpace.Description = workSpaceDTO.Description;
            foundWorkSpace.UpdatedAt = DateTime.UtcNow;
            foundWorkSpace.Status = (int)workSpaceDTO.Status;
            context.WorkSpaces.Update(foundWorkSpace);
            await context.SaveChangesAsync();
            return new PostPutResponse(true, HttpConstants.WorkSpaceSavedSuccessfully);
        }

        /// <summary>
        /// GetRecordByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<WorkSpace?> GetRecordByIdAsync(Guid id)
        {
            return await context.WorkSpaces
                .Include(_ => _.WorkSpaceUsers)
                .Include(_ => _.Boards)
                .FirstOrDefaultAsync(_ => _.Id == id && _.Status == (int)WorkSpaceStatus.Opened);
        }

        /// <summary>
        /// GetUserByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<ApplicationUser?> GetUserByIdAsync(string id)
        {
            return await context.Users.FirstOrDefaultAsync(_ => _.Id == id && _.ActiveStatus);
        }
    }
}
