using System;
using System.ComponentModel;
using System.Globalization;

namespace LoremIpsum
{
    public static class Utility
    {
        /// <summary>
        /// Retrieve the description value from this enum.
        /// </summary>
        /// <typeparam name="T">Automatically provided when calling as an extension.</typeparam>
        /// <param name="e">Represents the current instance of an enum that this method is being called from.</param>
        /// <returns></returns>
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            string description = null;

            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (attributes.Length > 0)
                        {
                            description = ((DescriptionAttribute)attributes[0]).Description;
                        }

                        break;
                    }
                }
            }

            return description;
        }
    }
}
