namespace WorkManagermentWeb.Application.DTOs.Responses
{
    /// <summary>
    /// PostWorkItemResponse
    /// </summary>
    /// <param name="Flag"></param>
    /// <param name="Message"></param>
    /// <param name="Id"></param>
    public record PostWorkItemResponse(bool Flag, string? Message = null, Guid Id = default);
}
