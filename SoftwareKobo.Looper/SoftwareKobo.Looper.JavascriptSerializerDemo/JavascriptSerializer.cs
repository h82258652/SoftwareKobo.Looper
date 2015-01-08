using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareKobo.JavascriptSerializerDemo
{
    public class JavascriptSerializer
    {
        public JavascriptSerializer()
        {
        }

        public string Serialize(object obj)
        {
            if (IsStandardType(obj))// 基本类型不入递归
            {
                StringBuilder sb = new StringBuilder();
                SerializeObject(sb, obj);
                return sb.ToString();
            }

            Looper looper = new Looper(obj, GetChildren, HasChildren);
            StringBuilder buffer = new StringBuilder();
            buffer.Append("{");
            looper.Loop((manager, o) =>
            {
                SerializeObject(buffer, o);
            }, (manager, o) =>
            {
                SerializeObjectEnd(buffer, o);
            }, () =>
            {
                buffer.Append(",");
            });
            buffer.Append("}");
            return buffer.ToString();
        }

        private void SerializeObject(StringBuilder buffer, object obj)
        {
            if (obj == null)
            {
                buffer.Append("null");
            }
            else if (obj is PropertyNameAndValue)
            {
                var property = (PropertyNameAndValue)obj;
                SerializeString(buffer, property.PropertyName);
                buffer.Append(":");

                SerializeObject(buffer, property.PropertyValue);
            }
            else if (obj is bool)
            {
                // 布尔值需要优先于 IConvertible
                SerializeBoolean(buffer, (bool)obj);
            }
            else if (obj is string)
            {
                // 字符串需要优先于 IConvertible
                SerializeString(buffer, (string)obj);
            }
            else if (obj is IConvertible)
            {
                SerializeIConvertible(buffer, (IConvertible)obj);
            }
            else
            {
                buffer.Append("{");
            }
        }

        private void SerializeObjectEnd(StringBuilder buffer,object obj)
        {
            if (obj is PropertyNameAndValue)
            {
                obj = ((PropertyNameAndValue)obj).PropertyValue;
            }

            if (IsStandardType(obj))
            {
                return;
            }

            buffer.Append("}");
        }

        private void SerializeBoolean(StringBuilder buffer, bool b)
        {
            buffer.Append(b.ToString().ToLower());
        }

        private void SerializeIConvertible(StringBuilder buffer, IConvertible i)
        {
            buffer.Append(i);
        }

        private void SerializeString(StringBuilder buffer, string s)
        {
            // 仅仅只是一个 Demo，没有进行编码。
            buffer.AppendFormat("\"{0}\"", s);
        }

        private IEnumerable<object> GetChildren(object arg)
        {
            if (arg is PropertyNameAndValue)
            {
                arg = ((PropertyNameAndValue)arg).PropertyValue;
            }

            if (IsStandardType(arg))
            {
                return null;
            }

            var type = arg.GetType();
            var properties = type.GetProperties();

            return from property in properties
                   select new PropertyNameAndValue(property.Name, property.GetValue(arg));
        }

        private bool HasChildren(object arg)
        {
            if (arg is PropertyNameAndValue)
            {
                arg = ((PropertyNameAndValue)arg).PropertyValue;
            }

            if (IsStandardType(arg))
            {
                return false;
            }

            var type = arg.GetType();
            var properties = type.GetProperties();
            return properties.Any();
        }

        private bool IsStandardType(object obj)
        {
            if (obj == null)
            {
                return true;
            }
            if (obj is IConvertible)
            {
                // 数值类型
                return true;
            }
            if (obj is string)
            {
                return true;
            }
            return false;
        }

        private class PropertyNameAndValue
        {
            internal string PropertyName
            {
                get;
                private set;
            }

            internal object PropertyValue
            {
                get;
                private set;
            }

            public PropertyNameAndValue(string propertyName, object propertyValue)
            {
                PropertyName = propertyName;
                PropertyValue = propertyValue;
            }
        }
    }
}