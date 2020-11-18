using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace SimpleHelper.Core
{
    public static class EnumHelper
    {
        /// <summary>
        /// Will return the description of a Enum.
        /// </summary>
        /// <param name="myEnum">Enumerator</param>
        /// <returns>The description of a Enum.</returns>
        public static string GetDescription(this Enum myEnum)
        {
            if (myEnum == null)
                return null;

            Type enumType = myEnum.GetType();
            FieldInfo field = enumType.GetField(myEnum.ToString());

            var type = myEnum.GetType();
            var typeInfo = type.GetTypeInfo();
            var memberInfo = typeInfo.GetMember(myEnum.ToString());

            if (memberInfo.Count() == 0)
            {
                return null;
            }

            var attributes = memberInfo[0].GetCustomAttributes<DescriptionAttribute>();
            var attribute = attributes.FirstOrDefault();

            if (attribute == null)
                return myEnum.ToString();

            return attribute.Description;
        }
    }
}
