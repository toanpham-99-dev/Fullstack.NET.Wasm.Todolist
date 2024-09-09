namespace WorkManagermentWeb.Application.Constants
{
    /// <summary>
    /// HttpConstants
    /// </summary>
    public static class HttpConstants
    {
        /// <summary>
        /// ObjectNotFound
        /// </summary>
        public const string ObjectNotFound = "not found";

        /// <summary>
        /// ObjectWasDeleted
        /// </summary>
        public const string ObjectWasDeleted = "was deleted";

        /// <summary>
        /// UserNotFound
        /// </summary>
        public const string UserNotFound = $"User {ObjectNotFound}";

        /// <summary>
        /// LoginSuccessfully
        /// </summary>
        public const string LoginSuccessfully = "Login successfully";

        /// <summary>
        /// LoginFalse
        /// </summary>
        public const string LoginFalse = "Login false, wrong credentials";

        /// <summary>
        /// UserAlreadyExist
        /// </summary>
        public const string UserAlreadyExist = "Account already exist";

        /// <summary>
        /// UsernameAlreadyTaken
        /// </summary>
        public const string UsernameAlreadyTaken = "Username already taken.";

        /// <summary>
        /// ModifyRecordFailed
        /// </summary>
        public const string ModifyRecordFailed = "Error occured.. please try again";

        /// <summary>
        /// UnavailbleInputData
        /// </summary>
        public const string UnavailbleInputData = "Unavailable input data";

        /// <summary>
        /// ResetPasswordFailed
        /// </summary>
        public const string ResetPasswordFailed = "You already use this request to change password before";

        /// <summary>
        /// WrongCurrentPassword
        /// </summary>
        public const string WrongCurrentPassword = "Wrong current password";

        /// <summary>
        /// RegistrationCompleted
        /// </summary>
        public const string RegistrationCompleted = "Registration completed";

        /// <summary>
        /// WorkItemNotFound
        /// </summary>
        public const string WorkItemNotFound = $"Task {ObjectNotFound}";

        /// <summary>
        /// ParentWorkItemNotFound
        /// </summary>
        public const string ParentWorkItemNotFound = $"Parent task {ObjectNotFound}";

        /// <summary>
        /// WorkspaceNotFound
        /// </summary>
        public const string WorkspaceNotFound = $"Workspace {ObjectNotFound}";

        /// <summary>
        /// BoardNotFound
        /// </summary>
        public const string BoardNotFound = $"Project {ObjectNotFound}";

        /// <summary>
        /// WorkItemWasDeleted
        /// </summary>
        public const string WorkItemWasDeleted = $"Task {ObjectWasDeleted}";

        /// <summary>
        /// WorkItemContainUnDoneTasks
        /// </summary>
        public const string WorkItemContainUnDoneTasks = "Task contain undone tasks";

        /// <summary>
        /// WorkItemMustBeResolvedBeforeChangeToDone
        /// </summary>
        public const string WorkItemMustBeResolvedBeforeChangeToDone = "Task must be resolved before change to done";

        /// <summary>
        /// MustBeThisWorkItemCreatorToDelete
        /// </summary>
        public const string MustBeThisWorkItemCreatorToDelete = "Must be this task creator to delete";

        /// <summary>
        /// WorkItemCreatedSuccessfully
        /// </summary>
        public const string WorkItemCreatedSuccessfully = "Task created successfully";

        /// <summary>
        /// BoardCreatedSuccessfully
        /// </summary>
        public const string BoardCreatedSuccessfully = "Project created successfully";

        /// <summary>
        /// BoardAddMemberSuccessfully
        /// </summary>
        public const string BoardAddMemberSuccessfully = "Project add member successfully";

        /// <summary>
        /// BoardRemoveMemberSuccessfully
        /// </summary>
        public const string BoardRemoveMemberSuccessfully = "Project remove member successfully";

        /// <summary>
        /// BoardUpdatedSuccessfully
        /// </summary>
        public const string BoardUpdatedSuccessfully = "Project updated successfully";

        /// <summary>
        /// AccountUpdatedSuccessfully
        /// </summary>
        public const string AccountUpdatedSuccessfully = "Account updated successfully";

        /// <summary>
        /// WorkItemSavedSuccessfully
        /// </summary>
        public const string WorkItemSavedSuccessfully = "Task saved successfully";

        /// <summary>
        /// NotificationNotFound
        /// </summary>
        public const string NotificationNotFound = $"Notification {ObjectNotFound}";

        /// <summary>
        /// MarkAsReadAction
        /// </summary>
        public const string MarkAsReadAction = "mark-as-read";

        /// <summary>
        /// WorkSpaceCreatedSuccessfully
        /// </summary>
        public const string WorkSpaceCreatedSuccessfully = "Workspace created successfully";

        /// <summary>
        /// WorkSpaceNotFound
        /// </summary>
        public const string WorkSpaceNotFound = $"Workspace {ObjectNotFound}";

        /// <summary>
        /// WorkSpaceAddedMember
        /// </summary>
        public const string WorkSpaceAddedMember = $"Workspace added a member";

        /// <summary>
        /// WorkSpaceRemovedMember
        /// </summary>
        public const string WorkSpaceRemovedMember = $"Workspace removed a member";

        /// <summary>
        /// WorkSpaceSavedSuccessfully
        /// </summary>
        public const string WorkSpaceSavedSuccessfully = "Workspace Saved successfully";

        /// <summary>
        /// Vietnamese
        /// </summary>
        public const string Vietnamese = "Vi";

        /// <summary>
        /// English
        /// </summary>
        public const string English = "En";

        /// <summary>
        /// SupportLanguage
        /// </summary>
        public static List<string> SupportLanguages = new List<string>() { Vietnamese, English };
    }
}
