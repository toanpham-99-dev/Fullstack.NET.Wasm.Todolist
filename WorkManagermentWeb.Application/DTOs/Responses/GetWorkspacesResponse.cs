namespace WorkManagermentWeb.Application.DTOs.Responses
{
    /// <summary>
    /// GetWorkspacesResponse
    /// </summary>
    /// <param name="WorkSpaces"></param>
    public record GetWorkspacesResponse(List<WorkSpaceDTO> WorkSpaces);
}
