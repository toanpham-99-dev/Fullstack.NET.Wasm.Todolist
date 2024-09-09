namespace WorkManagermentWeb.Application.DTOs.Responses
{
    /// <summary>
    /// PostPutResponse
    /// </summary>
    /// <param name="Flag"></param>
    /// <param name="Message"></param>
    /// <param name="Id"></param>
    public record PostPutResponse(bool Flag, string? Message = null, Guid Id = default);
}
