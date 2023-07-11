using System.Numerics;
using Unity.Mathematics;

//
// Extension from Keijiro's Procedural Motion package
// https://github.com/keijiro/ProceduralMotion
//
namespace UnityFeatureTest.Klak.Motion
{
    public struct MathTransform
    {
        public float3 localPosition;
        public quaternion localRotation;
    }

    //[UnityEngine.AddComponentMenu("Klak/Procedural Motion/Brownian Motion")]
    public class BrownianMotionExtra
    {
        #region Editable attributes

        public float3 positionAmount; // = new float3(0.02f, 0.04f, 0.02f);
        public float3 rotationAmount; // = new float3(1,0,1);
        public float frequency; // = 0.2f;
        public int octaves = 2;
        public uint seed = 0;
        public MathTransform mathTransform;
        public float positionAmountMultiplier;
        public float rotationAmountMultiplier;

        #endregion

        #region Public method

        public BrownianMotionExtra()
        {
            this.mathTransform.localPosition = float3.zero;
            this.mathTransform.localRotation = quaternion.identity;
            Enable();
            Start();
        }


        public void Rehash()
        {
            var rand = Utilities.Random(seed);

            _positionOffset = rand.NextFloat3(-1e3f, 1e3f);
            _rotationOffset = rand.NextFloat3(-1e3f, 1e3f);

            ApplyMotion();
        }

        #endregion

        #region Private members

        float3 _positionOffset;
        float3 _rotationOffset;
        float _time;

        float3 _initialPosition;
        quaternion _initialRotation;

        public static float Fbm(float x, float y, int octave)
        {
            var p = math.float2(x, y);
            var f = 0.0f;
            var w = 0.5f;
            for (var i = 0; i < octave; i++)
            {
                f += w * noise.snoise(p);
                p *= 2.0f;
                w *= 0.5f;
            }
            return f;
        }

        public void ApplyMotion()
        {
            var np = math.float3(
                Fbm(_positionOffset.x, _time, octaves),
                Fbm(_positionOffset.y, _time, octaves),
                Fbm(_positionOffset.z, _time, octaves)
            );

            var nr = math.float3(
                Fbm(_rotationOffset.x, _time, octaves),
                Fbm(_rotationOffset.y, _time, octaves),
                Fbm(_rotationOffset.z, _time, octaves)
            );

            np = np * positionAmount / 0.75f;
            nr = nr * rotationAmount / 0.75f;

            var nrq = quaternion.EulerZXY(math.radians(nr));

            mathTransform.localPosition = _initialPosition + np;
            mathTransform.localRotation = math.mul(nrq, _initialRotation);
        }

        #endregion

        #region MonoBehaviour implementation

        public void Start()
        {
            Rehash();
        }

        public void Enable()
        {
            _initialPosition = mathTransform.localPosition;
            _initialRotation = mathTransform.localRotation;
        }

        public void Disable()
        {
            mathTransform.localPosition = _initialPosition;
            mathTransform.localRotation = _initialRotation;
        }

        public void Update()
        {
            //positionAmount = positionAmountMultiplier * _setting.brownianPositionAmount;
            //rotationAmount = rotationAmountMultiplier * _setting.brownianRotationAmount;
            //frequency = _setting.brownianFrequency;

            _time += UnityEngine.Time.deltaTime * frequency;
            ApplyMotion();
        }

        #endregion
    }
}
