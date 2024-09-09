namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// SortingDTO
    /// </summary>
    public class SortingDTO
    {
        /// <summary>
        /// SortBy
        /// </summary>
        public string SortBy { get; set; } = nameof(SortBy);

        /// <summary>
        /// IsAscending
        /// </summary>
        public bool IsAscending { get; set; }
    }
}
