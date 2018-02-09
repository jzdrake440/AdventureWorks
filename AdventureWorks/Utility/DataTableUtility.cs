using AdventureWorks.Models.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Services.Description;
using static AdventureWorks.Models.DataTables.DataTableServerSideRequest;

namespace AdventureWorks.Utility
{
    public static class DataTableUtility
    {
        public static Func<T, bool> SearchPredicate<T>(DataTableServerSideRequest request)
        {
            ParameterExpression pe = Expression.Parameter(typeof(T));
            Expression globalSearchValue = ToStringExpression(Expression.Constant(request.Search.Value));

            bool searchOnGlobal = !String.IsNullOrWhiteSpace(request.Search.Value);

            Expression masterExpression = null;
            foreach (DataTableColumn column in request.Columns)
            {
                if (!column.Searchable)
                    continue;

                Expression targetValue = ToStringExpression(Expression.Property(pe, column.Data)); //i.e. customer.AccountNumber

                if (!String.IsNullOrWhiteSpace(column.Search.Value))
                {
                    Expression columnSearchValue = ToStringExpression(Expression.Constant(column.Search.Value));
                    masterExpression = OrElseIgnoreNull(masterExpression, ContainsExpression(targetValue, columnSearchValue));
                }

                if (!searchOnGlobal)
                    continue;

                masterExpression = OrElseIgnoreNull(masterExpression, ContainsExpression(targetValue, globalSearchValue));
            }

            if (masterExpression == null)
                return c => true;
            else
                return Expression.Lambda<Func<T, bool>>(masterExpression, pe).Compile();
        }

        private static Expression ToStringExpression(Expression e)
        {
            return Expression.Call(e, "ToString", Type.EmptyTypes);
        }

        private static Expression ContainsExpression(Expression caller, Expression arg1)
        {

            return Expression.Call(caller, "Contains", Type.EmptyTypes, arg1);
        }

        private static Expression OrElseIgnoreNull(Expression left, Expression right)
        {
            if (left == null && right == null)
                throw new ArgumentException();

            if (left == null)
                return right;

            if (right == null)
                return left;

            return Expression.OrElse(left, right);
        }
    }
}