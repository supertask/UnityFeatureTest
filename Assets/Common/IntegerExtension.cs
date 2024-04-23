using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public static class IntegerExtension
    {
        public static float RandomValue(this int seed)
        {
            Random.State oldState = Random.state;

            Random.InitState(seed);

            var value = Random.value;

            Random.state = oldState;

            return value;
        }

        public static float RandomValueRange(this int seed, float min, float max, bool debug = false)
        {
            var t = seed.RandomValue();
            var rand = Mathf.Lerp(min, max, t);
            return rand;
        }
        public static float RandomValueRange(this int seed, float min, float max, AnimationCurve curve, bool debug = false)
        {
            var t = seed.RandomValue();
            t = curve.Evaluate(t);
            var rand = Mathf.Lerp(min, max, t);
            return rand;
        }

        public static Vector3 RandomUnitSphere(this int seed)
        {
            Random.State oldState = Random.state;

            Random.InitState(seed);

            var value = Random.onUnitSphere;

            Random.state = oldState;

            return value;
        }
    }
}