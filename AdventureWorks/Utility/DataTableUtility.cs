using AdventureWorks.Models.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Services.Description;
using static AdventureWorks.Models.DataTables.DataTableServerSideRequest;

namespace AdventureWorks.Utility
{
    public static class DataTableUtility
    {
        public static List<T> FilterData<T>(DataTableServerSideRequest request, IEnumerable<T> data)
        {
            return data.Where(DataTableUtility.SearchPredicate<T>(request)).ToList();
        }

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

                if (column.Data == null)
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

        public static void OrderData<T>(DataTableServerSideRequest request, List<T> data)
        {
            List<DataTableOrderDataOption> options = new List<DataTableOrderDataOption>();

            foreach (DataTableOrder order in request.Order) //simplify the in data for the compare delegate
            {
                if (!request.Columns[order.Column].Orderable)
                    continue;

                if (request.Columns[order.Column].Data == null)
                    continue;

                var prop = typeof(T).GetProperty(
                    request.Columns[order.Column].Data, 
                    BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);

                bool isNullable = prop.PropertyType.IsGenericType && 
                    typeof(Nullable<>) == prop.PropertyType.GetGenericTypeDefinition();

                var underlyingType = isNullable ?
                    prop.PropertyType.GetGenericArguments()[0] :
                    prop.PropertyType;

                var option = new DataTableOrderDataOption
                {
                    Property = prop,
                    FromNullable = isNullable,
                    Comparable = underlyingType.GetInterface("IComparable")?.GetMethod("CompareTo"),
                    Direction = order.Dir
                };


                if (option.Comparable == null) //ensure that the dynamic field is properly comparable
                    throw new ArgumentException(
                        String.Format(
                            "Attempting to sort on uncomparable property {0}.{1}; Implement the IComparable interface.",
                            typeof(T).FullName,
                            option.Property.Name)
                        );

                options.Add(option);
            }

            data.Sort((t1, t2) =>
            {
                foreach (DataTableOrderDataOption option in options)
                {
                    var propVal1 = option.Property.GetValue(t1);
                    var propVal2 = option.Property.GetValue(t2);

                    if (propVal1 == null && propVal2 == null)
                        continue;

                    if (propVal1 == null)
                        return option.Dir(-1);

                    if (propVal2 == null)
                        return option.Dir(1);

                    int c = (int)option.Comparable.Invoke(propVal1, new object[] { propVal2 });

                    if (c != 0)
                        return option.Dir(c);
                }
                return 0;
            });
        }

        private class DataTableOrderDataOption
        {
            public const string DIRECTION_ASC = "asc";
            public const string DIRECTION_DESC = "desc";

            public PropertyInfo Property { get; set; }
            public MethodInfo Comparable { get; set; }
            public bool FromNullable { get; set; }
            public string Direction { get; set; }

            public int Dir(int c) //change direction as necessary
            {
                return Direction == DIRECTION_ASC ? c : c * -1;
            }
        }
    }
}