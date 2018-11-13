using Sample.Core.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Sample.Core.Expressions
{
    public class ExpressionsHelper
    {

        public static object ConvertTo(object value, Type convertToType)
        {
            var nullableConvertType = Nullable.GetUnderlyingType(convertToType);

            if (nullableConvertType != null)
            {
                var defaultValue = convertToType.GetTypeInfo().IsValueType ? Activator.CreateInstance(convertToType) : null;

                return value == null ? defaultValue : Convert.ChangeType(value, nullableConvertType);
            }
            else
            {
                return Convert.ChangeType(value, convertToType);
            }
        }
        public static bool HasDynamicProperty(dynamic obj, string name)
        {
            Type objType = obj.GetType();

            if (objType == typeof(ExpandoObject))
            {
                return ((IDictionary<string, object>)obj).ContainsKey("View");
            }
            else if (objType == typeof(JObject))
            {
                return ((JObject)obj).Property(name) != null;
            }

            return objType.GetProperty(name) != null;
        }

        public static Expression<Func<TEntity, bool>> GetCompareQuery<TEntity, TValue>(string propertyPath, TValue propertyValue, string comparisonMethod = "Contains")
        {
            if (!string.IsNullOrWhiteSpace(propertyPath))
            {
                var entityType = typeof(TEntity);
                var searchEntityType = entityType;
                var propertyNames = propertyPath.Split('.');
                PropertyInfo searchProperty = null;

                for (int i = 0; i < propertyNames.Length; i++)
                {
                    searchProperty = searchEntityType
                        .GetProperties()
                        .FirstOrDefault(property => property.Name.Equals(propertyNames[i], StringComparison.OrdinalIgnoreCase));

                    if (searchProperty == null)
                    {
                        return null;
                    }
                    else
                    {
                        searchEntityType = searchProperty.PropertyType;
                    }
                }

                if (searchProperty != null)
                {
                    var source = Expression.Parameter(typeof(TEntity), "entity");

                    // create entity.Id portion of lambda expression
                    var property = Expression.Property(source, propertyNames[0]);

                    if (propertyNames.Length > 1)
                    {
                        for (int i = 1; i < propertyNames.Length; i++)
                        {
                            property = Expression.Property(property, propertyNames[i]);
                        }
                    }
                    /*
                    Expression left = property,
                        right = Expression.Constant(propertyValue, typeof(TValue));

                    if (left.Type != right.Type)
                    {
                        var nullableType = Nullable.GetUnderlyingType(left.Type);

                        if (nullableType != null)
                        {
                            right = Expression.Convert(right, left.Type);
                        }
                        else
                        {
                            left = Expression.Convert(left, right.Type);
                        }
                    }
                    
                     
                    // create entity.Id == 'id' portion of lambda expression
                    var body = Expression.Equal(left, right);
                    // finally create entire expression - entity => entity.Id == 'id'
                    var expr = Expression.Lambda<Func<TEntity, bool>>(body, source);

                    return expr;
                     */

                    Expression comparisonCall = null;

                    if (searchProperty.PropertyType == typeof(string))
                    {
                        var containsMethod = typeof(string).GetMethod(comparisonMethod, new[] { typeof(string) });

                        if (containsMethod != null)
                        {
                            var comparisonText = Expression.Constant(propertyValue);

                            comparisonCall = Expression.Call(property, containsMethod, comparisonText);
                        }
                    }
                    else if (searchProperty.PropertyType == typeof(Guid))
                    {
                        var equalsMethod = typeof(Guid).GetMethod("Equals", new[] { typeof(Guid) });
                        var guidToCompare = Guid.Empty;

                        if (Guid.TryParse(propertyValue.ToString(), out guidToCompare))
                        {
                            comparisonCall = Expression.Call(property, equalsMethod, Expression.Constant(guidToCompare));
                        }
                    }
                    else if (searchProperty.PropertyType.IsEnum)
                    {
                        var queryEnumValue = EnumHelper.GetValue(searchProperty.PropertyType, propertyValue.ToString());

                        if (queryEnumValue != null)
                        {
                            var queryEnumConstant = Expression.Constant(queryEnumValue);

                            comparisonCall = Expression.Equal(property, queryEnumConstant);
                        }
                    }
                    else if (NumberHelper.IsNumeric(searchProperty.PropertyType) && NumberHelper.IsNumeric(propertyValue.ToString()))
                    {
                        /*
                        var numberValue = float.Parse(propertyValue.ToString());
                        var numberValueConstant = Expression.Constant(numberValue);

                        comparisonCall = Expression.Equal(property, numberValueConstant);
                        */
                    }

                    var functionType = typeof(Func<,>).MakeGenericType(entityType, typeof(bool));

                    if (comparisonCall != null)
                    {
                        return (Expression<Func<TEntity, bool>>)Expression.Lambda(functionType, comparisonCall, source);
                    }
                }
            }

            return null;
        }

        public static Expression<Func<TEntity, bool>> GetEqualsQuery<TEntity, TValue>(string propertyPath, TValue propertyValue)
        {/*
            if (!string.IsNullOrWhiteSpace(propertyPath))
            {
                var entityType = typeof(TEntity);
                var propertyNames = propertyPath.Split('.');
                PropertyInfo searchProperty = null;

                for (int i = 0; i < propertyNames.Length; i++)
                {
                    searchProperty = entityType
                        .GetProperties()
                        .FirstOrDefault(property => property.Name.StartsWith(propertyNames[i], StringComparison.OrdinalIgnoreCase));

                    if (searchProperty == null)
                    {
                        return null;
                    }
                    else
                    {
                        entityType = searchProperty.PropertyType;
                    }
                }

                if (searchProperty != null)
                {
                    var source = Expression.Parameter(typeof(TEntity), "entity");

                    // create entity.Id portion of lambda expression
                    var property = Expression.Property(source, propertyNames[0]);

                    if (propertyNames.Length > 1)
                    {
                        for (int i = 1; i < propertyNames.Length; i++)
                        {
                            property = Expression.Property(property, propertyNames[i]);
                        }
                    }

                    Expression left = property,
                        right = Expression.Constant(propertyValue, typeof(TValue));

                    if (left.Type != right.Type)
                    {
                        var nullableType = Nullable.GetUnderlyingType(left.Type);

                        if (nullableType != null)
                        {
                            right = Expression.Convert(right, left.Type);
                        }
                        else
                        {
                            left = Expression.Convert(left, right.Type);
                        }
                    }

                    // create entity.Id == 'id' portion of lambda expression
                    var body = Expression.Equal(left, right);
                    // finally create entire expression - entity => entity.Id == 'id'
                    var expr = Expression.Lambda<Func<TEntity, bool>>(body, source);

                    return expr;
                }
            }

            return null;*/

            return GetCompareQuery<TEntity, TValue>(propertyPath, propertyValue, "Equals");
        }

        public static Type GetNullableType(Type TypeToConvert)
        {
            // Abort if no type supplied
            if (TypeToConvert != null)
            {
                // If the given type is already nullable, just return it
                if (IsTypeNullable(TypeToConvert))
                {
                    return TypeToConvert;
                }

                // If the type is a ValueType and is not System.Void, convert it to a Nullable<Type>
                if (TypeToConvert.IsValueType && TypeToConvert != typeof(void))
                {
                    return typeof(Nullable<>).MakeGenericType(TypeToConvert);
                }
            }

            // Done - no conversion
            return null;
        }

        public static bool IsAnonymousType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            // HACK: The only way to detect anonymous types right now.
            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                && type.IsGenericType && type.Name.Contains("AnonymousType")
                && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
        }

        public static bool IsSimpleType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimpleType(type.GetGenericArguments()[0]);
            }

            return type.IsPrimitive
              || type.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal))
              || type.Equals(typeof(Guid));
        }

        public static bool? IsPropertyVirtual(PropertyInfo self)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            bool? found = null;

            foreach (var method in self.GetAccessors())
            {
                if (found.HasValue)
                {
                    if (found.Value != method.IsVirtual)
                        return null;
                }
                else
                {
                    found = method.IsVirtual;
                }
            }

            return found;
        }

        public static bool IsTypeNullable(Type typeToTest)
        {
            // Abort if no type supplied
            if (typeToTest == null)
            {
                return false;
            }

            // If this is not a value type, it is a reference type, so it is automatically nullable
            //  (NOTE: All forms of Nullable<T> are value types)
            if (!typeToTest.IsValueType)
            {
                return true;
            }

            // Report whether TypeToTest is a form of the Nullable<> type
            return typeToTest.IsGenericType && typeToTest.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }

        public static object GetValueFromJsonString(Type valueType, string jsonValue)
        {
            if (string.IsNullOrWhiteSpace(jsonValue))
            {
                return GetDefault(valueType);
            }

            object value = null;

            if (valueType.Namespace.StartsWith("System"))
            {
                if (valueType == typeof(DateTime?) || valueType == typeof(DateTime))
                {
                    value = DateTime.Parse(jsonValue, null, DateTimeStyles.RoundtripKind);
                }
                else if (valueType == typeof(DateTimeOffset?) || valueType == typeof(DateTimeOffset))
                {
                    value = DateTimeOffset.Parse(jsonValue, null, DateTimeStyles.RoundtripKind);
                }
                else if (valueType == typeof(Guid?) || valueType == typeof(Guid))
                {
                    value = Guid.Parse(jsonValue);
                }
                else
                {
                    value = TypeDescriptor.GetConverter(valueType).ConvertFromInvariantString(jsonValue);
                }
            }
            else
            {
                value = JsonConvert.DeserializeObject(jsonValue, valueType);
            }

            return value;
        }

        public static bool IsNullable(Type type, object value)
        {
            if (value == null)
            {
                return true; // obvious
            }

            //Type type = typeof(T);
            if (!type.IsValueType)
            {
                return true; // ref-type
            }

            if (Nullable.GetUnderlyingType(type) != null)
            {
                return true; // Nullable<T>
            }

            return false; // value-type
        }

        public static bool IsPropertyNonStringEnumerable(Type type)
        {
            if (type == null || type == typeof(string))
                return false;
            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        public static Type GetElementType(Type seqType)
        {
            var ienum = FindIEnumerable(seqType);

            if (ienum == null)
            {
                return seqType;
            }

            return ienum.GetGenericArguments()[0];
        }

        public static bool HasBaseType(object mainObject, Type baseType)
        {
            if (mainObject != null)
            {
                return HasBaseType(mainObject.GetType(), baseType);
            }

            return false;
        }

        public static bool HasBaseType(Type mainType, Type baseType)
        {
            if (mainType.BaseType == null)
            {
                return false;
            }

            if (mainType.BaseType == baseType)
            {
                return true;
            }

            return HasBaseType(mainType.BaseType, baseType);
        }

        private static Type FindIEnumerable(Type seqType)
        {
            if (seqType == null || seqType == typeof(string))
                return null;
            if (seqType.IsArray)
                return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());
            if (seqType.IsGenericType)
            {
                foreach (Type arg in seqType.GetGenericArguments())
                {
                    Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
                    if (ienum.IsAssignableFrom(seqType))
                    {
                        return ienum;
                    }
                }
            }

            var ifaces = seqType.GetInterfaces();

            if (ifaces != null && ifaces.Length > 0)
            {
                foreach (Type iface in ifaces)
                {
                    Type ienum = FindIEnumerable(iface);
                    if (ienum != null) return ienum;
                }
            }

            if (seqType.BaseType != null && seqType.BaseType != typeof(object))
            {
                return FindIEnumerable(seqType.BaseType);
            }

            return null;
        }

        public static bool HasTypeProperty(Type type, string propertyName, bool ignoreCase = true)
        {
            PropertyInfo propertyInfo = null;

            if (ignoreCase)
            {
                propertyInfo = type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            }
            else
            {
                propertyInfo = type.GetProperty(propertyName);
            }

            return propertyInfo != null;
        }

        public static Expression GetNavigationPropertyExpression(Expression parameter, Expression resultExpression, params string[] properties)
        {
            Expression childParameter, navigationPropertyPredicate;
            Type childType = null;

            if (properties.Count() > 1)
            {
                //build path
                parameter = Expression.Property(parameter, properties[0]);
                var isCollection = typeof(IEnumerable).IsAssignableFrom(parameter.Type);
                //if it´s a collection we later need to use the predicate in the methodexpressioncall
                if (isCollection)
                {
                    childType = parameter.Type.GetGenericArguments()[0];
                    childParameter = Expression.Parameter(childType, childType.Name);
                }
                else
                {
                    childParameter = parameter;
                }
                //skip current property and get navigation property expression recursivly
                var innerProperties = properties.Skip(1).ToArray();
                var selectType = childType;
                navigationPropertyPredicate = GetNavigationPropertyExpression(childParameter, parameter, innerProperties);

                if (isCollection)
                {
                    //build methodexpressioncall
                    var returnParameterExpression = Expression.Parameter(childType);
                    var functionType = typeof(Func<,>).MakeGenericType(childType, parameter.Type);

                    navigationPropertyPredicate = Expression.Lambda(functionType, parameter, returnParameterExpression);
                    resultExpression = BuildSubQuery(parameter, childType, navigationPropertyPredicate);
                }
                else
                {
                    resultExpression = navigationPropertyPredicate;
                }
            }

            return resultExpression;
        }

        public static Expression BuildPredicateForSubQuery(Type childType, LambdaExpression parameter)
        {
            var lambdaParameter = ParameterExpression.Parameter(childType, childType.Name);
            //var childParameterExpression = Expression.Parameter(childType, lambdaParameter);
            var functionType = typeof(Func<,>).MakeGenericType(childType, parameter.Type);

            return Expression.Lambda(functionType, parameter, lambdaParameter);
        }

        public static LambdaExpression BuildPredicateForSubQuery(Type parentType, Type propertyType, Expression parameter)
        {
            var resultParameterVisitor = new ParameterVisitor();
            resultParameterVisitor.Visit(parameter);

            var parentParameter = (ParameterExpression)resultParameterVisitor.Parameter;
            var functionType = typeof(Func<,>).MakeGenericType(parentType, propertyType);

            return Expression.Lambda(functionType, parameter, parentParameter);
        }

        public static Expression BuildSubQuery(Expression parameter, Type childType, Expression predicate)
        {
            var selectMethod = typeof(Enumerable).GetMethods().FirstOrDefault(m => m.Name == "Select" && m.GetParameters().Length == 2);
            var paramaterLambda = parameter as LambdaExpression;
            var predicateLambda = predicate as LambdaExpression;
            Type predicateCollectionType = predicate.Type;

            if (predicateLambda != null)
            {
                predicateCollectionType = predicateLambda.ReturnType;
            }

            if (paramaterLambda != null)
            {
                parameter = paramaterLambda.Body;
            }

            selectMethod = selectMethod.MakeGenericMethod(childType, predicateCollectionType);
            predicate = Expression.Call(selectMethod, parameter, predicate);

            return MakeLambda(parameter, predicate);
        }

        public static Expression MakeLambda(Expression parameter, Expression predicate)
        {
            var resultParameterVisitor = new ParameterVisitor();
            resultParameterVisitor.Visit(parameter);
            var resultParameter = resultParameterVisitor.Parameter;

            return Expression.Lambda(predicate, (ParameterExpression)resultParameter);
        }

        public static ParameterExpression GetParamaterFromExpression(Expression parameter)
        {
            var resultParameterVisitor = new ParameterVisitor();
            resultParameterVisitor.Visit(parameter);
            var resultParameter = resultParameterVisitor.Parameter as ParameterExpression;

            return resultParameter;
        }

        public static Expression GetInnerExpression(Expression parameter, string innerMethodName)
        {
            var parameterLambda = parameter as LambdaExpression;

            if (parameterLambda != null)
            {
                parameter = parameterLambda.Body;
            }

            var callRemover = new CallRemover(innerMethodName);

            parameter = callRemover.Visit(parameter);

            return parameter;
        }

        public static bool IsExpressionLambda(Expression expression)
        {
            var lambdaExpression = expression as LambdaExpression;

            return lambdaExpression != null;
        }

        private class ParameterVisitor : System.Linq.Expressions.ExpressionVisitor
        {
            public Expression Parameter
            {
                get;
                private set;
            }
            protected override Expression VisitParameter(ParameterExpression node)
            {
                Parameter = node;
                return node;
            }
        }

        private class CallRemover : System.Linq.Expressions.ExpressionVisitor
        {
            private string _methodToRemoveName;
            public CallRemover(string methodToRemoveName)
                : base()
            {
                _methodToRemoveName = methodToRemoveName;
            }
            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (node.Method.DeclaringType != typeof(Enumerable) && node.Method.DeclaringType != typeof(Queryable))
                {
                    return base.VisitMethodCall(node);
                }

                if (node.Method.Name != _methodToRemoveName)
                {
                    return base.VisitMethodCall(node);
                }

                //eliminate the method call from the expression tree by returning the object of the call.
                return base.Visit(node.Arguments.Last());
            }
        }

    }
}
