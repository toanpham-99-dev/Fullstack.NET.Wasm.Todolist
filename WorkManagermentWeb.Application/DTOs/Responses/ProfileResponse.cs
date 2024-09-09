namespace WorkManagermentWeb.Application.DTOs.Responses
{
    /// <summary>
    /// ProfileResponse
    /// </summary>
    /// <param name="Flag"></param>
    /// <param name="Message"></param>
    /// <param name="User"></param>
    public record ProfileResponse(bool Flag, string Message, UserDTO? User = null);
}
