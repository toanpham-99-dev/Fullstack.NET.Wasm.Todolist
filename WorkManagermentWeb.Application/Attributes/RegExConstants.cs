namespace WorkManagermentWeb.Application.Attributes
{
    /// <summary>
    /// RegExConstants
    /// </summary>
    public static class RegExConstants
    {
        /// <summary>
        /// UpperCaseRegEx
        /// </summary>
        public const string UpperCaseRegEx = "^.*(?=.*[A-Z]).*$";

        /// <summary>
        /// LowerCaseRegEx
        /// </summary>
        public const string LowerCaseRegEx = "^.*(?=.*[a-z]).*$";

        /// <summary>
        /// DigitRegEx
        /// </summary>
        public const string DigitRegEx = "^.*(?=.*[0-9]).*$";

        /// <summary>
        /// SpecCharRegEx
        /// </summary>
        public const string SpecCharRegEx = "^.*(?=.*\\W).*$";

        /// <summary>
        /// PasswordLengthRegEx
        /// </summary>
        public const string PasswordLengthRegEx = "^.*(?=.{8,16}).*$";

        /// <summary>
        /// UpperCaseRegEx
        /// </summary>
        public const string UpperCaseErrorMessage = "Must at least one upper case.";

        /// <summary>
        /// LowerCaseRegEx
        /// </summary>
        public const string LowerCaseErrorMessage = "Must at least one lower case.";

        /// <summary>
        /// DigitRegEx
        /// </summary>
        public const string DigitErrorMessage = "Must at least one digit.";

        /// <summary>
        /// SpecCharRegEx
        /// </summary>
        public const string SpecCharErrorMessage = "Must at least one special character.";

        /// <summary>
        /// PasswordLengthRegEx
        /// </summary>
        public const string PasswordLengthErrorMessage = "Must be 8-16 characters long.";

        /// <summary>
        /// UsernameRegEx
        /// </summary>
        public const string UsernameRegEx = "[a-zA-Z0-9]{5,40}";

        /// <summary>
        /// UsernameErrorMessage
        /// </summary>
        public const string UsernameErrorMessage = "Username just allows letters and numbers between 5 and 40 chars long.";

        /// <summary>
        /// FullNameRegEx
        /// </summary>
        public const string FullNameRegEx = "^[A-EGHIK-VXYÂĐỔÔÚỨ][a-eghik-vxyàáâãèéêìíòóôõùúýỳỹỷỵựửữừứưụủũợởỡờớơộổỗồốọỏịỉĩệểễềếẹẻẽặẳẵằắăậẩẫầấạảđ₫]+(\\s[A-EGHIK-VXYÂĐỔÔÚỨ][a-eghik-vxyàáâãèéêìíòóôõùúýỳỹỷỵựửữừứưụủũợởỡờớơộổỗồốọỏịỉĩệểễềếẹẻẽặẳẵằắăậẩẫầấạảđ₫]+)*$";
        /// <summary>
        /// FullNameErrorMessage
        /// </summary>
        public const string FullNameErrorMessage = "Full Name just allows letters and spaces and valid format (Ex: Nguyễn Văn An).";

        /// <summary>
        /// PhoneRegEx
        /// </summary>
        public const string PhoneRegEx = "^(\\+\\d{1,2}\\s?)?\\(?\\d{3}\\)?[\\s.-]?\\d{3}[\\s.-]?\\d{4}$";

        /// <summary>
        /// PhoneErrorMessage
        /// </summary>
        public const string PhoneErrorMessage = "Phone Number just allow format +84xxxxxxxxxx or +1xxxxxxxxxx or xxxxxxxxxx";

        /// <summary>
        /// DateLaterErrorMessage
        /// </summary>
        public const string DateLaterErrorMessage = "End date must be on or after start date.";

        /// <summary>
        /// DateEarlierErrorMessage
        /// </summary>
        public const string DateEarlierErrorMessage = "Start date must be on or earlier end date.";

        /// <summary>
        /// NumberRegEx
        /// </summary>
        public const string NumberRegEx = "^([0-9]{1,2})([.][0-9]{1,2})?$";

        /// <summary>
        /// NumberErrorMessage
        /// </summary>
        public const string NumberErrorMessage = "Number just allow format 1 or 0,2 or 1,02.";

        /// <summary>
        /// PasswordRegEx
        /// </summary>
        public const string PasswordRegEx = "^.*(?=.{8,})(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*\\W).*$";

        /// <summary>
        /// PasswordErrorMessage
        /// </summary>
        public const string PasswordErrorMessage = "Password must have upper & lower letters, digits, special chars and min length of 8 chars long.";
    }
}
