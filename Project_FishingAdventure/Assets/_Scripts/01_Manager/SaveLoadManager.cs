using UnityEngine;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;
    private const string SAVE_FILE_NAME = "/savegame.json";
    public GameSaveData gameSaveData { get; private set; }

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

    public void SaveGame()
    {
        try
        {
            gameSaveData.money = PlayerManager.player.playerMoney.money;
            Debug.Log("[SaveDataManager] Data Sync Success");
        }
        catch (System.Exception e)
        {
            Debug.Log($"[SaveLoadManager] Data Sync failed. {e.Message}");
            return;
        }
        string json = JsonUtility.ToJson(gameSaveData, true);

        string filePath = Application.persistentDataPath + SAVE_FILE_NAME;

        try
        {
            File.WriteAllText(filePath, json);
            Debug.Log($"[SaveLoadManager] 게임 저장 완료: {filePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[SaveLoadManager] 파일 저장 실패: {e.Message}");
        }
    }

    public void LoadGame()
    {
        string filePath = Application.persistentDataPath + SAVE_FILE_NAME;

        if (!File.Exists(filePath))
        {
            Debug.LogWarning("[SaveLoadManager] 저장 파일이 존재하지 않아 로드하지 않습니다.");
        }

        try
        {
            string json = File.ReadAllText(filePath);

            GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);

            Debug.Log($"[SaveLoadManager] 게임 로드 완료. 돈: {data.money}");
            gameSaveData = data;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[SaveLoadManager] 파일 로드 실패: {e.Message}");
        }
    }

    public void NewGame()
    {
        gameSaveData = new() { money = 1500.0, fishDatas = new() };
        Debug.Log("[SaveLoadManager] New Data Created");
    }

    public bool HasSaveData()
    {
        string filePath = Application.persistentDataPath + SAVE_FILE_NAME;
        if (File.Exists(filePath)) { return true; }
        else { return false; }
    }
}