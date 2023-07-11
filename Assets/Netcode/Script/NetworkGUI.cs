using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkGUI : MonoBehaviour
{
    string serverAddress = "";

    private void OnGUI()
    {
        GUILayout.BeginVertical("Box");
        if (NetworkManager.Singleton.IsListening == false )
        {
            if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Client"))
            {
                var unityTransport = GetComponent<UnityTransport>();
                unityTransport.ConnectionData.Address = serverAddress;
                unityTransport.ConnectionData.Port = 7777;
               var result =  NetworkManager.Singleton.StartClient();
                Debug.Log("result : " + result);
            }
            serverAddress = GUILayout.TextField(serverAddress, GUILayout.Width(100));
            GUILayout.EndHorizontal();

        }
        else
        {
            if (NetworkManager.Singleton.IsServer)
                GUILayout.Label("server");

            if (NetworkManager.Singleton.IsClient)
                GUILayout.Label("client : connected to " + serverAddress);
        }

        GUILayout.EndVertical();
    }
}
