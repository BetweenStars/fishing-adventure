using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public sealed class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public static Player player { get; private set; }

    public static event Action OnPlayerReady;
    public static bool IsPlayerReady { get; private set; } = false;

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

        IsPlayerReady = false;
    }

    private void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    private void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            spawnPointTransform = FindAnyObjectByType<PlayerSpawnPoint>().transform;
            SpawnPlayer();
        }
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
                OnPlayerReady?.Invoke();
                IsPlayerReady = true;
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
