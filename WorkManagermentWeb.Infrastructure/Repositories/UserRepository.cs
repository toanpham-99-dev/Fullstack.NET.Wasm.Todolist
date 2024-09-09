using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Data;
using System.Net;
using WorkManagermentWeb.Application.Constants;
using WorkManagermentWeb.Application.Contracts;
using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Application.Utilities;
using WorkManagermentWeb.Authorization.Services;
using WorkManagermentWeb.Domain.Entities;
using WorkManagermentWeb.Domain.Enums;
using WorkManagermentWeb.Infrastructure.Data;
using WorkManagermentWeb.Infrastructure.Options;
using WorkManagermentWeb.Infrastructure.Services;
using PasswordGenerator;

namespace WorkManagermentWeb.Infrastructure.Repositories
{
    /// <summary>
    /// UserRepository
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="roleManager"></param>
    /// <param name="authenticationService"></param>
    /// <param name="context"></param>
    /// <param name="emailSender"></param>
    /// <param name="smsSender"></param>
    /// <param name="workItemRepository"></param>
    /// <param name="commonOptions"></param>
    public class UserRepository(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        AppDBContext context,
        IAuthentication authenticationService,
        IEmailSender emailSender,
        ISmsSender smsSender,
        IWorkItem workItemRepository,
        IOptions<CommonOptions> commonOptions
        ) : IUser
    {
        /// <summary>
        /// GetByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ProfileResponse> GetByIdAsync(string id)
        {
            ApplicationUser? foundUser = await context.Users
                .Include(_ => _.WorkItems.Where(_ => !_.IsDeleted))
                    .ThenInclude(_ => _.ParentWorkItem)
                .Include(_ => _.WorkItems.Where(_ => !_.IsDeleted))
                    .ThenInclude(_ => _.CalendarEvent)
                .FirstOrDefaultAsync(_ => _.Id == id)
                .ConfigureAwait(false);

            if (foundUser is null)
            {
                return new ProfileResponse(false, HttpConstants.UserNotFound);
            }
            List<string> roles = (await userManager.GetRolesAsync(foundUser)).ToList();

            IQueryable<Board> boardQuery = context.Boards.Where(_ => !_.IsDeleted)
                .Include(_ => _.BoardUsers).Include(_ => _.WorkItems.Where(_ => !_.IsDeleted))
                .AsQueryable();

            List<Board> boards = await boardQuery.ToListAsync();

            List<BoardDTO> userBoardsDTO = boards.Where(_ => _.BoardUsers.Any(bu => bu.UserId == id))
                .OrderByDescending(_ => _.CreatedAt)
                .Select(_ => new BoardDTO
                {
                    Id = _.Id,
                    Name = _.Name,
                    CreatedAt = _.CreatedAt,
                    StartDate = _.StartDate,
                    EndDate = _.EndDate,
                    Status = (BoardStatus)_.Status,
                    Owner = new UserDTO
                    {
                        Id = _.AssigneeId
                    },
                    WorkItems = _.WorkItems.Select(w => new WorkItemDTO
                    {
                        Id = w.Id
                    }).ToList(),
                    Members = _.BoardUsers.Select(bu => new UserDTO
                    {
                        Id = bu.UserId
                    }).ToList()

                }).ToList();

            return new ProfileResponse(true, string.Empty, new UserDTO
            {
                Id = id,
                FullName = foundUser.FullName,
                Email = foundUser.Email!,
                IsEmailConfirmed = foundUser.EmailConfirmed,
                UserName = foundUser.UserName!,
                Phone = foundUser.PhoneNumber,
                IsPhoneConfirmed = foundUser.PhoneNumberConfirmed,
                ActiveStatus = foundUser.ActiveStatus,
                AccountType = foundUser.AccountType,
                ExternalAccountConnected = foundUser.ExternalAccountConnected,
                Role = new RoleDTO
                {
                    Value = String.Join(",", roles),
                    DisplayName = Helper.GetConstFieldAttributeValue<UserRoleConstants, string, DescriptionAttribute>(String.Join(",", roles), y => y.Description),
                },
                Projects = userBoardsDTO,
                WorkItems = foundUser.WorkItems
                .Select(_ => new WorkItemDTO
                {
                    Id = _.Id,
                    Code = _.Code,
                    Title = _.Title,
                    StartDate = _.StartDate,
                    EndDate = _.EndDate,
                    Type = _.Type,
                    Status = (WorkItemStatus)_.Status,
                    Priority = _.Priority,
                    ParentWorkItemCode = _.ParentWorkItemId != _.Id ? _.ParentWorkItem.Code : null,
                    IsSyncToCalendar = _.CalendarEvent is not null && _.CalendarEvent.IsSynced ? true : false,
                    Board = new BoardInfo
                    {
                        Id = _.BoardId,
                        Name = boards.FirstOrDefault(b => b.Id == _.BoardId)!.Name
                    }
                }).ToList()
            });
        }

        /// <summary>
        /// GetListAsync
        /// </summary>
        /// <param name="usersDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<UsersResponse> GetListAsync(UsersDTO usersDTO)
        {
            IQueryable<IdentityRole> rolesQuery = roleManager.Roles
                .AsQueryable();
            if (!string.IsNullOrEmpty(usersDTO.RoleName))
            {
                rolesQuery = rolesQuery.Where(u => u.Name!.ToLower() == usersDTO.RoleName);
            }
            IQueryable<IdentityUserRole<string>> userRolesQuery = context.UserRoles
                .Where(ur => rolesQuery.Any(r => r.Id == ur.RoleId))
                .AsQueryable();

            IQueryable<ApplicationUser> usersQuery = userManager.Users
                .Where(u => userRolesQuery.Any(ur => ur.UserId == u.Id))
                .Include(_ => _.BoardUsers)
                .AsQueryable();

            if (usersDTO.ActiveStatus is not null)
            {
                usersQuery = usersQuery.Where(u => u.ActiveStatus == usersDTO.ActiveStatus);
            }
            if (!string.IsNullOrEmpty(usersDTO.SearchInput))
            {
                usersQuery = usersQuery.Where(_ => _.FullName.Contains(usersDTO.SearchInput)
                || (_.UserName != null && _.UserName.Contains(usersDTO.SearchInput))
                || (_.PhoneNumber != null && _.PhoneNumber.Contains(usersDTO.SearchInput))
                || _.Email!.Contains(usersDTO.SearchInput));
            }
            if (usersDTO.BoardId != Guid.Empty)
            {
                usersQuery = usersQuery.Where(u => u.BoardUsers.Any(bu => bu.BoardId == usersDTO.BoardId));
            }
            int total = await usersQuery.CountAsync().ConfigureAwait(false);
            if (total == 0)
            {
                return new UsersResponse(new List<UserDTO>(), total);
            }
            IEnumerable<ApplicationUser> users = await usersQuery.OrderBy(_ => _.UserName).Skip(usersDTO.Paging.Skip).Take(usersDTO.Paging.Take)
            .ToListAsync().ConfigureAwait(false);

            var result = await users.SelectAsync(async s => await SetUserDTOAsync(s), 1);
            return new UsersResponse(result.ToList(), total);
        }

        /// <summary>
        /// SetUserDTOAsync
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<UserDTO> SetUserDTOAsync(ApplicationUser user)
        {
            List<string> roles = (await userManager.GetRolesAsync(user)).ToList();
            return new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName!,
                Phone = user.PhoneNumber!,
                Email = user.Email!,
                AccountType = user.AccountType,
                ActiveStatus = user.ActiveStatus,
                Role = new RoleDTO
                {
                    Value = String.Join(",", roles),
                    DisplayName = Helper.GetConstFieldAttributeValue<UserRoleConstants, string, DescriptionAttribute>(String.Join(",", roles), y => y.Description)
                }
            };
        }

