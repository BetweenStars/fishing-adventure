using UnityEngine;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;
    private const string SAVE_FILE_NAME = "/savegame.json";
    public GameSaveData gameSaveData{ get; private set; }

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

    /// <summary>
    /// 현재 GameSaveData 객체를 JSON 파일로 저장합니다.
    /// </summary>
    /// <param name="data">저장할 데이터 객체</param>
    public void SaveGame()
    {
        //데이터 동기화
        try
        {
            gameSaveData.money = PlayerManager.player.playerMoney.money;
            Debug.Log("[SaveDataManager] Data Sync Success");
        }
        catch(System.Exception e)
        {
            Debug.Log($"[SaveLoadManager] Data Sync failed. {e.Message}");
            return;
        }

            // 1. 객체를 JSON 문자열로 직렬화
            string json = JsonUtility.ToJson(gameSaveData, true); // true는 가독성(pretty print)을 높여줍니다.

        // 2. 저장 경로 설정 (플랫폼별 영구 데이터 경로)
        string filePath = Application.persistentDataPath + SAVE_FILE_NAME;

        try
        {
            // 3. 파일에 JSON 문자열 쓰기
            File.WriteAllText(filePath, json);
            Debug.Log($"[SaveLoadManager] 게임 저장 완료: {filePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[SaveLoadManager] 파일 저장 실패: {e.Message}");
        }
    }

    /// <summary>
    /// JSON 파일에서 GameSaveData 객체를 불러옵니다.
    /// </summary>
    /// <returns>불러온 GameSaveData 객체, 실패 시 null</returns>
    public void LoadGame()
    {
        string filePath = Application.persistentDataPath + SAVE_FILE_NAME;

        // 1. 저장 파일이 존재하는지 확인
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("[SaveLoadManager] 저장 파일이 존재하지 않아 로드하지 않습니다.");
        }

        try
        {
            // 2. 파일에서 JSON 문자열 읽기
            string json = File.ReadAllText(filePath);

            // 3. JSON 문자열을 GameSaveData 객체로 역직렬화
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
        if(File.Exists(filePath)){ return true; }
        else{ return false; }
    }
}