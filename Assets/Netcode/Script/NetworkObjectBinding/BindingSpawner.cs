using Unity.Netcode;
using UnityEngine;
using Klak.Spout;

public class BindingSpawner : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private GameObject itemPrefab;

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
        }
    }

    private void OnServerStarted()
    {
        Spawn();
    }

    private void Spawn()
    {
        var chracterObj = Instantiate(characterPrefab);
        chracterObj.GetComponent<NetworkObject>().Spawn();
        var itemObj = Instantiate(itemPrefab);
        itemObj.GetComponent<NetworkObject>().Spawn();

        Transform hangRootTransform = chracterObj.transform.Find("hangRoot");
        //itemObj.transform.parent = hangRootTransform;
        itemObj.transform.parent = chracterObj.transform;
        itemObj.transform.localPosition = hangRootTransform.localPosition;
    }

}
