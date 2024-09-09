namespace WorkManagermentWeb.Application.DTOs.Responses
{
    /// <summary>
    /// GenerateTokenResponse
    /// </summary>
    /// <param name="Flag"></param>
    /// <param name="Message"></param>
    /// <param name="Token"></param>
    public record GenerateTokenResponse(bool Flag, string Message, string Token = null!);
}
