using WorkManagermentWeb.Domain.Enums;

namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// BoardDTO
    /// </summary>
    public class BoardDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// CreatedAt
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// UpdatedAt
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// LastUpdateBy
        /// </summary>
        public string LastUpdateBy { get; set; } = string.Empty;

        /// <summary>
        /// StartDate
        /// </summary>
        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

        /// <summary>
        /// EndDate
        /// </summary>
        public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

        /// <summary>
        /// AssigneeId
        /// </summary>
        public string AssigneeId { get; set; } = string.Empty;

        /// <summary>
        /// AssigneeName
        /// </summary>
        public string AssigneeName { get; set; } = string.Empty;

        /// <summary>
        /// Status
        /// </summary>
        public BoardStatus Status { get; set; } = 0;

        /// <summary>
        /// WorkSpaceId
        /// </summary>
        public Guid WorkSpaceId { get; set; }

        /// <summary>
        /// Owner
        /// </summary>
        public UserDTO Owner { get; set; } = new();

        /// <summary>
        /// Members
        /// </summary>
        public List<UserDTO> Members { get; set; } = new List<UserDTO>();

        /// <summary>
        /// WorkItems
        /// </summary>
        public List<WorkItemDTO> WorkItems { get; set; } = new List<WorkItemDTO>();
    }
}
