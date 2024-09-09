using System.ComponentModel.DataAnnotations;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Domain.Enums;

namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// UserDTO
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// FullName
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// IsEmailConfirmed
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// Phone
        /// </summary>
        public string? Phone { get; set; } = string.Empty;

        /// <summary>
        /// IsPhoneConfirmed
        /// </summary>
        public bool IsPhoneConfirmed { get; set; }

        /// <summary>
        /// ActiveStatus
        /// </summary>
        public bool ActiveStatus { get; set; }

        /// <summary>
        /// AccountType
        /// </summary>
        public AccountType AccountType { get; set; }

        /// <summary>
        /// ExternalAccountConnected
        /// </summary>
        public bool ExternalAccountConnected { get; set; }

        /// <summary>
        /// Role
        /// </summary>
        public RoleDTO Role { get; set; } = new();

        /// <summary>
        /// Members
        /// </summary>
        public List<BoardDTO> Projects { get; set; } = new List<BoardDTO>();

        /// <summary>
        /// WorkItems
        /// </summary>
        public List<WorkItemDTO> WorkItems { get; set; } = new List<WorkItemDTO>();
    }
}
