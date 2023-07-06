using Unity.Netcode;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int targetFrameRate = 30;

    [SerializeField]
    private GameObject ojisanPrefab;

    private NetworkManager networkManager;

    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
        networkManager = FindObjectOfType<NetworkManager>();
        if (networkManager != null)
        {
            networkManager.OnServerStarted += SupplyObjects;
            //networkManager.OnClientConnectedCallback += OnClientConnected;
            //networkManager.OnClientDisconnectCallback += OnClientDisconnected;
        }

    }

    private void SupplyObjects()
    {
        if (ojisanPrefab != null)
        {
            var ojisan = Instantiate(ojisanPrefab);
            var ojisanScript = ojisan.GetComponent<Ojisan>();
            ojisanScript.color = Color.red;
            ojisan.GetComponent<NetworkObject>().Spawn();
        }
    }

}
