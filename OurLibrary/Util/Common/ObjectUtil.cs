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
        public static string CreateInsertQuery(string Name, string[] Params, object Object)
        {
            string Q = "INSERT INTO   " + Name + "  (";
            string Val = "";
            for (int i = 0; i < Params.Length; i++)
            {
                string PropertyName = Params[i];
                if (!HasProperty(PropertyName, Object))
                    continue;
                Q += PropertyName;
                object Value = GetValueFromProp(PropertyName, Object);
                if (Value.GetType().Equals(typeof(DateTime)))
                {
                    Value = StringUtil.DateTimeToString((DateTime)Value);
                }

                Val += "'" + Value + "'";

                if (i < Params.Length - 1)
                {
                    Q += ",";
                    Val += ",";
                }


            }
            Q = Q + ")VALUES(" + Val + ")";

            return Q;

        }

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
                    if (Attribute.FieldType != null && Attribute.FieldType.Contains("id_"))
                    {
                        return PropsInfo.Name;
                    }
                }
            }
            return null;
        }

        public static object GetObjectValues(string[] Props, object OriginalObj)
        {
            object NewObject = Activator.CreateInstance(OriginalObj.GetType());
            for (int i = 0; i < Props.Length; i++)
            {
                string PropName = Props[i];
                if (HasProperty(PropName, OriginalObj))
                {
                    object val = OriginalObj.GetType().GetProperty(PropName).GetValue(OriginalObj);
                    NewObject.GetType().GetProperty(PropName).SetValue(NewObject, val);
                }
            }
            return NewObject;
        }

        public static bool HasProperty(string PropName, object O)
        {
            foreach (PropertyInfo Prop in O.GetType().GetProperties())
            {
                if (Prop.Name.Equals(PropName))
                {
                    return true;
                }
            }
            return false;
        }
    }


}