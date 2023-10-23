using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityFeatureTest.Klak.Motion;
using UnityFeatureTest.Util;
using TMPro;

public class LineOfTime : NetworkBehaviour
{
    public Color color;
    public TimeType timeType;

    private float fbmFrequency = 0.2f;
    private double startTime;
    private double currentTime;
    private Vector3 _movingHalfSize = new Vector3(10, 0, 0);

    private double flowerTime;
    private double previousLocalTime;
    public Transform cameraTransform;

    private NetworkVariable<Vector3> _fbmOffset = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private TMP_Text _textMeshPro;


    // Start is called before the first frame update
    void Start()
    {
        startTime = NetworkManager.Singleton.LocalTime.Time;

        // Instantiate the objects for each time value as children of this script's object
        //serverTimeObject = CreateMovingObject(Color.red);  // Red for server time

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        //timeType = TimeType.Server;
        //timeType = TimeType.Local;
        //timeType = TimeType.Flower;
        timeType = TimeType.Custom;

        _fbmOffset.Value = new Vector3(
            0, UnityEngine.Random.Range(- _movingHalfSize.y, _movingHalfSize.y), 0
            //UnityEngine.Random.Range(- _movingHalfSize.x, _movingHalfSize.x),
            //UnityEngine.Random.Range(- _movingHalfSize.y, _movingHalfSize.y),
            //UnityEngine.Random.Range(- _movingHalfSize.z, _movingHalfSize.z)
        );
        foreach (Transform child in this.transform)
        {
            if (child.GetComponent<MeshRenderer>() != null && child.name == "Cube")
            {
                child.GetComponent<MeshRenderer>().material.color = color;
            }
            else if (child.GetComponent<TMP_Text>() != null)
            {
                _textMeshPro = child.GetComponent<TMP_Text>();

            }
        }

        if (NetworkManager.Singleton.IsClient == false)
        {
            Application.targetFrameRate = 30;
        }
    }
    void Update()
    {

        //Spike
        if (NetworkManager.Singleton.IsClient)
        {
            float randomScale = UnityEngine.Random.Range(0.3f, 1.8f);
            Time.timeScale = randomScale;
        }


        //currentTime = startTime - NetworkManager.Singleton.LocalTime.TimeAsFloat;

        // Create noise-based movements within the screen bounds
        double serverTime = NetworkManager.Singleton.ServerTime.Time;
        double localTime = NetworkManager.Singleton.LocalTime.Time;

        double deltaTime = localTime - previousLocalTime;

        flowerTime += deltaTime;

        double time = 0;
        if (timeType == TimeType.Server)
        {
            time = serverTime * fbmFrequency;
        }
        else if (timeType == TimeType.Local)
        {
            time = localTime * fbmFrequency;
        }
        else if (timeType == TimeType.Custom)
        {
            time = (localTime - serverTime) * fbmFrequency;
        }
        else if (timeType == TimeType.Flower)
        {
            time = flowerTime * fbmFrequency;
        }


        Vector3 offset = Vector3.zero;
        if (NetworkManager.Singleton.IsClient)
        {
            offset = new Vector3(10, 0, 0);
        }


        this.transform.position = offset + new Vector3(
            0,
            Util.Remap(BrownianMotionExtra.Fbm(_fbmOffset.Value.x, (float)time, 1), -1, 1, -_movingHalfSize.x, _movingHalfSize.x),
            0
        //Util.Remap( BrownianMotionExtra.Fbm(_fbmOffset.y, time, 1), -1, 1, - _movingHalfSize.y, _movingHalfSize.y),
        //Util.Remap( BrownianMotionExtra.Fbm(_fbmOffset.z, time, 1), -1, 1, - _movingHalfSize.z, _movingHalfSize.z)
        );

        if (_textMeshPro != null)
        {
            string debugLine = string.Format("Line time: {0:F4}, Server Time: {1:F4}, \nLocal Time: {2:F4}, Flower time: {3:F4}",
                time, serverTime, localTime, flowerTime);
            _textMeshPro.SetText(debugLine);
        }


        previousLocalTime = NetworkManager.Singleton.LocalTime.Time;
    }
}
