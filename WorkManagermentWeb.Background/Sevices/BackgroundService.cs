using Hangfire;
using System.Linq.Expressions;
using WorkManagermentWeb.Application.Contracts;

namespace WorkManagermentWeb.Background.Sevices
{
    /// <summary>
    /// BackgroundService
    /// </summary>
    public class BackgroundService : IBackground
    {
        /// <summary>
        /// CreateEnqueue
        /// </summary>
        /// <param name="expression"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void CreateEnqueue(Expression<Action> expression)
        {
            BackgroundJob.Enqueue(expression);
        }

        /// <summary>
        /// CreateEnqueue
        /// </summary>
        /// <param name="expression"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void CreateEnqueue(Expression<Func<Task>> expression)
        {
            BackgroundJob.Enqueue(expression);
        }

        /// <summary>
        /// CreateOrUpdateRecurring
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="expression"></param>
        /// <param name="cronExpression"></param>
        /// <param name="timeZone"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void CreateOrUpdateRecurring(string jobId, Expression<Action> expression, string cronExpression, TimeZoneInfo? timeZone = null)
        {
            RecurringJob.AddOrUpdate(jobId, expression, cronExpression, new RecurringJobOptions { TimeZone = timeZone });
        }

        /// <summary>
        /// CreateOrUpdateRecurring
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="expression"></param>
        /// <param name="cronExpression"></param>
        /// <param name="timeZone"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void CreateOrUpdateRecurring(string jobId, Expression<Func<Task>> expression, string cronExpression, TimeZoneInfo timeZone)
        {
            RecurringJob.AddOrUpdate(jobId, expression, cronExpression, new RecurringJobOptions { TimeZone = timeZone });
        }

        /// <summary>
        /// CreateOrUpdateRecurring
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobId"></param>
        /// <param name="expression"></param>
        /// <param name="cronExpression"></param>
        /// <param name="timeZone"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void CreateOrUpdateRecurring<T>(string jobId, Expression<Func<T, Task>> expression, string cronExpression, TimeZoneInfo timeZone)
        {
            RecurringJob.AddOrUpdate<T>(jobId, expression, cronExpression, new RecurringJobOptions { TimeZone = timeZone });
        }

        /// <summary>
        /// CreateSchedule
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="dateTimeOffset"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void CreateSchedule(Expression<Action> expression, DateTimeOffset dateTimeOffset)
        {
            BackgroundJob.Schedule(expression, dateTimeOffset);
        }

        /// <summary>
        /// CreateSchedule
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="dateTimeOffset"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void CreateSchedule(Expression<Func<Task>> expression, DateTimeOffset dateTimeOffset)
        {
            BackgroundJob.Schedule(expression, dateTimeOffset);
        }
    }
}
