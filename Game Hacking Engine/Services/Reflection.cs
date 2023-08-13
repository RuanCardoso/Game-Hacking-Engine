using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Game_Hacking_Engine.Services
{
    internal static class Reflection
    {
        public static FieldInfo[] GetFields<T>()
        {
            var _t_ = typeof(T);
            return _t_.GetFields(BindingFlags.Public | BindingFlags.Instance);
        }

        public static string GetFieldValueToString(FieldInfo fieldInfo, object instance)
        {
            object? value = fieldInfo.GetValue(instance);
            if (value != null)
            {
                string typeName = value.GetType().Name;
                return Type.GetTypeCode(fieldInfo.FieldType) switch
                {
                    TypeCode.Int16 => ((short)value).ToString(),
                    TypeCode.Int32 => ((int)value).ToString(),
                    TypeCode.UInt16 => ((ushort)value).ToString(),
                    TypeCode.UInt32 => ((uint)value).ToString(),
                    TypeCode.Byte => ((byte)value).ToString(),
                    TypeCode.Object => typeName,
                    _ => typeName,
                }; ;
            }
            else
            {
                return "Unknown";
            }
        }

        public static string GetTypeCodeToString(FieldInfo info)
        {
            return info.FieldType.Name;
        }

        public static nint GetOffsetOfField<T>(string fieldName)
        {
            return Marshal.OffsetOf<T>(fieldName);
        }
    }
}