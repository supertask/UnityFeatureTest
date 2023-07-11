using Unity.Netcode;
using UnityEngine;
using Klak.Spout;

public enum TimeType
{
    Server,
    Local,
    Custom,
    Flower
}

public class Spawner : MonoBehaviour
{
    public int targetFrameRate = 30;
    //public TimeType timeType; 
    public Transform cameraTransform;
    public SpoutSender spoutSender;

    [SerializeField] private GameObject ojisanPrefab;
    [SerializeField] private GameObject characterPrefab;

    private NetworkManager networkManager;

    void Start()
    {
        if (NetworkManager.Singleton.IsClient == false)
        {
        }

        networkManager = FindObjectOfType<NetworkManager>();
        if (networkManager != null)
        {
            networkManager.OnServerStarted += OnServerStarted;
            networkManager.OnClientConnectedCallback += OnClientConnected;
            //networkManager.OnClientDisconnectCallback += OnClientDisconnected;
        }

    }

    private void Update()
    {
    }

    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsClient)
        {
            cameraTransform.position = new Vector3(10, 0, 0);
            spoutSender.spoutName = "Client";
        }
    }

    private void OnServerStarted()
    {
        cameraTransform.position = new Vector3(0, 0, 0);
        spoutSender.spoutName = "Server";
        Spawn();
    }

    private void Spawn()
    {
        if (ojisanPrefab != null)
        {
            var line = Instantiate(ojisanPrefab);
            var lineScript = line.GetComponent<LineOfTime>();
            lineScript.color = Color.red;
            lineScript.cameraTransform = cameraTransform;
            //lineScript.timeType = timeType;
            line.GetComponent<NetworkObject>().Spawn();
            //line.transform.parent = cameraTransform;
        }
        if (characterPrefab != null)
        {
            var characterObj = Instantiate(characterPrefab);
            characterObj.GetComponent<NetworkObject>().Spawn();
        }
    }

}
