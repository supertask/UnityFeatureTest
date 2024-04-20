using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFeatureTest.Util
{
    public class Util
    {
        public static float Remap(float v, float minOld, float maxOld, float minNew, float maxNew)
        {
            return minNew + (v-minOld) * (maxNew - minNew) / (maxOld-minOld);
        }
    }
}

