using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TulipInfo.Net.EFCore
{
    public class ExpressionFilterConvert<EntityType>
    {
        Type _typeOfEntity;
        IEntityTableMapping _entityTableMapping;
        public ExpressionFilterConvert(IEntityTableMapping entityTableMapping)
        {
            _entityTableMapping = entityTableMapping;
            _typeOfEntity = typeof(EntityType);
            if(_entityTableMapping.EntityFullName != _typeOfEntity.FullName)
            {
                throw new ArgumentException("Invalid EntityTableMapping, The EntityFullName Not Allowed.");
            }
        }

        public DbContextSql ConvertToSql(Expression<Func<EntityType, bool>> exp)
        {
            string sqlFilters = "";
            List<object> paramValueList = new List<object>();

            if (exp.Body is BinaryExpression)
            {
                ConvertToSql(exp.Body as BinaryExpression, ref sqlFilters, ref paramValueList);
            }
            else
            {
                throw new NotSupportedException("Not Supported Expression");
            }

            return new DbContextSql(sqlFilters, paramValueList);
        }

        private void ConvertToSql(BinaryExpression exp,
            ref string sqlFilters, ref List<object> paramValueList)
        {
            sqlFilters += "(";

            if (exp.Left is MemberExpression)
            {
                ConvertToSql(exp.Left as MemberExpression, ref sqlFilters, ref paramValueList);
            }
            else if (exp.Left is BinaryExpression)
            {
                ConvertToSql(exp.Left as BinaryExpression, ref sqlFilters, ref paramValueList);
            }
            else if (exp.Left is ConstantExpression)
            {
                ConvertToSql(exp.Left as ConstantExpression, ref sqlFilters, ref paramValueList);
            }
            else
            {
                ConvertToSql(exp.Left, ref sqlFilters, ref paramValueList);
            }

            sqlFilters += GetDbOperation(exp.NodeType);

            if (exp.Right is MemberExpression)
            {
                ConvertToSql(exp.Right as MemberExpression, ref sqlFilters, ref paramValueList);
            }
            else if (exp.Right is BinaryExpression)
            {
                ConvertToSql(exp.Right as BinaryExpression, ref sqlFilters, ref paramValueList);
            }
            else if (exp.Right is ConstantExpression)
            {
                ConvertToSql(exp.Right as ConstantExpression, ref sqlFilters, ref paramValueList);
            }
            else
            {
                ConvertToSql(exp.Right, ref sqlFilters, ref paramValueList);
            }

            sqlFilters += ")";
        }

        private void ConvertToSql(MemberExpression exp,
            ref string sqlFilters, ref List<object> paramValueList)
        {
            if (IsEntityPropertyExpression(exp))
            {
                string columnName = _entityTableMapping.GetColumn(exp.Member.Name).Name;
                sqlFilters += columnName;
            }
            else
            {
                string sqlValuePlace = "{" + paramValueList.Count + "}";
                sqlFilters += sqlValuePlace;
                paramValueList.Add(GetExpressionRuntimeValue(exp));
            }
        }

        private void ConvertToSql(ConstantExpression exp,
            ref string sqlFilters, ref List<object> paramValueList)
        {
            string sqlValuePlace = "{" + paramValueList.Count + "}";
            sqlFilters += sqlValuePlace;
            paramValueList.Add(exp.Value);
        }

        private void ConvertToSql(Expression exp,
            ref string sqlFilters, ref List<object> paramValueList)
        {
            string sqlValuePlace = "{" + paramValueList.Count + "}";
            sqlFilters += sqlValuePlace;
            paramValueList.Add(GetExpressionRuntimeValue(exp));
        }

        private bool IsEntityPropertyExpression(MemberExpression exp)
        {
            return exp.Member.ReflectedType.FullName == _typeOfEntity.FullName;
        }

        private object GetExpressionRuntimeValue(Expression exp)
        {
            return Expression.Lambda(exp).Compile().DynamicInvoke();
        }

        private string GetDbOperation(ExpressionType expType)
        {
            string exp = " ";
            switch (expType)
            {
                case ExpressionType.AndAlso:
                    exp = " AND ";
                    break;
                case ExpressionType.OrElse:
                    exp = " OR ";
                    break;
                case ExpressionType.Equal:
                    exp = "=";
                    break;
                case ExpressionType.NotEqual:
                    exp = "<>";
                    break;
                case ExpressionType.GreaterThan:
                    exp = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    exp = ">=";
                    break;
                case ExpressionType.LessThan:
                    exp = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    exp = "<=";
                    break;
                default:
                    throw new NotSupportedException($"Not Supported Expression Type:{expType.ToString()}");
            }
            return exp;
        }
    }
}
