namespace WorkManagermentWeb.Application.DTOs.Responses
{
    /// <summary>
    /// GetRolesResponse
    /// </summary>
    /// <param name="RoleDTOs"></param>
    public record GetRolesResponse(List<RoleDTO> RoleDTOs);

    /// <summary>
    /// RoleDTO
    /// </summary>
    public class RoleDTO
    {
        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// DisplayName
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;
    }

}
