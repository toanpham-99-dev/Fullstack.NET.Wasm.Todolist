using System.ComponentModel.DataAnnotations;

namespace WorkManagermentWeb.Domain.Entities.Base
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// CreatedAt
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// UpdatedAt
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; } = 0;

        /// <summary>
        /// IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}
