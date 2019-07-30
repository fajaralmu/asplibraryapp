using OurLibrary.Annotation;
using OurLibrary.Parameter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using OurLibrary.Models;

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

        public static string[] ValidateProps(string[] Props, object Obj)
        {
            List<string> ValidProp = new List<string>();
            for (int j = 0; j < Props.Length; j++)
            {
                string Prop = Props[j];
                if (HasProperty(Prop, Obj))
                {
                    ValidProp.Add(Prop);
                }
            }
            return ValidProp.ToArray<string>();
        }

        public static List<object> ICollectionToListObj(ICollection Collection)
        {   List<object> List = new List<object>();
            foreach(var item in Collection)
            {
                if (item == null)
                    continue;
                List.Add(item);
            }
            return List;
        }

        public static string ListToDelimitedString(object ListOfObject, string Delimiter,string ValDelimiter, params string[] Props)
        {
            String ListString = "";
            if (ListOfObject == null )
                return "";
            List<object> List = ICollectionToListObj((ICollection)ListOfObject);
            if(List == null || List.Count == 0)
            {
                return "";
            }
            string[] ValidProps = ValidateProps(Props, List.ElementAt(0));
           
            for (int i = 0; i < List.Count; i++)

            {
                object Obj = List.ElementAt(i);
                string Value = "";
                for (int j = 0; j < ValidProps.Length; j++)
                {
                    string Prop = ValidProps[j];
                    Value += GetValueFromProp(Prop, Obj).ToString();
                    if (j < ValidProps.Length - 1)
                    {
                        Value += ValDelimiter;
                    }
                }
                ListString += Value;
                if (i < List.Count - 1)
                {
                    ListString += Delimiter;
                }

            }
            return ListString;
        }
    }


}