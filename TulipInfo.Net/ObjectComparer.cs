using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections.Concurrent;
using System.Collections;

namespace TulipInfo.Net
{
    public class ObjectComparer<ObjectType>
    {

        /// <summary>
        /// key: type full name
        /// value: all public get properties
        /// </summary>
        static ConcurrentDictionary<string, PropertyInfo[]> ObjectProperties = new ConcurrentDictionary<string, PropertyInfo[]>();
        static void SetObjectProperties(Type type)
        {
            if (type == null)
                return;

            string objTypeFullName = type.FullName;
            if (ObjectProperties.ContainsKey(objTypeFullName) == false)
            {
                PropertyInfo[] objProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
                ObjectProperties.TryAdd(objTypeFullName, objProperties);
                foreach (var p in objProperties)
                {
                    if (p.PropertyType.IsArray)
                    {
                        Type elementType = p.PropertyType.GetElementType();
                        SetObjectProperties(elementType);
                    }
                    else if (p.PropertyType.GetInterface("IDictionary") != null)
                    {
                        Type keyType = p.PropertyType.GenericTypeArguments[0];
                        SetObjectProperties(keyType);
                        Type valueType = p.PropertyType.GenericTypeArguments[1];
                        SetObjectProperties(valueType);
                    }
                    else if (p.PropertyType.GetInterface("IEnumerable") != null)
                    {
                        Type[] types = p.PropertyType.GetGenericArguments();
                        foreach(var t in types)
                        {
                            SetObjectProperties(t);
                        }
                    }
                    else if (p.PropertyType.IsClass)
                    {
                        SetObjectProperties(p.PropertyType);
                    }
                }
            }
        }
        static PropertyInfo[] GetObjectProperties(Type type)
        {
            string objTypeFullName = type.FullName;
            if (ObjectProperties.ContainsKey(objTypeFullName))
            {
                return ObjectProperties[objTypeFullName];
            }
            return new PropertyInfo[0];
        }

        public bool IsChanged { get; private set; } = false;

        string _previousHashValue = string.Empty;
        string _currentHashValue = string.Empty;
        Type _objType = null;

        public ObjectComparer()
        {
            _objType = typeof(ObjectType);
            SetObjectProperties(_objType);
        }

        public ObjectComparer(ObjectType initObj) : this()
        {
            if (initObj != null)
            {
                Add(initObj);
            }
        }

        public void Add(ObjectType obj)
        {
            _previousHashValue = _currentHashValue;
            _currentHashValue = GetHashValue(obj);
            IsChanged = _previousHashValue != string.Empty && _previousHashValue != _currentHashValue;
        }

        public void Reset()
        {
            _previousHashValue = string.Empty;
            _currentHashValue = string.Empty;
            IsChanged = false;
        }

        private string GetHashValue(ObjectType obj)
        {
            if (obj == null)
            {
                return "NULL";
            }

            using(MemoryStream ms = new MemoryStream())
            {
                using(StreamWriter sw = new StreamWriter(ms))
                {
                    var allProperties = GetObjectProperties(_objType);
                    foreach(var p in allProperties)
                    {
                        WritePropertyValue(sw, p, obj);
                    }
                }

               return Hash.MD5(ms.ToArray());
            }
        }

        private void WritePropertyValue(StreamWriter sw, PropertyInfo p, object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                sw.WriteLine("NULL");
                return;
            }

            var value = p.GetValue(obj, null);
            if (value == null || obj == DBNull.Value)
            {
                sw.WriteLine("NULL");
                return;
            }

            if (Reflector.IsValueType(p.PropertyType))
            {
                WriteValue(sw, value);
            }
            else if (p.PropertyType.IsArray)
            {
                Array arValues = value as Array;
                Type elementType = p.PropertyType.GetElementType();
                WriteArray(sw, elementType, arValues);
            }
            else if (p.PropertyType.GetInterface("IDictionary") != null)
            {
                IDictionary dicValues = value as IDictionary;
                Type keyType = p.PropertyType.GenericTypeArguments[0];
                Type valueType = p.PropertyType.GenericTypeArguments[1];
                WriteDic(sw, keyType, valueType, dicValues);
            }
            else if (p.PropertyType.GetInterface("IEnumerable") != null)
            {
                IEnumerable enValues = value as IEnumerable;
                Type elementType = p.PropertyType.GetGenericArguments()[0];
                foreach (var v in enValues)
                {
                    foreach (var ep in GetObjectProperties(elementType))
                    {
                        WritePropertyValue(sw, ep, v);
                    }
                }
            }
            else if (p.PropertyType.IsClass)
            {
                foreach (var cp in GetObjectProperties(p.PropertyType))
                {
                    WritePropertyValue(sw, cp, value);
                }
            }
        }

        private void WriteArray(StreamWriter sw, Type elementType,Array arValues)
        {
            if (Reflector.IsValueType(elementType))
            {
                for (int i = 0; i < arValues.Length; i++)
                {
                    WriteValue(sw, arValues.GetValue(i));
                }
            }
            else
            {
                var arrayProperties = GetObjectProperties(elementType);
                for (int i = 0; i < arValues.Length; i++)
                {
                    foreach (var ap in arrayProperties)
                    {
                        WritePropertyValue(sw, ap, arValues.GetValue(i));
                    }
                }
            }
        }

        private void WriteDic(StreamWriter sw,Type keyType,Type valueType,IDictionary dicValues)
        {
            if (Reflector.IsValueType(keyType))
            {
                foreach (var key in dicValues.Keys)
                {
                    WriteValue(sw, key);
                }
            }
            else
            {
                var dicProps = GetObjectProperties(keyType);
                foreach (var key in dicValues.Keys)
                {
                    foreach (var dicp in dicProps)
                    {
                        WritePropertyValue(sw, dicp, key);
                    }
                }
            }

            if (Reflector.IsValueType(valueType))
            {
                foreach (var v in dicValues.Values)
                {
                    WriteValue(sw, v);
                }
            }
            else
            {
                var dicProps = GetObjectProperties(valueType);
                foreach (var value in dicValues.Values)
                {
                    foreach (var dicp in dicProps)
                    {
                        WritePropertyValue(sw, dicp, value);
                    }
                }
            }
        }

        private void WriteValue(StreamWriter sw, object value)
        {
            if (value == null || value == DBNull.Value)
            {
                sw.WriteLine("NULL");
            }
            Type valueType = value.GetType();
            if (valueType == typeof(DateTime) || valueType == typeof(DateTime?))
            {
                sw.WriteLine(((DateTime)value).Ticks);
            }
            else
            {
                sw.WriteLine(value);
            }
        }
    }
}
