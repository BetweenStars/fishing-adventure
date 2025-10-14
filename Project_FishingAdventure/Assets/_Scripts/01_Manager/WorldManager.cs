using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;
    public static Ship ship;

    [SerializeField]
    private Transform spawnPointTransform;

    [Header("Containers")]
    public Transform entityParent;

    [Header("Addressable Settings")]
    [SerializeField]
    private string shipPrefabAddress = "";
    private AsyncOperationHandle<GameObject> instantiateHandle;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        SpawnShip();
    }

    private void OnDestroy()
    {
        ReleaseShip();
    }

    private void SpawnShip()
    {
        if (ship != null)
        {
            Debug.Log("[World Manager] Player Already Exist...");
            return;
        }

        instantiateHandle = Addressables.InstantiateAsync(
        shipPrefabAddress,
        spawnPointTransform.position,
        Quaternion.identity,
        entityParent
);

        instantiateHandle.Completed += OnShipInstantiated;
    }

    private void OnShipInstantiated(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject shipGO = handle.Result;
            ship = shipGO.GetComponent<Ship>();

            if (ship != null)
            {
                Debug.Log("[World Manager] Ship Spawn Success");
            }
            else
            {
                Debug.LogError($"[World Manager] Ship Spawn Failed");
                Addressables.Release(handle);
            }
        }
        else
        {
            Debug.LogError($"[World Manager] Ship Spawn Failed : {handle.OperationException}");
        }
    }

    public void ReleaseShip()
    {
        if (instantiateHandle.IsValid())
        {
            Addressables.Release(instantiateHandle);
            ship = null;
            Debug.Log("[World Manager] Ship Unload Complete");
        }
    }
}
