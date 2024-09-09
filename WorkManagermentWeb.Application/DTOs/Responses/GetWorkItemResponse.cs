namespace WorkManagermentWeb.Application.DTOs.Responses
{
    /// <summary>
    /// GetWorkItemResponse
    /// </summary>
    /// <param name="Flag"></param>
    /// <param name="Message"></param>
    /// <param name="WorkItem"></param>
    public record GetWorkItemResponse(bool Flag, string? Message = null, WorkItemDTO? WorkItem = null);
}
