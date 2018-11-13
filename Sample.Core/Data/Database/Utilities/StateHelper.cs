using Sample.Core.Data.Database.Entities;
using Sample.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace Sample.Core.Data.Database
{
    public class StateHelper
    {
        public static EntityState ConvertState(ObjectState state)
        {
            switch (state)
            {
                case ObjectState.Added:
                    return EntityState.Added;

                case ObjectState.Modified:
                    return EntityState.Modified;

                case ObjectState.Deleted:
                    return EntityState.Deleted;

                default:
                    return EntityState.Unchanged;
            }
        }

        public static ObjectState ConvertState(EntityState state)
        {
            switch (state)
            {
                case EntityState.Detached:
                    return ObjectState.Unchanged;

                case EntityState.Unchanged:
                    return ObjectState.Unchanged;

                case EntityState.Added:
                    return ObjectState.Added;

                case EntityState.Deleted:
                    return ObjectState.Deleted;

                case EntityState.Modified:
                    return ObjectState.Modified;

                default:
                    throw new ArgumentOutOfRangeException("state");
            }
        }

        public static IEnumerable<TEntity> SetObjectStateAll<TEntity>(IEnumerable<TEntity> entities, 
            ObjectState objectState, IEnumerable<Type> ignoreTypes = null) where TEntity : Entity
        {
            return entities.Select(entity => SetObjectStateAll(entity, objectState, ignoreTypes));
        }
        /*
        public static TEntity SetObjectStateAll<TEntity>(TEntity entity, IEnumerable<Type> includedTypes, ObjectState objectState) where TEntity : Entity
        {

        }*/

        public static TEntity SetObjectStateAll<TEntity>(TEntity entity, ObjectState objectState, IEnumerable<Type> ignoreTypes = null) where TEntity : Entity
        {
            var serializeStack = new List<object>();

            if (ignoreTypes == null)
            {
                ignoreTypes = new List<Type>();
            }

            SetObjectStateRecursive(entity, objectState, serializeStack, ignoreTypes);

            return entity;
        }

        private static void SetObjectStateRecursive(object entity, ObjectState objectState, List<object> serializeStack, IEnumerable<Type> ignoreTypes)
        {
            if (entity != null)
            {
                var type = entity.GetType();
                var entityType = typeof(Entity);
                var objectStateProperty = type.GetProperty("ObjectState", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                serializeStack.Add(entity);
                objectStateProperty.SetValue(entity, objectState);

                foreach (var property in type.GetProperties())
                {
                    var isPropertySubclassOfEntity = property.PropertyType.IsSubclassOf(entityType);
                    var isEnumerableEntity = ExpressionsHelper.IsPropertyNonStringEnumerable(property.PropertyType)
                        && ExpressionsHelper.GetElementType(property.PropertyType) != null &&
                        ExpressionsHelper.GetElementType(property.PropertyType).IsSubclassOf(entityType);
                    object propertyValue = null;

                    if (isPropertySubclassOfEntity || isEnumerableEntity)
                    {
                        propertyValue = property.GetValue(entity);
                    }

                    if (propertyValue != null)
                    {
                        if (isPropertySubclassOfEntity)
                        {
                            if (!IsCircularReference(property, propertyValue, serializeStack) && !ignoreTypes.Contains(property.PropertyType))
                            {
                                SetObjectStateRecursive(propertyValue, objectState, serializeStack, ignoreTypes);
                            }
                        }
                        else if (isEnumerableEntity)
                        {
                            var entityList = propertyValue as IEnumerable<object>;

                            if (entityList != null)
                            {
                                foreach (var entityFromList in entityList)
                                {
                                    if (!IsCircularReference(property, entityFromList, serializeStack) && !ignoreTypes.Contains(entityFromList.GetType()))
                                    {
                                        SetObjectStateRecursive(entityFromList, objectState, serializeStack, ignoreTypes);
                                    }
                                }
                            }
                        }
                    }                   
                }
            }
        }

        private static bool IsCircularReference(PropertyInfo property, object propertyValue, List<object> serializeStack)
        {
            if (propertyValue == null || ExpressionsHelper.IsSimpleType(propertyValue.GetType()))
            {
                return false;
            }

            return serializeStack.Contains(propertyValue);
        }
    }
}