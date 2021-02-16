// Created at   : 30.06.2017
// Signature    : -m _UID
// Note: It requires copyright

// it can be base of ROM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Akgul_Yemek.Models;

namespace Akgul_Yemek
{
    public class MTranslation
    {
        static Type[] types = Assembly.GetExecutingAssembly().GetTypes();

        public static object BuildObject(FormCollection formCollection, string class_name)
        {
            Type tip = types.Where(w => w.Name == class_name).Single();

            object _return = Activator.CreateInstance(tip);

            foreach (string key in formCollection.AllKeys)
            {
                PropertyInfo property = tip.GetProperty(key);
                if (property == null) // if there is not a property in class skip this key
                    continue;

                object value = null;

                if (property.PropertyType == typeof(string))
                    value = formCollection[key].ToString();
                else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(Nullable<int>))
                    value = Convert.ToInt32(formCollection[key].ToString());
                else if (property.PropertyType == typeof(Single))
                    value = Convert.ToSingle(formCollection[key].ToString().Replace(".", ",")); // 1.5 is invalid, 1,5 is valid
                else if (property.PropertyType == typeof(float) || property.PropertyType == typeof(double))
                    value = Convert.ToDouble(formCollection[key].ToString().Replace(".", ",")); // 1.5 is invalid, 1,5 is valid
                else if (property.PropertyType == typeof(DateTime))
                    value = formCollection[key].ToString().ToDateTime();

                property.SetValue(_return, value, null);
            }
            return _return;
        }

    }
}



/*
 * 
 * 
 * 
V1
 * 
        public enum ObjectTypes
        {
            user,
            content,
        }

        static void Assign(Type t, object o, string propname, object value)
        {
            PropertyInfo myPropInfo = t.GetProperty(propname);
            myPropInfo.SetValue(o, value, null);
        }


        public static object ToObject(FormCollection formCollection, ObjectTypes o_type)
        {
            object _return=null;
            Type type=null;

            switch(o_type)
            {
                case ObjectTypes.user:
                    _return = new user();
                    type = typeof(user);
                    break;

                case ObjectTypes.content:
                    _return = new content();
                    type = typeof(content);
                    break;
            }

            foreach (string key in formCollection.AllKeys)
            {
                Assign(type, _return, key, formCollection[key]);
            }

            return _return;
        }

 * 
 * 
 * 
 * 
V2:
 * 
        static Type[] ts = {
            // DB entities:
            typeof(content),
            typeof(cours),
            typeof(detail),
            typeof(faq),
            typeof(forum_categories),
            typeof(forum_comments),
            typeof(forum_topics),
            typeof(gallery_images),
            typeof(gallery_titles),
            typeof(helpfull_urls),
            typeof(user),

            // Special Entities
            typeof(IletisimModel)
        };

        public static object ToObject2(FormCollection formCollection, string class_name)
        {
            object _return = null;
            Type _type = null;

            foreach (Type tip in ts)
            {
                if (tip.Name == class_name)
                {
                    _return = Activator.CreateInstance(tip);
                    _type = tip;
                }
            }

            foreach (string key in formCollection.AllKeys)
            {
                Assign(_type, _return, key, formCollection[key]);
            }

            return _return;
        }
  

 * 
         
 public static bool IsNumericType(this object o)
{   
switch (Type.GetTypeCode(o.GetType()))
{
case TypeCode.Byte:
case TypeCode.SByte:
case TypeCode.UInt16:
case TypeCode.UInt32:
case TypeCode.UInt64:
case TypeCode.Int16:
case TypeCode.Int32:
case TypeCode.Int64:
case TypeCode.Decimal:
case TypeCode.Double:
case TypeCode.Single:
return true;
default:
return false;
}
}
         
         
         
 */
