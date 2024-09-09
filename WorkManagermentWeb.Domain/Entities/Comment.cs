using System.ComponentModel.DataAnnotations;
using WorkManagermentWeb.Domain.Entities.Base;
#nullable disable

namespace WorkManagermentWeb.Domain.Entities
{
    /// <summary>
    /// Comment
    /// </summary>
    public class Comment : BaseModel
    {
        /// <summary>
        /// Content
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// LikeCount
        /// </summary>
        [Required]
        public int LikeCount { get; set; }

        /// <summary>
        /// WorkItemId
        /// </summary>
        [Required]
        public Guid WorkItemId { get; set; }

        /// <summary>
        /// ParentId
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// WorkItem
        /// </summary>
        public virtual WorkItem WorkItem { get; set; }

        /// <summary>
        /// ParentComment
        /// </summary>
        public virtual Comment ParentComment { get; set; }

        /// <summary>
        /// SubComments
        /// </summary>
        public virtual ICollection<Comment> SubComments { get; set; }
    }
}
