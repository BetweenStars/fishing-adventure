using UnityEngine;
using UnityEngine.UI;

public class UI_LoadGameBtn : MonoBehaviour
{
    private Button btn;
    private Image image;

    private void Awake()
    {
        btn = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (SaveLoadManager.Instance != null)
        {
            if (SaveLoadManager.Instance.HasSaveData())
            {
                btn.enabled = true;
                image.color = ImageUtils.GetTransparencyColor(image.color, 1.0f);
            }
            else
            {
                btn.enabled = false;
                image.color = ImageUtils.GetTransparencyColor(image.color, 0.7f);
            }
        }
    }
}