        /// <summary>
        /// LoginAsync
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns></returns>
        public async Task<GenerateTokenResponse> LoginAsync(LoginDTO loginDTO)
        {
            ApplicationUser? foundUser = await userManager.FindByEmailAsync(loginDTO.Email);

            if (foundUser is null)
                return new GenerateTokenResponse(false, HttpConstants.UserNotFound);

            bool isPasswordCorrect = await userManager.CheckPasswordAsync(foundUser, loginDTO.Password);

            if (!isPasswordCorrect)
                return new GenerateTokenResponse(false, HttpConstants.LoginFalse);

            var getUserRole = await userManager.GetRolesAsync(foundUser);
            var userSession = new UserSession(foundUser.Id, foundUser.UserName, foundUser.Email, getUserRole.First());

            string token = authenticationService.GenerateJwtToken(userSession);
            return new GenerateTokenResponse(true, HttpConstants.LoginSuccessfully, token);
        }

        /// <summary>
        /// RegisterAsync
        /// </summary>
        /// <param name="registerUserDTO"></param>
        /// <returns></returns>
        public async Task<RegistrationResponse> RegisterAsync(RegisterUserDTO registerUserDTO)
        {
            ApplicationUser? foundUser = await userManager.FindByEmailAsync(registerUserDTO.Email);
            if (foundUser is not null)
                return new RegistrationResponse(false, HttpConstants.UserAlreadyExist);

            bool isUsernameExist = await context.Users.AnyAsync(_ => _.UserName == registerUserDTO.UserName);
            if (isUsernameExist)
                return new RegistrationResponse(false, HttpConstants.UsernameAlreadyTaken);

            var newUser = new ApplicationUser()
            {
                FullName = registerUserDTO.FullName,
                Email = registerUserDTO.Email,
                PasswordHash = registerUserDTO.Password,
                UserName = registerUserDTO.UserName,
                PhoneNumber = registerUserDTO.Phone,
                PhoneNumberConfirmed = registerUserDTO.IsPhoneComfirmed,
                EmailConfirmed = registerUserDTO.IsEmailComfirmed,
                ActiveStatus = registerUserDTO.ActiveStatus
            };
            var createUser = await userManager.CreateAsync(newUser!, registerUserDTO.Password);
            if (!createUser.Succeeded) return new RegistrationResponse(false, HttpConstants.ModifyRecordFailed);

            var checkCEO = await roleManager.FindByNameAsync(UserRoleConstants.CEO);
            if (checkCEO is null)
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = UserRoleConstants.CEO });
                await userManager.AddToRoleAsync(newUser, UserRoleConstants.CEO);
            }
            else
            {
                var checkUser = await roleManager.FindByNameAsync(registerUserDTO.RoleName);
                if (checkUser is null)
                    await roleManager.CreateAsync(new IdentityRole() { Name = registerUserDTO.RoleName });

                await userManager.AddToRoleAsync(newUser, registerUserDTO.RoleName);
            }

            return new RegistrationResponse(true, HttpConstants.RegistrationCompleted, newUser.Id);
        }

        /// <summary>
        /// RemindNearingDuesAsync
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task RemindNearingDuesAsync()
        {
            List<UserNearingDue> userNearingDues = await GetUsersHaveNearingDuesAsync();
            List<Task> tasks = new List<Task>();
            foreach (var userNearingDue in userNearingDues)
            {
                tasks.Add(InboxToSingleAsync(userNearingDue));
            }
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// InboxToSingleAsync
        /// </summary>
        /// <param name="userNearingDue"></param>
        /// <returns></returns>
        private async Task InboxToSingleAsync(UserNearingDue userNearingDue)
        {
            string taskListHtml = string.Empty;
            string singleTaskHtml = string.Empty;
            foreach (var task in userNearingDue.WorkItems)
            {
                singleTaskHtml = String.Format(EmailAndHtmlPatterms.EmailHtmlWorkItemsTemplate, task.Code, task.Title);
                taskListHtml += singleTaskHtml;
            }
            string bodyHtml = String.Format(EmailAndHtmlPatterms.NearringDueEmailHtmlTemplate,
                userNearingDue.FullName, taskListHtml);
            await emailSender.SendEmailAsync(userNearingDue.Email,
                EmailAndHtmlPatterms.RemindNearringDueSubject, bodyHtml);
        }

        /// <summary>
        /// GetUsersHaveNearingDuesAsync
        /// </summary>
        /// <returns></returns>
        private async Task<List<UserNearingDue>> GetUsersHaveNearingDuesAsync()
        {
            IQueryable<WorkItem> nearingDues = workItemRepository.QueryAllNearingDues();
            List<ApplicationUser> users = await userManager.Users
                .Include(_ => _.WorkItems)
                .Where(u => u.ActiveStatus && nearingDues.Any(n => n.AssigneeId == u.Id))
                .ToListAsync();

            List<UserNearingDue> userNearingDues = new List<UserNearingDue>();

            foreach (var user in users)
            {
                UserNearingDue userNearingDue = new UserNearingDue
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email!,
                    WorkItems = user.WorkItems.Where(_ => nearingDues.Contains(_))
                        .Select(_ => new SubWorkItem
                        {
                            Id = _.Id,
                            Code = _.Code,
                            Title = _.Title,
                            StartDate = _.StartDate,
                            EndDate = _.EndDate,
                            Status = (WorkItemStatus)_.Status,
                        }).ToList()
                };
                userNearingDues.Add(userNearingDue);
            }
            return userNearingDues;
        }

        /// <summary>
        /// UpdateRoleAsync
        /// </summary>
        /// <param name="roleDTO"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostPutResponse> UpdateRolesAsync(UserRoleDTO roleDTO, string id)
        {
            ApplicationUser? foundUser = await userManager
                .FindByIdAsync(id)
                .ConfigureAwait(false);

            if (foundUser is null)
            {
                return new PostPutResponse(false, HttpConstants.UserNotFound);
            }
            List<string> foundRoles = (await userManager.GetRolesAsync(foundUser)
                .ConfigureAwait(false)).ToList();

            await userManager.RemoveFromRolesAsync(foundUser, foundRoles)
                .ConfigureAwait(false);
            if (roleDTO.RoleNames is null || roleDTO.RoleNames.Count == 0)
            {
                return new PostPutResponse(true);
            }
            IdentityResult result = await userManager.AddToRolesAsync(foundUser, roleDTO.RoleNames)
                .ConfigureAwait(false);
            return new PostPutResponse(result.Succeeded, HttpConstants.AccountUpdatedSuccessfully);
        }

        /// <summary>
        /// ChangePasswordAsync
        /// </summary>
        /// <param name="passwordDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostPutResponse> ChangePasswordAsync(PasswordDTO passwordDTO)
        {
            ApplicationUser? foundUser = await userManager
                .FindByIdAsync(passwordDTO.UserId)
                .ConfigureAwait(false);

            if (foundUser is null)
            {
                return new PostPutResponse(false, HttpConstants.UserNotFound);
            }
            IdentityResult result = await userManager.ChangePasswordAsync(foundUser, passwordDTO.CurrentPassword, passwordDTO.NewPassword)
                .ConfigureAwait(false);
            if (!result.Succeeded)
            {
                return new PostPutResponse(result.Succeeded, HttpConstants.WrongCurrentPassword);
            }

            return new PostPutResponse(result.Succeeded, HttpConstants.AccountUpdatedSuccessfully);
        }

        /// <summary>
        /// UpdateAsync
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostPutResponse> UpdateAsync(UpdateUserDTO userDTO)
        {
            ApplicationUser? foundUser = await userManager
                .FindByIdAsync(userDTO.Id)
                .ConfigureAwait(false);

            if (foundUser is null)
            {
                return new PostPutResponse(false, HttpConstants.UserNotFound);
            }

            bool isUsernameExist = await context.Users.AnyAsync(_ => _.UserName == userDTO.UserName && _.Id != userDTO.Id);
            if (isUsernameExist)
                return new PostPutResponse(false, HttpConstants.UsernameAlreadyTaken);

            foundUser.FullName = userDTO.FullName;
            foundUser.UserName = userDTO.UserName;
            foundUser.PhoneNumber = userDTO.Phone;
            foundUser.PhoneNumberConfirmed = userDTO.IsPhoneConfirmed;
            string foundRole = (await userManager.GetRolesAsync(foundUser)
                .ConfigureAwait(false)).FirstOrDefault()!;
            if (userDTO.Role is not null && foundRole != userDTO.Role!.Value)
            {
                await userManager.RemoveFromRoleAsync(foundUser, foundRole)
                    .ConfigureAwait(false);
                await userManager.AddToRoleAsync(foundUser, userDTO.Role.Value)
                    .ConfigureAwait(false);
            }

            IdentityResult result = await userManager.UpdateAsync(foundUser).ConfigureAwait(false);
            return new PostPutResponse(result.Succeeded, HttpConstants.AccountUpdatedSuccessfully);
        }

        /// <summary>
        /// ChangeActiveStatusAsync
        /// </summary>
        /// <param name="activeStatusDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostPutResponse> ChangeActiveStatusAsync(ActiveStatusDTO activeStatusDTO)
        {
            ApplicationUser? foundUser = await userManager
                .FindByIdAsync(activeStatusDTO.UserId)
                .ConfigureAwait(false);

            if (foundUser is null)
            {
                return new PostPutResponse(false, HttpConstants.UserNotFound);
            }

            foundUser.ActiveStatus = activeStatusDTO.Status;
            IdentityResult result = await userManager.UpdateAsync(foundUser).ConfigureAwait(false);
            return new PostPutResponse(result.Succeeded, HttpConstants.AccountUpdatedSuccessfully);
        }

        /// <summary>
        /// ValidatePhoneNumberAsync
        /// </summary>
        /// <param name="phoneNumberDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CheckingDataResponse> ValidatePhoneNumberAsync(PhoneNumberDTO phoneNumberDTO)
        {
            ApplicationUser? foundUser = await userManager
                .FindByIdAsync(phoneNumberDTO.UserId)
                .ConfigureAwait(false);

            if (foundUser is null)
            {
                return new CheckingDataResponse(false);
            }
            bool isvalidatePhoneNumber = await userManager
                .VerifyChangePhoneNumberTokenAsync(foundUser, phoneNumberDTO.Token, phoneNumberDTO.PhoneNumber)
                .ConfigureAwait(false);
            return new CheckingDataResponse(isvalidatePhoneNumber);
        }

        /// <summary>
        /// SendPhoneValidateTokenAsync
        /// </summary>
        /// <param name="phoneNumberDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CheckingDataResponse> SendPhoneValidateTokenAsync(PhoneNumberDTO phoneNumberDTO)
        {
            ApplicationUser? foundUser = await userManager
                .FindByIdAsync(phoneNumberDTO.UserId)
                .ConfigureAwait(false);

            if (foundUser is null)
            {
                return new CheckingDataResponse(false);
            }
            string token = await userManager.GenerateChangePhoneNumberTokenAsync(foundUser, phoneNumberDTO.PhoneNumber)
                .ConfigureAwait(false);
            await smsSender.SendAsync(
                phoneNumberDTO.PhoneNumber,
                $"We detect some one has use your phone number on our system WorkManagerment." +
                $" If that's you, use this token {token} to validate your phone number!")
                .ConfigureAwait(false);
            return new CheckingDataResponse(true);
        }

        /// <summary>
        /// GetRolesAsync
        /// </summary>
        /// <returns></returns>
        public async Task<GetRolesResponse> GetRolesAsync()
        {
            GetRolesResponse reponse = new(new List<RoleDTO>());
            foreach (var role in UserRoleConstants.UserRoles)
            {
                reponse.RoleDTOs.Add(new RoleDTO
                {
                    DisplayName = Helper.GetConstFieldAttributeValue<UserRoleConstants, string, DescriptionAttribute>(String.Join(",", role), y => y.Description),
                    Value = role
                });
            }
            return await Task.FromResult(reponse);
        }

        /// <summary>
        /// ValidateEmailAsync
        /// </summary>
        /// <param name="emailDTO"></param>
        /// <returns></returns>
        public async Task<CheckingDataResponse> ValidateEmailAsync(EmailDTO emailDTO)
        {
            ApplicationUser? foundUser = await userManager
                .FindByIdAsync(emailDTO.UserId)
                .ConfigureAwait(false);

            if (foundUser is null)
            {
                return new CheckingDataResponse(false);
            }
            IdentityResult isvalidateEmail = await userManager
                .ConfirmEmailAsync(foundUser, emailDTO.Token)
                .ConfigureAwait(false);

            if (isvalidateEmail.Succeeded)
            {
                foundUser.ActiveStatus = true;
                await userManager.UpdateAsync(foundUser).ConfigureAwait(false);
            }
            return new CheckingDataResponse(isvalidateEmail.Succeeded);

        }

        /// <summary>
        /// SendEmailValidateTokenAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CheckingDataResponse> SendEmailValidateTokenAsync(string userId)
        {
            ApplicationUser? foundUser = await userManager
                .FindByIdAsync(userId)
                .ConfigureAwait(false);

            if (foundUser is null)
            {
                return new CheckingDataResponse(false);
            }
            string token = await userManager.GenerateEmailConfirmationTokenAsync(foundUser)
                .ConfigureAwait(false);
            token = WebUtility.UrlEncode(token);
            string redirectUrl = $"{commonOptions.Value.ApiBaseAddress}/{ApiRouteConstants.PrefixUrl}/{ApiRouteConstants.User}/{foundUser.Id}/{ApiRouteConstants.VerifyEmail}?{nameof(token)}={token}";
            string bodyHtml = String.Format(EmailAndHtmlPatterms.VerifyEmailHtmlTemplate,
                foundUser.FullName, redirectUrl);
            await emailSender.SendEmailAsync(foundUser.Email!,
                EmailAndHtmlPatterms.ConfirmEmailSubject, bodyHtml)
                .ConfigureAwait(false);
            return new CheckingDataResponse(true);
        }

        /// <summary>
        /// SendPasswordResetTokenAsync
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CheckingDataResponse> SendPasswordResetTokenAsync(string email)
        {
            ApplicationUser? foundUser = await userManager
                .FindByEmailAsync(email)
                .ConfigureAwait(false);

            if (foundUser is null || foundUser.AccountType == AccountType.External)
            {
                return new CheckingDataResponse(false);
            }
            string token = await userManager.GeneratePasswordResetTokenAsync(foundUser)
                .ConfigureAwait(false);

            IdentityUserToken<string>? foundUserToken = await context.UserTokens
                .FirstOrDefaultAsync(_ => _.UserId == foundUser.Id && _.Name == ApiRouteConstants.RequestResetPasswordToken);

            if (foundUserToken is not null)
            {
                foundUserToken.Value = token;
                context.UserTokens.Update(foundUserToken);
            }
            else
            {
                IdentityUserToken<string> newUserToken = new IdentityUserToken<string>()
                {
                    LoginProvider = Guid.NewGuid().ToString(),
                    UserId = foundUser.Id,
                    Name = ApiRouteConstants.RequestResetPasswordToken,
                    Value = token
                };
                context.UserTokens.Add(newUserToken);
            }
            await context.SaveChangesAsync();

            token = WebUtility.UrlEncode(token);
            string redirectUrl = $"{commonOptions.Value.Domain}/forgot-password?{nameof(IdentityUserToken<string>.UserId).ToLower()}={foundUser.Id}" +
                $"&{nameof(token)}={token}&{nameof(email)}={email}";
            string bodyHtml = String.Format(EmailAndHtmlPatterms.ResetPasswordHtmlTemplate,
                foundUser.FullName, redirectUrl);
            await emailSender.SendEmailAsync(foundUser.Email!,
                EmailAndHtmlPatterms.ResetPasswordSubject, bodyHtml)
                .ConfigureAwait(false);
            return new CheckingDataResponse(true);
        }

        /// <summary>
        /// ResetPasswordAsync
        /// </summary>
        /// <param name="resetPasswordDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PostPutResponse> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            ApplicationUser? foundUser = await userManager
                .FindByEmailAsync(resetPasswordDTO.Email)
                .ConfigureAwait(false);

            if (foundUser is null)
            {
                return new PostPutResponse(false, HttpConstants.UserNotFound);
            }
            IdentityResult result = await userManager.ResetPasswordAsync(foundUser, resetPasswordDTO.Token, resetPasswordDTO.NewPassword)
                .ConfigureAwait(false);
            string message = HttpConstants.ResetPasswordFailed;
            if (result is not null && result.Succeeded)
            {
                IdentityUserToken<string>? foundUserToken = await context.UserTokens
                    .FirstOrDefaultAsync(_ => _.UserId == foundUser.Id &&
                                         _.Name == ApiRouteConstants.RequestResetPasswordToken &&
                                         _.Value == resetPasswordDTO.Token);
                if (foundUserToken is not null)
                {
                    context.UserTokens.Remove(foundUserToken);
                    await context.SaveChangesAsync();
                }
                message = HttpConstants.AccountUpdatedSuccessfully;
            }
            return new PostPutResponse(result!.Succeeded, message);
        }

        /// <summary>
        /// IsPasswordResetTokenAlive
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CheckingDataResponse> IsPasswordResetTokenAlive(string userId, string token)
        {
            string base64EncodeToken = token.Replace(" ", "+");

            bool isTokenAlive = await context.UserTokens
                    .AnyAsync(_ => _.UserId == userId &&
                              _.Name == ApiRouteConstants.RequestResetPasswordToken &&
                              _.Value == base64EncodeToken);
            return new CheckingDataResponse(isTokenAlive);
        }

        /// <summary>
        /// ExternalLoginAsync
        /// </summary>
        /// <param name="externalLoginDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GenerateTokenResponse> ExternalLoginAsync(ExternalLoginDTO externalLoginDTO)
        {
            ApplicationUser? foundUser = await userManager.FindByEmailAsync(externalLoginDTO.Email);
            UserSession userSession;
            string token = string.Empty;
            string userId = foundUser is not null ? foundUser!.Id : Guid.NewGuid().ToString();

            if (foundUser is not null)
            {
                if (foundUser.AccountType == AccountType.Internal)
                {
                    foundUser.ExternalAccountConnected = true;
                    IdentityResult result = await userManager.UpdateAsync(foundUser).ConfigureAwait(false);
                }

                string foundRole = (await userManager.GetRolesAsync(foundUser)
                    .ConfigureAwait(false)).FirstOrDefault()!;

                var getUserRole = await userManager.GetRolesAsync(foundUser);
                userSession = new UserSession(foundUser.Id, foundUser.UserName, foundUser.Email, getUserRole.First());

                token = authenticationService.GenerateJwtToken(userSession);
                return new GenerateTokenResponse(true, HttpConstants.LoginSuccessfully, token);
            }
            Password pwd = new Password(16);
            string password = pwd.Next();
            string username = externalLoginDTO.Email;
            bool isUsernameExist = await context.Users.AnyAsync(_ => _.UserName == username);
            if (isUsernameExist)
                username = $"{username}1";

            var newUser = new ApplicationUser()
            {
                Id = userId,
                FullName = externalLoginDTO.FullName,
                Email = externalLoginDTO.Email,
                PasswordHash = password,
                UserName = username,
                EmailConfirmed = true,
                ActiveStatus = true,
                AccountType = AccountType.External
            };
            var createUser = await userManager.CreateAsync(newUser!, password);
            await userManager.AddToRoleAsync(newUser, externalLoginDTO.RoleName);

            string bodyHtml = String.Format(EmailAndHtmlPatterms.NewMSAccountHtmlTemplate, newUser.FullName);

            await emailSender.SendEmailAsync(newUser.Email!, EmailAndHtmlPatterms.NewMSAccountSubject, bodyHtml)
                .ConfigureAwait(false);

            userSession = new UserSession(newUser.Id, newUser.UserName, newUser.Email, externalLoginDTO.RoleName);

            token = authenticationService.GenerateJwtToken(userSession);
            return new GenerateTokenResponse(true, HttpConstants.LoginSuccessfully, token);
        }

        /// <summary>
        /// UpsertGraphToken
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="graphToken"></param>
        /// <returns></returns>
        private async Task UpsertGraphToken(string userId, string graphToken)
        {
            IdentityUserToken<string>? foundToken = await context.UserTokens
                .FirstOrDefaultAsync(_ => _.UserId == userId && _.Name == AuthorizationConstants.GraphToken)
                .ConfigureAwait(false);

            if (foundToken is not null)
            {
                foundToken.Value = graphToken;
                context.UserTokens.Update(foundToken);
                await context.SaveChangesAsync();
                return;
            }
            IdentityUserToken<string> newUserToken = new IdentityUserToken<string>()
            {
                LoginProvider = Guid.NewGuid().ToString(),
                UserId = userId,
                Name = AuthorizationConstants.GraphToken,
                Value = graphToken
            };
            context.UserTokens.Add(newUserToken);
            await context.SaveChangesAsync();
            return;
        }
    }
}
