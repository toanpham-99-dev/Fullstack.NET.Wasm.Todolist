namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// UserSession
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    /// <param name="Email"></param>
    /// <param name="Role"></param>
    public record UserSession(string? Id, string? Name, string? Email, string? Role);
}
