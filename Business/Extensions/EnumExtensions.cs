using System;
using MinesweeperML.Business.Attributes;

namespace MinesweeperML.Business.Extensions
{
    /// <summary>
    /// Extensions for enumerations.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Description or null.</returns>
        public static string GetDescription(this Enum value)
        {
            // Return the enum description.
            if (value != null)
            {
                var type = value.GetType();
                var name = Enum.GetName(type, value);
                if (name != null)
                {
                    var field = type.GetField(name);
                    if (field != null)
                    {
                        if (Attribute.GetCustomAttribute(field, typeof(LocalizedDescriptionAttribute)) is LocalizedDescriptionAttribute attr)
                        {
                            return attr.Description;
                        }
                        throw new NotImplementedException($"The {nameof(LocalizedDescriptionAttribute)} of {field} could not be found.");
                    }
                }
            }
            return string.Empty;
        }
    }
}