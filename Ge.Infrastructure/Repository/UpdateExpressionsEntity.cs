using System;
using System.Linq.Expressions;

namespace Ge.Infrastructure.Repository
{
    public class UpdateExpressionsEntity<T>
    {
        public UpdateExpressionsEntity()
        {

        }

        public UpdateExpressionsEntity(Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, T>> updateExpression)
        {
            FilterExpression = filterExpression;
            UpdateExpression = updateExpression;
        }

        public Expression<Func<T, bool>> FilterExpression { get; set; }

        public Expression<Func<T, T>> UpdateExpression { get; set; }
    }
}
