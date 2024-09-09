using WorkManagermentWeb.Application.DTOs.Responses;

namespace WorkManagermentWeb.Application.Contracts
{
    /// <summary>
    /// IGanttChart
    /// </summary>
    public interface IGanttChart
    {
        /// <summary>
        /// GetDataAsync
        /// </summary>
        /// <param name="boardIds"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        Task<GanttChartResponse> GetDataAsync(List<Guid> boardIds, string culture);
    }
}
