using OurLibrary.Annotation;
using OurLibrary.Parameter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace OurLibrary.Util.Common
{
    public class ObjectUtil
    {

        public static object ConvertList(List<object> value, Type type)
        {
            IList list = (IList)Activator.CreateInstance(type);
            foreach (var item in value)
            {
                list.Add(item);
            }
            return list;
        }

        public static object GetValueFromProp(string propname, object Object)
        {
            return Object.GetType().GetProperty(propname).GetValue(Object);
        }

        public static string GetIDProps(string ObjectPath)
        {
           Type t = Type.GetType(ObjectPath);
            PropertyInfo[] Props = t.GetProperties();
            for (int i = 0; i < Props.Length; i++)
            {
                PropertyInfo PropsInfo = Props[i];
                object[] Attributes = PropsInfo.GetCustomAttributes(typeof(FieldAttribute), true);
                if (Attributes.Length > 0)
                {
                    FieldAttribute Attribute = (FieldAttribute)Attributes[0];
                    if (Attribute.FieldType != null && Attribute.FieldType.Equals(AttributeConstant.TYPE_ID))
                    {
                        return PropsInfo.Name;
                    }
                }
            }
            return null;
        }
    }
}