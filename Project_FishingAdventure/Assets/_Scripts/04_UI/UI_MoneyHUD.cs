using UnityEngine;
using UnityEngine.UI;

public class UI_MoneyHUD : MonoBehaviour
{
    private Text text;

    private void Awake()
    {
        text = GetComponentInChildren<Text>();
    }

    private void OnEnable()
    {
        PlayerManager.OnPlayerReady += HandlePlayerReady;

        if (PlayerManager.IsPlayerReady) { HandlePlayerReady(); }
    }
    private void OnDisable()
    {
        PlayerManager.OnPlayerReady -= HandlePlayerReady;

        if (PlayerManager.player.playerMoney != null)
        {
            PlayerManager.player.playerMoney.OnMoneyChanged -= SetText;
        }
    }
    private void OnDestroy()
    {
        OnDisable();
    }

    private void HandlePlayerReady() { PlayerManager.player.playerMoney.OnMoneyChanged += SetText; }
    private void SetText(double money) { text.text = money.ToString(); }
}
