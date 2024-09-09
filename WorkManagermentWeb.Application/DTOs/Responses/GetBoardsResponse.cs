namespace WorkManagermentWeb.Application.DTOs.Responses
{
    /// <summary>
    /// GetBoardsResponse
    /// </summary>
    /// <param name="boards"></param>
    public record GetBoardsResponse(List<BoardDTO> Boards);
}
