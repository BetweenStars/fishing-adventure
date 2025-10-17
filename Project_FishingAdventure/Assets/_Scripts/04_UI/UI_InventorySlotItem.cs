using UnityEngine;
using UnityEngine.UI;

public class UI_InventorySlotItem : MonoBehaviour
{
    [SerializeField] private Image itemImage;

    private void Awake()
    {
        SetItemImage(null);
    }

    public void SetItemImage(Sprite sprite)
    {
        if (sprite == null)
        {
            itemImage.color = ImageUtils.GetTransparencyColor(itemImage.color, 0);
        }
        else
        {
            itemImage.color = Color.white;
        }
        itemImage.sprite = sprite;
    }
}
