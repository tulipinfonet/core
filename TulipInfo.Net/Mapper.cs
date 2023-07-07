using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;

namespace TulipInfo.Net
{
    public static class Mapper
    {
        public static TargetType Map<TargetType>(object source)
            where TargetType : class
        {
            Type sourceType = source.GetType();
            Type targetType = typeof(TargetType);
            object target = Activator.CreateInstance(targetType);

            Map(source, target, sourceType, targetType);

            return (TargetType)target;
        }

        public static void Map(object source, object target)
        {
            Map(source, target, source.GetType(), target.GetType());
        }

        public static TargetType? Map<TargetType>(IDictionary<string,object?> source)
            where TargetType : class
        {
            Type targetType = typeof(TargetType);
            object? target = Activator.CreateInstance(targetType);

            if (target != null)
            {
                Map(source, target);
                return (TargetType)target;
            }

            return null;
        }

        public static void Map(IDictionary<string, object?> source, object? target)
        {
            if (source != null && target != null)
            {
                Type targetType = target.GetType();

                Dictionary<string, PropertyInfo> tps = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty)
                    .ToDictionary(t => t.Name.ToLower(), t => t);

                foreach (var sKey in source.Keys)
                {
                    string key =sKey.ToLower();
                    if (tps.ContainsKey(key))
                    {
                        PropertyInfo tp = tps[key];
                        object? sourceValue = source[sKey];
                        MapValue(sourceValue, null, target, tp);
                    }
                }
            }
        }

        private static object Map(object source, Type sourceType, Type targetType)
        {
            if (IsValueType(sourceType))
            {
                if (IsValueType(targetType))
                {
                    return Convert.ChangeType(source, targetType);
                }
            }
            else if (source != null)
            {
                object target = Activator.CreateInstance(targetType);

                Map(source, target, sourceType, targetType);

                return target;
            }
            return null;
        }

