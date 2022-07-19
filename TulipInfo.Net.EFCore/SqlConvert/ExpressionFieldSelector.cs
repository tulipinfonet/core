using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TulipInfo.Net.EFCore
{
    public class ExpressionFieldSelector<EntityType>
    {
        IEntityTableMapping _entityTableMapping;
        public ExpressionFieldSelector(IEntityTableMapping entityTableMapping)
        {
            _entityTableMapping = entityTableMapping;
            if (_entityTableMapping.EntityFullName != typeof(EntityType).FullName)
            {
                throw new ArgumentException("Invalid EntityTableMapping, The EntityFullName Not Allowed.");
            }
        }

        public IEnumerable<KeyValuePair<string, object>> GetFieldAndValues(Expression<Func<EntityType>> initExp)
        {
            var exp = initExp.Body as MemberInitExpression;
            if (exp == null)
            {
                throw new NotSupportedException("Not Supported Expression");
            }

            List<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>(exp.Bindings.Count);
            foreach (MemberAssignment bind in exp.Bindings)
            {
                string propertyName = bind.Member.Name;
                string columnName = _entityTableMapping.GetColumn(propertyName).Name;
                object value = null;
                if (bind.Expression is ConstantExpression)
                {
                    value = ((ConstantExpression)bind.Expression).Value;
                }
                else
                {
                    value = Expression.Lambda(bind.Expression).Compile().DynamicInvoke();
                }
                result.Add(new KeyValuePair<string, object>(columnName, value));
            }

            return result;
        }
    }
}
