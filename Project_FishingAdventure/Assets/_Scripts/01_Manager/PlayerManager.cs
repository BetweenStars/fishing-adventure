using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public sealed class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public static Player player { get; private set; }

    [SerializeField]
    private Transform spawnPointTransform;

    [Header("Addressable Settings")]
    [SerializeField]
    private string playerPrefabAddress = "Player_Prefab";
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
        SpawnPlayer();
    }

    private void OnDestroy()
    {
        ReleasePlayer();
    }

    private void SpawnPlayer()
    {
        if (player != null)
        {
            Debug.Log("[Player Manager] Player Already Exist...");
            return;
        }

        instantiateHandle = Addressables.InstantiateAsync(
            playerPrefabAddress,
            spawnPointTransform.position,
            Quaternion.identity,
            WorldManager.Instance.entityParent
        );

        instantiateHandle.Completed += OnPlayerInstantiated;
    }

    private void OnPlayerInstantiated(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject playerGO = handle.Result;
            player = playerGO.GetComponent<Player>();

            if (player != null)
            {
                Debug.Log("[Player Manager] Player Spawn Success");
            }
            else
            {
                Debug.LogError($"[Player Manager] Player Spawn Failed");
                Addressables.Release(handle);
            }
        }
        else
        {
            Debug.LogError($"[Player Manager] Player Spawn Failed : {handle.OperationException}");
        }
    }

    public void ReleasePlayer()
    {
        if (instantiateHandle.IsValid())
        {
            Addressables.Release(instantiateHandle);
            player = null;
            Debug.Log("[Player Manager] Player Unload Complete");
        }
    }
}
