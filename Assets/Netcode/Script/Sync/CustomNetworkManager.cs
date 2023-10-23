using UnityEngine;
using System.Collections;
using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.Events;

namespace Sample
{
    public class CustomNetworkManager : MonoBehaviour
    {
#if UNITY_EDITOR
        public NetworkMode networkModeInEditorOnly;
#endif

        public UnityEvent OnStartedServer;
        public UnityEvent OnStartedClient;

        private NetworkMode networkMode;
        private string networkAddress;
        private ushort networkPort;

        private void Start()
        {
            StartCoroutine(StartNetwork());
        }

        IEnumerator StartNetwork()
        {
            yield return new WaitForEndOfFrame();

            LoadCommandLine();
        }

        float t;
        private void Update()
        {
            Shader.SetGlobalFloat("_SyncTime", NetworkManager.Singleton.LocalTime.TimeAsFloat);

            t += Time.deltaTime;

            if(networkMode == NetworkMode.Client)
            {
                if(NetworkManager.Singleton.IsListening == false && t>=3.0f)
                {
                    StartCoroutine(TryToConnect());
                }
                else
                {
                    if (retryCount > 0)
                        retryCount = 0;
                }
            }
        }

        private void LoadCommandLine()
        {
            string[] args = System.Environment.GetCommandLineArgs();

#if UNITY_EDITOR
            if (networkModeInEditorOnly == NetworkMode.Server)
            {
                args = new string[] { "-ServerAddress", "127.0.0.1:7777", "-Server" };
            }
            else
            {
                args = new string[] { "-ServerAddress", "127.0.0.1:7777", "-Client" };
            }
#endif
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                if (arg.Equals("-ServerAddress"))
                {
                    var address = args[i + 1].Split(':');
                    var ip = address[0];
                    ushort port = 0;
                    ushort.TryParse(address[1], out port);

                    networkAddress = ip;
                    networkPort = port;
                }
                if (arg.Equals("-Client"))
                {
                    Begin(NetworkMode.Client);
                }

                if (arg.Equals("-Server"))
                {
                    Begin(NetworkMode.Server);
                }
            }
        }

        int retryCount;

        IEnumerator TryToConnect(float waitTime = 3f, int limitRetryCount = 1200)
        {
            yield return new WaitForSeconds(waitTime);

            retryCount++;

            if (retryCount <= limitRetryCount)
                BeginClient();

            t = 0;
        }

        public void Begin(NetworkMode mode)
        {
            networkMode = mode;

            switch (mode)
            {
                case NetworkMode.Client:

                    BeginClient();

                    break;
                case NetworkMode.Server:

                    BeginServer();

                    break;
                default:
                    break;
            }
        }

        private void BeginServer()
        {
            if (NetworkManager.Singleton.StartServer())
            {
                OnStartedServer.Invoke();
            }
        }

        private void BeginClient()
        {
            var unetTransport = this.GetComponent<UnityTransport>();

            unetTransport.ConnectionData.Address = networkAddress;
            unetTransport.ConnectionData.Port = networkPort;

            if(NetworkManager.Singleton.StartClient())
            {
                OnStartedClient.Invoke();
            }
        }
    }

    [Serializable]
    public enum NetworkMode
    {
        Client,
        Server
    }
}

