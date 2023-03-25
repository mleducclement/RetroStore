﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RetroStore.Utility.ExtensionMethods {
    public static class EnumExtensionMethods {
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
            DescriptionAttribute? attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attr != null ? attr.Description : name;
        }
    }
}