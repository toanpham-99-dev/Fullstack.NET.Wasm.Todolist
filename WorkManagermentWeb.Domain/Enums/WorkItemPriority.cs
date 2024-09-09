using System.ComponentModel.DataAnnotations;
using WorkManagermentWeb.Domain.Resources;

namespace WorkManagermentWeb.Domain.Enums
{
    /// <summary>
    /// WorkItemPriority
    /// </summary>
    public enum WorkItemPriority
    {
        /// <summary>
        /// Wont
        /// </summary>
        [Display(Description = nameof(Wont), ResourceType = typeof(Resource))]
        Wont,

        /// <summary>
        /// Could
        /// </summary>
        [Display(Description = nameof(Could), ResourceType = typeof(Resource))]
        Could,

        /// <summary>
        /// Should
        /// </summary>
        [Display(Description = nameof(Should), ResourceType = typeof(Resource))]
        Should,

        /// <summary>
        /// Must
        /// </summary>
        [Display(Description = nameof(Must), ResourceType = typeof(Resource))]
        Must,

        /// <summary>
        /// DoItNow
        /// </summary>
        [Display(Description = nameof(DoItNow), ResourceType = typeof(Resource))]
        DoItNow
    }
}
