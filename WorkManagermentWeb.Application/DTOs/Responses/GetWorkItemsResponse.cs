namespace WorkManagermentWeb.Application.DTOs.Responses
{
    /// <summary>
    /// GetWorkItemsResponse
    /// </summary>
    /// <param name="Total"></param>
    /// <param name="WorkItems"></param>
    public record GetWorkItemsResponse(int Total, List<WorkItemDTO> WorkItems);
}
