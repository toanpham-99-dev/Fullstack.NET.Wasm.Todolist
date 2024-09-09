namespace WorkManagermentWeb.Application.DTOs.Responses
{
    /// <summary>
    /// GetListNotiResponse
    /// </summary>
    /// <param name="Total"></param>
    /// <param name="Notifications"></param>
    public record GetListNotiResponse(int Total, List<NotificationDTO> Notifications);
}
