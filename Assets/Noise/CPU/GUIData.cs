using System;
using UnityEngine;

//
// Order: Start Flying, Approaching, Grabbed and Fly Up, Drop Off
//
namespace CPUNoise.Data
{

    [AttributeUsage(AttributeTargets.Field)]
    class LabelText : Attribute
    {
        internal readonly string text;
        internal LabelText(string text)
        {
            this.text = text;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    class EnumType : Attribute
    {
        internal readonly Type enumType;
        internal EnumType(Type enumType)
        {
            this.enumType = enumType;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    class Slider : Attribute
    {
        internal readonly float range_from;
        internal readonly float range_to;
        internal Slider(float from, float to)
        {
            this.range_from = from;
            this.range_to = to;
        }
    }

    public enum TravelDirection
    {
        RightToLeft = 0,
        LeftToRight,
        Either
    }

    public enum ThemeColor
    {
        Light,
        Dark,
    }

    public enum StageKind
    {
        Stage01 = 1,
        Stage02 = 2,
        Stage03 = 3,
        Stage04 = 4,
        Stage05 = 5,
        Stage06 = 6,
        Stage07 = 7,
        Stage08 = 8,
        Stage09 = 9,
        Stage10 = 10
    }


}