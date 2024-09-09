using WorkManagermentWeb.Domain.Enums;
#nullable disable

namespace WorkManagermentWeb.Application.DTOs.Responses
{
    /// <summary>
    /// GetWorkItemSpecicalPropertiesInfoResponse
    /// </summary>
    public class GetPropertiesInfoResponse
    {
        /// <summary>
        /// Statuses
        /// </summary>
        public List<PropertyInfo<WorkItemStatus>> Statuses 
        {
            get { return InitData<WorkItemStatus>(); } 
        }

        /// <summary>
        /// Categories
        /// </summary>
        public List<PropertyInfo<WorkItemType>> Categories
        {
            get { return InitData<WorkItemType>(); }
        }

        /// <summary>
        /// Priorities
        /// </summary>
        public List<PropertyInfo<WorkItemPriority>> Priorities
        {
            get { return InitData<WorkItemPriority>(); }
        }

        /// <summary>
        /// BoardStatus
        /// </summary>
        public List<PropertyInfo<BoardStatus>> BoardStatus
        {
            get { return InitData<BoardStatus>(); }
        }

        /// <summary>
        /// InitData
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private List<PropertyInfo<T>> InitData<T>()
        {
            List<T> values = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            return values.Select(_ => new PropertyInfo<T>()
            {
                Id = _,
                Name = _!.ToString()!
            }).ToList();
        }
    }

    /// <summary>
    /// PropertyInfo
    /// </summary>
    public class PropertyInfo<T>
    {
        /// <summary>
        /// Id
        /// </summary>
        public T Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
