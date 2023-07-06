using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Ojisan : NetworkBehaviour
{
    public Color color;

    private float startTime;
    private float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = NetworkManager.Singleton.LocalTime.TimeAsFloat;

        // Instantiate the objects for each time value as children of this script's object
        //serverTimeObject = CreateMovingObject(Color.red);  // Red for server time

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        this.GetComponent<Renderer>().material.color = color;
    }
    void Update()
    {
        currentTime = startTime - NetworkManager.Singleton.LocalTime.TimeAsFloat;

        // Create noise-based movements within the screen bounds
        float serverTime = NetworkManager.Singleton.ServerTime.TimeAsFloat;
        float localTime = NetworkManager.Singleton.LocalTime.TimeAsFloat;

        this.transform.localPosition = TimeToPosition(serverTime);

        // Debug log currentTime, startTime, and server time
        Debug.LogFormat("Current Time: {0}, Start Time: {1}, Server Time: {2}, Local Time: {3}",
            currentTime,
            startTime,
            serverTime,
            localTime
        );
    }


    /*
    // Creates a colored cube
    private GameObject CreateMovingObject(Color color)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.AddComponent<NetworkObject>();
        obj.GetComponent<Renderer>().material.color = color;
        obj.transform.SetParent(transform, false);
        return obj;
    }
    */

    // Converts a time value to a position within the screen bounds
    private Vector3 TimeToPosition(float time)
    {
        return new Vector3(
            Mathf.Clamp(Mathf.PerlinNoise(time, 0) * 2 - 1, -Screen.width / 2, Screen.width / 2),
            Mathf.Clamp(Mathf.PerlinNoise(time, 0) * 2 - 1, -Screen.height / 2, Screen.height / 2),
            0
        );
    }

}
