using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;

public class FishDataBase : MonoBehaviour
{
    public const string FishDefLable = "FishDef";
    public static FishDataBase Instance;

    [field: SerializeField] public Dictionary<int, FishDef_SO> fishDefs { get; private set; } = new Dictionary<int, FishDef_SO>();

    private AsyncOperationHandle<IList<FishDef_SO>> loadHandle;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadAllFishDefs();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnDestroy()
    {
        if (loadHandle.IsValid())
        {
            Addressables.Release(loadHandle);
        }
    }

    public FishDef_SO GetRandomFishDef()
    {
        if (fishDefs.Count == 0)
        {
            Debug.LogWarning("[FishDataBase] FishDef data not loaded.");
            return null;
        }

        int[] allKeys = fishDefs.Keys.ToArray();

        int randomIndex = Random.Range(0, allKeys.Length);
        int randomKey = allKeys[randomIndex];

        return fishDefs[randomKey];
    }

    public FishDef_SO GetFishDefByID(int id)
    {
        if(!fishDefs.ContainsKey(id)){ Debug.Log($"[FishDataBase] No Data in ID : {id}"); return null; }
        return fishDefs[id];
    }

    private void LoadAllFishDefs()
    {
        loadHandle = Addressables.LoadAssetsAsync<FishDef_SO>(FishDefLable,
        (fishDef) => { }
        );

        loadHandle.Completed += OnLoadComplete;
    }
    private void OnLoadComplete(AsyncOperationHandle<IList<FishDef_SO>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<FishDef_SO> loadedFishDefs = handle.Result;

            fishDefs.Clear();

            foreach (var fishDef in loadedFishDefs)
            {
                if (fishDef != null && !fishDefs.ContainsKey(fishDef.entityID))
                {
                    fishDefs.Add(fishDef.entityID, fishDef);
                }
            }

            Debug.Log($"[FishDataBase] Fish Data Base Initialize complete! {fishDefs.Count} of FishDef_SO has loaded");
        }
        else
        {
            Debug.Log($"[FishDataBase] FishDef_SO load failed : {handle.OperationException}");
        }
    }
}