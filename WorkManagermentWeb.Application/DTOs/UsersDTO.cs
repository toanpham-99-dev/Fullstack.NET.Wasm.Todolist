namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// UsersDTO
    /// </summary>
    public class UsersDTO
    {
        /// <summary>
        /// SearchInput
        /// </summary>
        public string? SearchInput { get; set; }

        /// <summary>
        /// RoleName
        /// </summary>
        public string RoleName { get; set; } = string.Empty;

        /// <summary>
        /// ActiveStatus
        /// </summary>
        public bool? ActiveStatus { get; set; } = null;

        /// <summary>
        /// BoardId
        /// </summary>
        public Guid BoardId { get; set; } = Guid.Empty;

        /// <summary>
        /// WorkSpaceId
        /// </summary>
        public Guid WorkSpaceId { get; set; } = Guid.Empty;

        /// <summary>
        /// Paging
        /// </summary>
        public PagingDTO Paging { get; set; } = new PagingDTO();
    }
}
