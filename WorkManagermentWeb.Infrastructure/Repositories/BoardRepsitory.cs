using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using WorkManagermentWeb.Application.Constants;
using WorkManagermentWeb.Application.Contracts;
using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Application.Utilities;
using WorkManagermentWeb.Domain.Entities;
using WorkManagermentWeb.Domain.Enums;
using WorkManagermentWeb.EventHandler.Events;
using WorkManagermentWeb.EventHandler.Services;
using WorkManagermentWeb.Infrastructure.Data;

namespace WorkManagermentWeb.Infrastructure.Repositories
{
    /// <summary>
    /// BoardRepsitory
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userManager"></param>
    /// <param name="user"></param>
    /// <param name="eventPusher"></param>
    public class BoardRepsitory(
        AppDBContext context,
        UserManager<ApplicationUser> userManager,
        IUser user,
        IEventPusher eventPusher) : IBoard
    {
        /// <summary>
        /// AddAsync
        /// </summary>
        /// <param name="boardDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostPutResponse> AddAsync(PostPutBoardDTO boardDTO)
        {
            WorkSpace? foundWorkSpace = await context.WorkSpaces
                .FirstOrDefaultAsync(_ => _.Id == boardDTO.WorkSpaceId && _.Status == (int)WorkSpaceStatus.Opened);

            if (foundWorkSpace is null)
            {
                return new PostPutResponse(false, HttpConstants.WorkspaceNotFound);
            }

            ApplicationUser? foundUser = await GetUserByIdAsync(boardDTO.LastUpdateBy);
            if (foundUser is null)
            {
                return new PostPutResponse(false, HttpConstants.UserNotFound);
            }
            Guid newBoardId = Guid.NewGuid();
            Board newBoard = new Board()
            {
                Id = newBoardId,
                Name = boardDTO.Title,
                Description = boardDTO.Description,
                LastUpdateBy = boardDTO.LastUpdateBy,
                WorkSpaceId = foundWorkSpace.Id,
                AssigneeId = boardDTO.AssigneeId,
                AssigneeName = boardDTO.AssigneeName,
                StartDate = boardDTO.StartDate!.Value.ToDateOnly(Helper.GetTimeZonePlus7()),
                EndDate = boardDTO.EndDate!.Value.ToDateOnly(Helper.GetTimeZonePlus7()),
                BoardUsers = new List<BoardUser>()
                {
                    new BoardUser
                    {
                        Id = Guid.NewGuid(),
                        BoardId = newBoardId,
                        UserId = foundUser.Id
                    },
                    new BoardUser
                    {
                        Id = Guid.NewGuid(),
                        BoardId = newBoardId,
                        UserId = boardDTO.AssigneeId
                    }
                }
            };
            context.Boards.Add(newBoard);
            await context.SaveChangesAsync();
            return new PostPutResponse(true, HttpConstants.BoardCreatedSuccessfully, newBoard.Id);
        }

        /// <summary>
        /// AddMemberAsync
        /// </summary>
        /// <param name="memberDTO"></param>
        /// <returns></returns>
        public async Task<PostPutResponse> AddMemberAsync(BoardMemberDTO memberDTO, Guid id)
        {
            Board? foundBoard = await GetBoardByIdAsync(id);
            if (foundBoard is null)
            {
                return new PostPutResponse(false, HttpConstants.BoardNotFound);
            }

            ApplicationUser? foundUser = await GetUserByIdAsync(memberDTO.MemberId);
            if (foundUser is null)
            {
                return new PostPutResponse(false, HttpConstants.UserNotFound);
            }
            ApplicationUser? foundUpdater = await GetUserByIdAsync(memberDTO.UpdaterId);

            if (!foundBoard.BoardUsers.Any(_ => _.UserId == memberDTO.MemberId))
            {
                BoardUser boardUser = new BoardUser
                {
                    Id = Guid.NewGuid(),
                    BoardId = id,
                    UserId = memberDTO.MemberId
                };
                context.BoardUsers.Add(boardUser);
                await context.SaveChangesAsync();
                await eventPusher.Publish(new BoardMemberAdd
                {
                    ObjectId = id.ToString(),
                    BoardName = foundBoard.Name,
                    UpdaterName = foundUpdater!.FullName,
                    RecieverId = memberDTO.MemberId
                });
            }

            return new PostPutResponse(true, HttpConstants.BoardAddMemberSuccessfully, new Guid(memberDTO.MemberId));
        }

        /// <summary>
        /// GetByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetBoardResponse> GetByIdAsync(Guid id)
        {
            Board? foundBoard = await GetBoardByIdAsync(id);
            if (foundBoard is null)
            {
                return new GetBoardResponse(false, HttpConstants.BoardNotFound);
            }

            List<string> uids = foundBoard.BoardUsers.Select(_ => _.UserId).ToList();
            ApplicationUser? owner = await GetUserByIdAsync(foundBoard.AssigneeId);
            List<string> roles = (await userManager.GetRolesAsync(owner!)).ToList();

            UserDTO ownerDTO = new UserDTO
            {
                Id = owner!.Id,
                FullName = owner.FullName!,
                Email = owner.Email!,
                Role = new RoleDTO
                {
                    Value = String.Join(",", roles),
                    DisplayName = Helper.GetConstFieldAttributeValue<UserRoleConstants, string, DescriptionAttribute>(String.Join(",", roles), y => y.Description)
                }
            };
            List<UserDTO> usersDTO = (await user.GetListAsync(new UsersDTO
            {
                BoardId = id,
                Paging = new PagingDTO
                {
                    Skip = 0,
                    Take = 1000
                }
            }).ConfigureAwait(false)).Users;

            return new GetBoardResponse(true, string.Empty, new BoardDTO
            {
                Id = foundBoard.Id,
                Name = foundBoard.Name,
                Description = foundBoard.Description,
                CreatedAt = foundBoard.CreatedAt,
                UpdatedAt = foundBoard.UpdatedAt,
                LastUpdateBy = foundBoard.LastUpdateBy,
                WorkSpaceId = foundBoard.WorkSpaceId,
                StartDate = foundBoard.StartDate,
                EndDate = foundBoard.EndDate,
                AssigneeId = foundBoard.AssigneeId,
                AssigneeName = foundBoard.AssigneeName,
                Status = (BoardStatus)foundBoard.Status,
                Owner = ownerDTO,
                Members = usersDTO,
                WorkItems = foundBoard.WorkItems.Where(_ => _.Status != (int)WorkItemStatus.Done)
                    .OrderByDescending(_ => _.Code)
                    .Select(_ => new WorkItemDTO
                    {
                        Id = _.Id,
                        Code = _.Code,
                        Title = _.Title,
                        StartDate = _.StartDate,
                        EndDate = _.EndDate,
                        Status = (WorkItemStatus)_.Status,
                        Type = _.Type,
                        Priority = _.Priority,
                        ParentWorkItemCode = _.ParentWorkItemId != _.Id ? _.ParentWorkItem.Code : null,
                        IsSyncToCalendar = _.CalendarEvent is not null && _.CalendarEvent.IsSynced ? true : false,
                        ParentWorkItemId = _.ParentWorkItemId
                    }).ToList()
            });
        }

        /// <summary>
        /// GetListAsync
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetBoardsResponse> GetListAsync(GetBoardsDTO filter)
        {
            IQueryable<Board> query = context.Boards.Where(_ => !_.IsDeleted)
                .Include(_ => _.BoardUsers).AsQueryable();

            if (!string.IsNullOrEmpty(filter.UserId))
            {
                query = query.Where(_ => _.BoardUsers.Any(_ => _.UserId == filter.UserId));
            }
            List<BoardDTO> boardsDTO = await query.OrderByDescending(_ => _.CreatedAt)
                .Select(_ => new BoardDTO
                {
                    Id = _.Id,
                    Name = _.Name,
                    Description = _.Description,
                    CreatedAt = _.CreatedAt,
                    UpdatedAt = _.UpdatedAt,
                    StartDate = _.StartDate,
                    EndDate = _.EndDate,
                    LastUpdateBy = _.LastUpdateBy,
                    WorkSpaceId = _.WorkSpaceId
                }).ToListAsync();
            return new GetBoardsResponse(boardsDTO);
        }

        /// <summary>
        /// RemoveMemberAsync
        /// </summary>
        /// <param name="memberDTO"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PostPutResponse> RemoveMemberAsync(BoardMemberDTO memberDTO, Guid id)
        {
            Board? foundBoard = await GetBoardByIdAsync(id);
            if (foundBoard is null)
            {
                return new PostPutResponse(false, HttpConstants.BoardNotFound);
            }

            ApplicationUser? foundMember = await GetUserByIdAsync(memberDTO.MemberId);
            if (foundMember is null)
            {
                return new PostPutResponse(false, HttpConstants.UserNotFound);
            }
            ApplicationUser? foundUpdater = await GetUserByIdAsync(memberDTO.UpdaterId);

            BoardUser? boardUser = foundBoard.BoardUsers
                .FirstOrDefault(_ => _.UserId == memberDTO.MemberId);
            if (boardUser is not null)
            {
                context.BoardUsers.Remove(boardUser);
                await context.SaveChangesAsync();
                await eventPusher.Publish(new BoardMemberRemove
                {
                    ObjectId = id.ToString(),
                    BoardName = foundBoard.Name,
                    UpdaterName = foundUpdater!.FullName,
                    RecieverId = memberDTO.MemberId
                });
            }
            return new PostPutResponse(true, HttpConstants.BoardRemoveMemberSuccessfully, new Guid(memberDTO.MemberId));
        }

        /// <summary>
        /// UpdateAsync
        /// </summary>
        /// <param name="boardDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostPutResponse> UpdateAsync(PostPutBoardDTO boardDTO)
        {
            Board? foundBoard = await GetBoardByIdAsync(boardDTO.Id);
            if (foundBoard is null)
            {
                return new PostPutResponse(false, HttpConstants.BoardNotFound);
            }
            if (boardDTO.AssigneeId != foundBoard.AssigneeId)
            {
                foundBoard.AssigneeId = boardDTO.AssigneeId;
                foundBoard.AssigneeName = boardDTO.AssigneeName;
                if (!foundBoard.BoardUsers.Any(_ => _.UserId == boardDTO.AssigneeId))
                {
                    context.BoardUsers.Add(new BoardUser
                    {
                        Id = Guid.NewGuid(),
                        BoardId = foundBoard.Id,
                        UserId = boardDTO.AssigneeId
                    });
                }
            }
            foundBoard.Name = boardDTO.Title;
            foundBoard.Description = boardDTO.Description;
            foundBoard.LastUpdateBy = boardDTO.LastUpdateBy;
            foundBoard.UpdatedAt = DateTime.UtcNow;
            foundBoard.Status = (int)boardDTO.Status;
            foundBoard.StartDate = boardDTO.StartDate!.Value.ToDateOnly(Helper.GetTimeZonePlus7());
            foundBoard.EndDate = boardDTO.EndDate!.Value.ToDateOnly(Helper.GetTimeZonePlus7());

            context.Boards.Update(foundBoard);
            await context.SaveChangesAsync();
            return new PostPutResponse(true, HttpConstants.BoardUpdatedSuccessfully, foundBoard.Id);
        }

        /// <summary>
        /// GetBoardByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<Board?> GetBoardByIdAsync(Guid id)
        {
            return await context.Boards.Include(_ => _.BoardUsers)
                .Include(_ => _.WorkItems.Where(w => !w.IsDeleted))
                .ThenInclude(_ => _.ParentWorkItem)
                .ThenInclude(_ => _.CalendarEvent)
                .FirstOrDefaultAsync(_ => _.Id == id && !_.IsDeleted);
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
