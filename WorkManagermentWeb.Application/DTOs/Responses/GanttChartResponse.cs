using System.Text.Json.Serialization;

namespace WorkManagermentWeb.Application.DTOs.Responses
{
    /// <summary>
    /// GanttChartResponse
    /// </summary>
    public class GanttChartResponse
    {
        /// <summary>
        /// Data
        /// </summary>
        [JsonPropertyName("data")]
        public List<GanttData> Data { get; set; } = new List<GanttData>();

        /// <summary>
        /// Links
        /// </summary>
        [JsonPropertyName("links")]
        public List<GanttLink> Links { get; set; } = new List<GanttLink>();
    }

    /// <summary>
    /// GanttLink
    /// </summary>
    public class GanttLink
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Source
        /// </summary>
        [JsonPropertyName("source")]
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// Target
        /// </summary>
        [JsonPropertyName("target")]
        public string Target { get; set; } = string.Empty;

        /// <summary>
        /// Type
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = "1";
    }

    /// <summary>
    /// GanttData
    /// </summary>
    public class GanttData
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Title
        /// </summary>
        [JsonPropertyName("text")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Assignee
        /// </summary>
        [JsonPropertyName("assignee")]
        public string Assignee { get; set; } = string.Empty;

        /// <summary>
        /// StartDate
        /// </summary>
        [JsonPropertyName("start_date")]
        public string StartDate { get; set; } = string.Empty;

        /// <summary>
        /// DueDate
        /// </summary>
        [JsonPropertyName("end_date")]
        public string DueDate { get; set; } = string.Empty;

        /// <summary>
        /// WorkRemain
        /// </summary>
        [JsonPropertyName("work_remain")]
        public double WorkRemain { get; set; }

        /// <summary>
        /// Parent
        /// </summary>
        [JsonPropertyName("parent")]
        public string Parent { get; set; } = string.Empty;

        /// <summary>
        /// Priority
        /// </summary>
        [JsonPropertyName("priority")]
        public string Priority { get; set; } = string.Empty;

        /// <summary>
        /// Progress
        /// </summary>
        [JsonPropertyName("progress")]
        public double Progress { get; set; }

        /// <summary>
        /// IsOpen
        /// </summary>
        [JsonPropertyName("open")]
        public bool IsOpen { get; set; } = true;

        /// <summary>
        /// Type
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }
}
