using System.ComponentModel.DataAnnotations;
using WorkManagermentWeb.Domain.Enums;

namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// WorkSpaceDTO
    /// </summary>
    public class WorkSpaceDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Name
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Owner
        /// </summary>
        [Required]
        public string Owner { get; set; } = string.Empty;

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
        /// Status
        /// </summary>
        public WorkSpaceStatus Status { get; set; }

        /// <summary>
        /// Members
        /// </summary>
        public List<UserDTO> Members { get; set; } = new List<UserDTO>();

        /// <summary>
        /// Boards
        /// </summary>
        public List<BoardDTO> Boards { get; set; } = new List<BoardDTO>();
    }
}
