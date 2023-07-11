using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityFeatureTest.Klak.Motion;
using UnityFeatureTest.Util;
using TMPro;

public class Character : NetworkBehaviour
{
    private float offset = 0;

    void Start()
    {
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        //offset = NetworkManager.Singleton.IsClient ? -5f : 5f;
        //debug log format offset
        //Debug.LogFormat("offset: {0}", offset);
    }
    void Update()
    {
        float x = 5 + (Mathf.PerlinNoise(Time.time, 0) - 0.5f) * 5f;
        this.transform.position = new Vector3(
            x,
            this.transform.position.y,
            this.transform.position.z
        );
    }
}
