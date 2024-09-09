using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkManagermentWeb.Application.Constants;
using WorkManagermentWeb.Application.Contracts;
using WorkManagermentWeb.Application.DTOs.Responses;

namespace WorkManagermentWeb.WebApi.Controllers
{
    /// <summary>
    /// GanttChartController
    /// </summary>
    /// <param name="ganttChart"></param>
    [Authorize(AuthorizationConstants.TwoAuthPolicy)]
    [Route($"{ApiRouteConstants.PrefixUrl}/{ApiRouteConstants.GanttChart}")]
    [ApiController]
    public class GanttChartController(IGanttChart ganttChart) : ControllerBase
    {
        /// <summary>
        /// Get
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<GanttChartResponse>> Get([FromQuery] List<Guid> ids, [FromQuery] string culture)
        {
            GanttChartResponse response = await ganttChart.GetDataAsync(ids, culture);
            return Ok(response);
        }
    }
}
