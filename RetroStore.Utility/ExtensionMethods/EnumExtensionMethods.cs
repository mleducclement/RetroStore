using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RetroStore.Utility.ExtensionMethods {
    public static class EnumExtensionMethods {

        // Returns the name in the [Description]
        public static string GetDescription(this Enum value) {
            Type type = value.GetType();
            string? name = Enum.GetName(type, value);
            if (name == null) {
                return "No Name";
            }
            FieldInfo? field = type.GetField(name);
            if (field == null) {
                return "No Field";
            }
            return Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr ? attr.Description : name;
        }
    }
}
