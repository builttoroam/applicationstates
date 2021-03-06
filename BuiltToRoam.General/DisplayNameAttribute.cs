﻿using System;

namespace BuiltToRoam
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class DisplayNameAttribute : Attribute
    {
        public string DisplayName { get; set; }

        public DisplayNameAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
