namespace WorkManagermentWeb.Application.DTOs.Responses
{
    /// <summary>
    /// MarkAllAsReadResponse
    /// </summary>
    /// <param name="Flag"></param>
    /// <param name="Message"></param>
    public record MarkAllAsReadResponse(bool Flag, string? Message = null);
}
