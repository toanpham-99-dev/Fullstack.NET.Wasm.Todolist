namespace WorkManagermentWeb.Application.Constants
{
    /// <summary>
    /// EmailAndHtmlPatterms
    /// </summary>
    public static class EmailAndHtmlPatterms
    {
        /// <summary>
        /// UserInfo
        /// </summary>
        public const string UserInfo = @"<p><b>{0}</b></br>
                                         <b>{1}</b></br>
                                         <b>{2}</b></p>";

        /// <summary>
        /// NearringDueEmailHtmlTemplate
        /// </summary>
        public const string NearringDueEmailHtmlTemplate = @"<h1>Redmine 2.0</h1>
                     <p>Hello <b>{0}</b>.</p>
                     <p>There are list tasks we need to finish to day!</p>
                     <ul>
                     {1}
                     </ul>
                     <p>Have a good day with full strength</p>";

        /// <summary>
        /// VerifyEmailHtmlTemplate
        /// </summary>
        public const string VerifyEmailHtmlTemplate = @"<h1>Redmine 2.0</h1>
                     <p>Hello <b>{0}</b>.</p>
                     <p>We received your request for email confirmation.</p>
                     <p>Go into this url to confirm your email: <a href='{1}'>Confirm Email</a></p>
                     <p>If you didn't request this url, you can safely ignore this email. Someone else might have typed your email address by mistake.</p>
                     <p>Thanks,</p>
                     <p>The Redmine 2.0 account team</p>";

        /// <summary>
        /// ResetPasswordHtmlTemplate
        /// </summary>
        public const string ResetPasswordHtmlTemplate = @"<h1>Redmine 2.0</h1>
                     <p>Hello <b>{0}</b>.</p>
                     <p>We received your request for password resetation.</p>
                     <p>Go into this url to change your password: <a href='{1}'>Reset password</a></p>
                     <p>If you didn't request this url, you can safely ignore this email. Someone else might have typed your email address by mistake.</p>
                     <p>Thanks,</p>
                     <p>The Redmine 2.0 account team</p>";

        /// <summary>
        /// NewMSAccountHtmlTemplate
        /// </summary>
        public const string NewMSAccountHtmlTemplate = @"<h1>Redmine 2.0</h1>
                     <p>Hello <b>{0}</b>.</p>
                     <p>Welcome to our system.</p>
                     <p>Our system auto set your username is your email by default. And you can also change it later.</p>
                     <p>Have a good day with full strength</p>   
                     <p>Thanks,</p>
                     <p>The Redmine 2.0 account team</p>";

        /// <summary>
        /// EmailHtmlWorkItemsTemplate
        /// </summary>
        public const string EmailHtmlWorkItemsTemplate = " < li >#{0}: {1}.</li>";

        /// <summary>
        /// RemindNearringDueSubject
        /// </summary>
        public const string RemindNearringDueSubject = "Redmine2.0 - Threre are some tasks are waitting for you!";

        /// <summary>
        /// ConfirmEmailSubject
        /// </summary>
        public const string ConfirmEmailSubject = "[Redmine2.0] Your confirm email url";

        /// <summary>
        /// ResetPasswordSubject
        /// </summary>
        public const string ResetPasswordSubject = "[Redmine2.0] Reset password";

        /// <summary>
        /// NewMSAccountSubject
        /// </summary>
        public const string NewMSAccountSubject = "[Redmine2.0] Welcome to our system";
    }
}
