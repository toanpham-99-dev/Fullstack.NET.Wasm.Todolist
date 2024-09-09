namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// PagingDTO
    /// </summary>
    public class PagingDTO
    {
        /// <summary>
        /// Skip
        /// </summary>
        public int Skip { get; set; } = 0;

        /// <summary>
        /// Take
        /// </summary>
        public int Take { get; set; } = 1000;
    }
}
