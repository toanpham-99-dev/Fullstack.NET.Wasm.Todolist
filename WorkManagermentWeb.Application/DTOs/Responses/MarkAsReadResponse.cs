namespace WorkManagermentWeb.Application.DTOs.Responses
{
    /// <summary>
    /// MarkAsReadResponse
    /// </summary>
    /// <param name="Flag"></param>
    /// <param name="Message"></param>
    public record MarkAsReadResponse(bool Flag, string? Message = null);
}
