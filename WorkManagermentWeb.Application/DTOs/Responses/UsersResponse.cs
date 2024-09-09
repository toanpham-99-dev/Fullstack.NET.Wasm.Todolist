namespace WorkManagermentWeb.Application.DTOs.Responses
{
    /// <summary>
    /// UsersResponse
    /// </summary>
    /// <param name="Users"></param>
    /// <param name="Total"></param>
    public record UsersResponse(List<UserDTO> Users, int Total);
}
