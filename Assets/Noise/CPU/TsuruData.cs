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




    [Serializable]
    public class TsuruData
    {
        [LabelText("Landing Material")] public TsuruLandingMaterialData landingMaterial = new TsuruLandingMaterialData();
        [LabelText("Landing General")] public TsuruLandingData tsuruLandingData = new TsuruLandingData();
        [LabelText("Landing Map")] public TsuruLandingMapData tsuruLandingMapData = new TsuruLandingMapData();
        [LabelText("Landing Interaction")] public TsuruLandingInteractionData tsuruLandingInteractionData = new TsuruLandingInteractionData();

        [LabelText("Flying Material")] public TsuruFlyMaterialData flyMaterial;
        [LabelText("Flying General")]  public FlyTsuruData flyTsuruData;
        [LabelText("Flying StartFlying")]  public StartFlying startFlying;
        [LabelText("Flying Approaching")]  public Approaching approaching;
        [LabelText("Flying GrabbedAndFlyUp")]  public GrabbedAndFlyUp grabbedAndFlyUp;

        [LabelText("Waiting time in the sky (Kubomi's room) [seconds]")]
        public Vector2 waitingTimeInSky = new Vector2(300, 320f); //400f; // time in seconds before will automatically release if no message
        public Vector2 delayTimeForAllDropOff = new Vector2(2f, 4f);

        [LabelText("Flying DropOff")] public DropOff dropOff;
        [LabelText("Flying FlyUpAfterDropOff")] public FlyUpAfterDropOff flyUpAfterDropOff;
    }

    [System.Serializable]
    public class FlyTsuruData
    {
        public int tsuruFlyMaxNum = 30;
        [EnumType(typeof(TravelDirection))] public TravelDirection travelDirection = TravelDirection.LeftToRight;
        [Slider(0f, 1f)] public Vector2 flyTsuruScaleRange = new Vector2(0.7f, 0.7f);
    }




    [System.Serializable]
    public class TsuruLandingData
    {
        public int numOfTsurus = 300;

        public Vector2 scaleRange = new Vector2(0.72f, 0.88f);
        public float initRotationYAngle = 5f; // max deviation in degrees from X axis
        public Vector2 rotationYAngle = new Vector2(-10f, 10f);
        public Vector2 rotationYSinFrequency = new Vector2(0f, 0.1f);
        public float animationUsage = 0.5f;
        public Vector2 animationSpeed = new Vector2(0f, 0f);
        public Vector2 interactionDuration = new Vector2(8f, 10f);

        public int batchSize = 100;
        public Vector2 landingDelayRange = new Vector2(0.1f, 0.2f);
        public Vector2 takeOffDelayRange = new Vector2(0.1f, 0.2f);
        public float landingDurationTime = 200.0f;
        public float lastSpurtBalance = 1.0f;
        public float overlapDuringTime = 2.0f;

        public float colliderScreenSizeX = 0.9f;

        public float timeBetweenLandingsMin = 5f; // time between each "wave" of tsurus (between one wave finishing takeoff and the next wave starting landing)
        public float timeBetweenLandingsMax = 10f;

        public float interactionRate = 0.5f; // インタラクションで反応させる割合

        public float firstLandingDelay = 2.5f * 60f; //最初に鶴が着陸するときは2分30秒の遅延
    }


    [System.Serializable]
    public class TsuruLandingMapData
    {
        [LabelText("Map threshold")]
        public Vector4 noiseMapScaleAndOffset = new Vector4(0.4f, 0.7f, 0, 0);

        [LabelText("Noise cut threshold (v > 0.675 means more gathering, else means distributed. )")]
        public float noiseCutThreshold = 0.5f;

        public Vector4 dissolveNoiseMapScaleAndOffset = new Vector4(3f, 3f, 0, 0);
        public float dissolveAmount = 0.5f;

        public Vector2 area1EllipseCenter = new Vector2(0.25f, 0.5f);
        public Vector2 area1EllipseRadius =  new Vector2(0.15f, 0.2f);

        public Vector2 area2EllipseCenter = new Vector2(0.75f, 0.5f);
        public Vector2 area2EllipseRadius =  new Vector2(0.15f, 0.2f);


        [LabelText("Phase1 Tsuru count")]
        public int phase1TsuruCountThreshold = 10;

        [LabelText("Phase2 Tsuru count")]
        public int phase2TsuruCountThreshold = 200;

        public bool showMap = true;
        public float debugSphereSize = 0.1f;

    }


    [System.Serializable]
    public class TsuruLandingInteractionData
    {
        public Vector2 animationSpeed = new Vector2(0.9f, 1.1f);
    }

    [System.Serializable]
    public class StartFlying
    {
        [LabelText("Start Speed")]
        public float startForwardSpeed = 0.3f;
        [LabelText("End Speed")]
        public float endForwardSpeed = 0.5f;

        [LabelText("Start flying position X (random a ~ b) [0 - 1]")]
        public Vector2 startFlyingXRange = new Vector2(0.64f, 0.67f);
        //public Vector2 startFlyingXRange = new Vector2(8, 14); //TODO: あとでけす

        [LabelText("Fly down Duration [seconds]")]
        public float flyDownDuration = 15f;

        public float startPositionOffsetY = 7f;
        public float startPositionOffsetZ = 2f;

        [LabelText("Fly down curve power")]
        public float flyDownCurvePower = 1f;

    }

    [System.Serializable]
    public class Approaching
    {

        [LabelText("Speed (Approaching)")]
        public float forwardSpeedApproaching = 0.45f;

        [LabelText("Approaching Y (random a ~ b)")]
        public Vector2 approachingYRange = new Vector2(1.0f, 1.2f);

        [LabelText("Start hang delay time from approach [seconds]")]
        public float startHangDelayTimeFromApproach = 0f;

        [LabelText("Start hang tsuru anim speed [ratio]")]
        public float startHangTsuruAnimSpeed = 1.2f;

        [LabelText("Start hang tsuru anim transition fixed duration [seconds]")]
        [Slider(0, 1f)]
        public float startHangTsuruAnimTransitionFixedDuration = 0.8f;
        
        [LabelText("Distance X to start descent [meter]")]
        public float distanceXToBeginningOfArrangeHeight = 2f;

        [LabelText("Distance X to start reducing speed [meter]")]
        public float distanceXToBeginningOfReduceSpeed = 1.07f;
    }

    //つかまられ、上に飛ぶ
    [System.Serializable]
    public class GrabbedAndFlyUp
    {
        //[LabelText("Foward speed acceleration")]
        //public float forwardSpeedAcceleration = 0.002f; //TODO: Flyupした後のスピードでlerpするようにする
        public float forwardSpeedFlyUp = 1f;

        [LabelText("Fly up duration[seconds]")]
        public float flyUpDuration = 10f;

        [LabelText("Fly up position offset Y [meter]")]
        public float flyUpPositionOffsetY = 6.5f;

        //
        //ぶらさがり中の揺れの動き
        //
        [LabelText("X axis swaying angle")]
        public Vector2 xAxisSwayingAngle = new Vector2(22f, 27f);

        [LabelText("X axis swaying Normalized time smoothstep")]
        public Vector2 xAxisSwayingNormalTimeSmoothstep = new Vector2(1f, 0f);

        [LabelText("X axis swaying Sin() frequency")]
        public float xAxisSwayingSinFrequency = 10f;

        [LabelText("X axis swaying Sin() offset")]
        public float xAxisSwayingSinOffset =　Mathf.PI / 4; //少しSin寄りにオフセットをずらす

        [LabelText("Fly up curve power")]
        public float flyUpCurvePower = 4f;
    }

    //おじさんを落とす
    [System.Serializable]
    public class DropOff
    {
        [LabelText("Fly Down position x range (random a ~ b) [0 - 1]")]

        public Vector2 flyDownXRange = new Vector2(0.65f, 0.68f);

        //
        // Fly down, Release hands, Fly up tsuru only
        //
        [LabelText("Speed (Flying Down Start)")]
        public float forwardSpeedFlyingDownStart = 0.45f;

        [LabelText("Speed (Flying Down End)")]
        public float forwardSpeedFlyingDownEnd = 0.5f;

        [LabelText("Speed (Hands Off)")]
        public float forwardSpeedHandsOff = 0.25f;

        [LabelText("end hang tsuru anim speed [ratio]")]
        public float endHangTsuruAnimSpeed = 1f;

        [LabelText("Fly down Duration [seconds]")]
        public float flyDownDuration = 12f;

        [LabelText("Start Walk To Hands Off Duration [seconds]")]
        public float startWalkToHandsOffDuration = 0.6f;

        [LabelText("Fly down reduce speed duration [seconds]")]
        public float flyDownReduceSpeedDuration = 1f;

        [LabelText("Fly down curve power")]
        public float flyDownCurvePower = 4f;

        [LabelText("Release hands delay time [seconds]")]
        public float releaseHandsDelayTime = 1.2f;
        
        [LabelText("Anim Transition Duration Between Loop And Walk[seconds]")]
        public float animTransitionDurationLoopAndWalk  = 0.5f;
    }

    //おじさんをおろした後の上昇して消える
    [System.Serializable]
    public class FlyUpAfterDropOff
    {
        //[LabelText("Forward speed acceleration")]
        //public float forwardSpeedAcceleration = 0.006f;
        public float forwardSpeedFlyUp = 1.2f;

        [LabelText("Fly up duration")]
        public float flyUpDuration = 10f;

        [LabelText("Fly up curve power")]
        public float flyUpCurvePower = 4f;
    }

    [System.Serializable]
    public class TsuruFlyMaterialData : TsuruCommonMaterialData { }

    [System.Serializable]
    public class TsuruLandingMaterialData : TsuruCommonMaterialData { }

    [System.Serializable]
    public class TsuruCommonMaterialData
    {
        public ThemeColor tsuruTexColor = ThemeColor.Dark;

        public float colorFractalIntencity = 0;
        public float colorFractalSaturation = 0;

        [Slider(0, 1f)] public float transparency = 1f;
        [Slider(0, 1f)] public float contrast = 1f;
        [Slider(0, 1f)] public float saturation = 1f;
        public Vector2 brightnessRange = new Vector2(0.3f, 1f);
        [Slider(1, 5f)] public float gamma = 1f;
        [Slider(0, 1f)] public float blackColorLevelContrast = 0.0f;
    }

}