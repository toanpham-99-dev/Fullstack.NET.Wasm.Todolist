namespace WorkManagermentWeb.Application.DTOs.Responses
{
    /// <summary>
    /// RegistrationResponse
    /// </summary>
    /// <param name="Flag"></param>
    /// <param name="Message"></param>
    /// <param name="Id"></param>
    public record RegistrationResponse(bool Flag, string Message, string? Id = null);

}
