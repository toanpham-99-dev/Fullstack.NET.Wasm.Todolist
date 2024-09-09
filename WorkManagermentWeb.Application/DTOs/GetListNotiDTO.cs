namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// GetListNotiDTO
    /// </summary>
    public class GetListNotiDTO
    {
        /// <summary>
        /// UserId
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Language
        /// </summary>
        public string Language { get; set; } = string.Empty;

        /// <summary>
        /// Paging
        /// </summary>
        public PagingDTO Paging { get; set; } = new PagingDTO();
    }
}