        private static void Map(object source, object target, Type sourceType, Type targetType)
        {
            if (source != null && target != null)
            {
                PropertyInfo[] sps = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

                Dictionary<string, PropertyInfo> tps = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty)
                    .ToDictionary(t => t.Name.ToLower(), t => t);

                foreach (var sp in sps)
                {
                    string key = sp.Name.ToLower();
                    if (tps.ContainsKey(key))
                    {
                        PropertyInfo tp = tps[key];
                        object? sourcePropValue = sp.GetValue(source, null);

                        MapValue(sourcePropValue, sp.PropertyType, target, tp);
                    }
                }
            }
        }

        private static bool IsValueType(Type t)
        {
            return Reflector.IsValueType(t);
        }


        private static void MapValue(object? sourceValue, Type? sourceValueType, object target, PropertyInfo targetProperty)
        {
            if (sourceValue == null)
            {
                if (Reflector.IsNullableType(targetProperty.PropertyType))
                {
                    targetProperty.SetValue(target, null, null);
                }
            }
            else
            {
                if(sourceValueType == null)
                {
                    sourceValueType = sourceValue.GetType();
                }
                if (IsValueType(sourceValueType))
                {
                    MapSimpleValue(sourceValue, sourceValueType, target, targetProperty);
                }
                else if (sourceValueType.IsArray)
                {
                    Array arSourcePropValue = sourceValue as Array;
                    MapArray(arSourcePropValue, target, targetProperty);
                }
                else if (sourceValueType.GetInterface("IDictionary") != null)
                {
                    Type keyType = sourceValueType.GenericTypeArguments[0];
                    Type valueType = sourceValueType.GenericTypeArguments[1];
                    IDictionary dicSourcePropValue = sourceValue as IDictionary;
                    MapDictionary(dicSourcePropValue, keyType, valueType, target, targetProperty);
                }
                else if (sourceValueType.GetInterface("IEnumerable") != null)
                {
                    IEnumerable enSourcePropValue = sourceValue as IEnumerable;
                    MapEnumerable(enSourcePropValue, target, targetProperty);
                }
                else if (sourceValueType.IsClass && targetProperty.PropertyType.IsClass)
                {
                    object targetPropValue = Activator.CreateInstance(targetProperty.PropertyType);
                    Map(sourceValue, targetPropValue);
                    targetProperty.SetValue(target, targetPropValue, null);
                }
            }
        }


        private static void MapSimpleValue(object sourceValue, Type soureValueType, object targetObj, PropertyInfo targetPropInfo)
        {
            if (targetPropInfo.PropertyType.IsEnum)
            {
                object enmuValue = null;
                if (Enum.TryParse(targetPropInfo.PropertyType, sourceValue.ToString(), true, out enmuValue))
                {
                    targetPropInfo.SetValue(targetObj, enmuValue, null);
                }
            }
            else if (IsValueType(targetPropInfo.PropertyType))
            {
                if (CanSetValueDirectly(soureValueType, targetPropInfo.PropertyType))
                {
                    targetPropInfo.SetValue(targetObj, sourceValue);
                }
                else
                {
                    try
                    {
                        targetPropInfo.SetValue(targetObj, TConvert.ChangeType(sourceValue, targetPropInfo.PropertyType), null);
                    }
                    catch
                    {
                        //ignore convert error
                    }
                }
            }
        }

        private static bool CanSetValueDirectly(Type sourceType, Type targetType)
        {
            if (sourceType == targetType)
            {
                return true;
            }

            bool isTargetNullable = targetType.Name.StartsWith("Nullable`");
            if (isTargetNullable && targetType.GenericTypeArguments != null && targetType.GenericTypeArguments.Length > 0)
            {
                var targetValueType = targetType.GenericTypeArguments[0];
                if (sourceType == targetValueType)
                {
                    return true;
                }
            }

            return false;
        }

        private static void MapArray(Array sourceValue, object targetObj, PropertyInfo targetPropInfo)
        {
            Array arTargetPropValue = GetTargetArray(sourceValue, targetPropInfo);

            if (targetPropInfo.PropertyType.IsArray)
            {
                targetPropInfo.SetValue(targetObj, arTargetPropValue, null);
            }
            else if(targetPropInfo.PropertyType.GetInterface("IEnumerable") != null)
            {
               var newPropValue = Activator.CreateInstance(targetPropInfo.PropertyType, new object[] { arTargetPropValue });
                targetPropInfo.SetValue(targetObj, newPropValue, null);
            }
        }

        private static Array GetTargetArray(Array sourceValue, PropertyInfo targetPropInfo)
        {
            Type elementType = targetPropInfo.PropertyType.GetElementType();
            Array arTargetPropValue = Array.CreateInstance(elementType, sourceValue.Length);

            for (int i = 0; i < sourceValue.Length; i++)
            {
                if (IsValueType(elementType))
                {
                    arTargetPropValue.SetValue(Convert.ChangeType(sourceValue.GetValue(i), elementType), i);
                }
                else
                {
                    object newArValue = Activator.CreateInstance(elementType);
                    Map(sourceValue.GetValue(i), newArValue);
                    arTargetPropValue.SetValue(newArValue, i);
                }
            }
            return arTargetPropValue;
        }

        private static void MapEnumerable(IEnumerable sourceValue, object targetObj, PropertyInfo targetPropInfo)
        {
            List<object> targetList = new List<object>();
            Type targetElementType = GetElementType(targetPropInfo.PropertyType);
            if (targetElementType == null)
                return;

            foreach (var element in sourceValue)
            {
                Type elementType = element.GetType();
                if (IsValueType(elementType))
                {
                    targetList.Add(element);
                }
                else
                {
                    object targetElementValue = Activator.CreateInstance(targetElementType);
                    Map(element, targetElementValue);
                    targetList.Add(targetElementValue);
                }
            }

            Array arTargetPropValue = Array.CreateInstance(targetElementType, targetList.Count);
            for(int i=0;i<targetList.Count;i++)
            {
                arTargetPropValue.SetValue(targetList[i], i);
            }

            if (targetPropInfo.PropertyType.IsArray)
            {
                targetPropInfo.SetValue(targetObj, arTargetPropValue, null);
            }
            else if (targetPropInfo.PropertyType.GetInterface("IEnumerable") != null)
            {
                var newPropValue = Activator.CreateInstance(targetPropInfo.PropertyType, new object[] { arTargetPropValue });
                targetPropInfo.SetValue(targetObj, newPropValue, null);
            }
        }

        private static void MapDictionary(IDictionary sourceValue, Type sourceKeyType, Type sourceValueType, object targetObj, PropertyInfo targetPropInfo)
        {
            if (targetPropInfo.PropertyType.GetInterface("IDictionary") != null)
            {
                Type targetKeyType = targetPropInfo.PropertyType.GenericTypeArguments[0];
                Type targetValueType = targetPropInfo.PropertyType.GenericTypeArguments[1];

                IDictionary newPropValue = Activator.CreateInstance(targetPropInfo.PropertyType) as IDictionary;
                foreach (var key in sourceValue.Keys)
                {
                    object targetKeyValue = Map(key, sourceKeyType, targetKeyType);
                    if (targetKeyValue != null)
                    {
                        object targetValueValue = Map(sourceValue[key], sourceValueType, targetValueType);
                        newPropValue.Add(targetKeyValue, targetValueValue);
                    }
                }
                targetPropInfo.SetValue(targetObj, newPropValue, null);
            }
        }

        private static Type GetElementType(Type mainType)
        {
            if (mainType.IsArray)
            {
                return mainType.GetElementType();
            }
            Type[] types = mainType.GetGenericArguments();
            if (types.Length > 0)
            {
                return types[0];
            }

            return null;
        }
    }
}
