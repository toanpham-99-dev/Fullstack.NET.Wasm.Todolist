namespace WorkManagermentWeb.Application.DTOs.Responses
{
    /// <summary>
    /// GetBoardResponse
    /// </summary>
    /// <param name="Flag"></param>
    /// <param name="Message"></param>
    /// <param name="BoardDTO"></param>
    public record GetBoardResponse(bool Flag, string Message, BoardDTO? BoardDTO = null);
}
