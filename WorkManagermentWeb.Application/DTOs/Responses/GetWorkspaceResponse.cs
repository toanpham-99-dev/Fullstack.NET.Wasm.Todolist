namespace WorkManagermentWeb.Application.DTOs.Responses
{
    /// <summary>
    /// GetWorkspaceResponse
    /// </summary>
    /// <param name="Flag"></param>
    /// <param name="Message"></param>
    /// <param name="WorkSpaceDTO"></param>
    public record GetWorkspaceResponse(bool Flag, string Message, WorkSpaceDTO? WorkSpaceDTO = null);
}
