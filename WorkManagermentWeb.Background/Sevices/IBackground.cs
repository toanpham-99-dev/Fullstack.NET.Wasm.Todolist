using System.Linq.Expressions;

namespace WorkManagermentWeb.Background.Sevices
{
    /// <summary>
    /// IBackground
    /// </summary>
    public interface IBackground
    {
        /// <summary>
        /// CreateEnqueue
        /// </summary>
        /// <param name="expression"></param>
        void CreateEnqueue(Expression<Action> expression);

        /// <summary>
        /// CreateEnqueue
        /// </summary>
        /// <param name="expression"></param>
        void CreateEnqueue(Expression<Func<Task>> expression);

        /// <summary>
        /// CreateSchedule
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="dateTimeOffset"></param>
        void CreateSchedule(Expression<Action> expression, DateTimeOffset dateTimeOffset);

        /// <summary>
        /// CreateSchedule
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="dateTimeOffset"></param>
        void CreateSchedule(Expression<Func<Task>> expression, DateTimeOffset dateTimeOffset);

        /// <summary>
        /// CreateOrUpdateRecurring
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="expression"></param>
        /// <param name="cronExpression"></param>
        /// <param name="timeZone"></param>
        void CreateOrUpdateRecurring(string jobId, Expression<Action> expression, string cronExpression, TimeZoneInfo timeZone);

        /// <summary>
        /// CreateOrUpdateRecurring
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="expression"></param>
        /// <param name="cronExpression"></param>
        /// <param name="timeZone"></param>
        void CreateOrUpdateRecurring(string jobId, Expression<Func<Task>> expression, string cronExpression, TimeZoneInfo timeZone);

        /// <summary>
        /// CreateOrUpdateRecurring
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobId"></param>
        /// <param name="expression"></param>
        /// <param name="cronExpression"></param>
        /// <param name="timeZone"></param>
        void CreateOrUpdateRecurring<T>(string jobId, Expression<Func<T, Task>> expression, string cronExpression, TimeZoneInfo timeZone);

    }
}
