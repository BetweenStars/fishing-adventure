using UnityEngine;
using UnityEngine.UI;

public class UI_SaveGameBtn : MonoBehaviour
{
    private Button btn;
    void Awake()
    {
        btn = GetComponent<Button>();
    }

    void Start()
    {
        if (SaveLoadManager.Instance != null)
        {
            btn.onClick.AddListener(SaveLoadManager.Instance.SaveGame);
            Debug.Log("save event sub");
        }
    }
}
