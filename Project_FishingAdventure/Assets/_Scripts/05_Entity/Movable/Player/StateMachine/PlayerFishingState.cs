using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFishingState : BasePlayerState
{
    public PlayerFishingState() { state = PlayerState.FISHING; }

    private Coroutine fishingCoroutine = null;

    private bool isFishingSuccess = false;
    private bool isFishing = false;
    private bool isBiting = false;

    private FishDef_SO baitedFish;

    public override void EnterState()
    {
        base.EnterState();

        isFishingSuccess = false;
        isBiting = false;
        isFishing = true;

        WorldManager.ship.DeactivateControl();

        InputManager.Instance.interactAction.action.started += HandleClicked;

        fishingCoroutine = stateManager.StartStateCoroutine(FishingCoroutine());
    }

    public override void ExitState()
    {
        base.ExitState();

        InputManager.Instance.interactAction.action.started -= HandleClicked;

        if (fishingCoroutine != null) stateManager.StopStateCoroutine(fishingCoroutine);
    }

    private void HandleClicked(InputAction.CallbackContext context)
    {
        isFishing = false;
        if(isBiting){ isFishingSuccess = true; }
    }

    private IEnumerator FishingCoroutine()
    {
        Debug.Log("Waiting for Fish...");
        FishingManager.Instance.fishingLine.StartLine(PlayerManager.player.rotTipTransform, PlayerManager.player.playerInteract.interactedPos);
        float waitTime = Random.Range(1.5f, 4.0f);
        float deltaTime = 0.0f;
        while (deltaTime <= waitTime)
        {
            if (!isFishing) break;

            deltaTime += Time.deltaTime;
            yield return null;
        }

        if (isFishing)
        {

            baitedFish = FishingManager.Instance.BaitedFish();

            Debug.Log("Fish is biting! Click Now!");
            isBiting = true;
            float inputTime = 2.0f;
            deltaTime = 0.0f;
            while (deltaTime <= inputTime)
            {
                if (!isFishing) break;

                deltaTime += Time.deltaTime;
                yield return null;
            }
            isBiting = false;
        }

        Debug.Log("fishing end!");

        FishingManager.Instance.fishingLine.EndLine();

        if (!isFishingSuccess)
        {
            Debug.Log("Fish ran way...");
            yield return new WaitForSeconds(1.0f);
            stateManager.ChangeState(new PlayerRidingState());
        }
        else
        {
            Debug.Log($"You got {baitedFish.entityName}!");
            PlayerManager.player.playerMoney.AddMoney(1000);
            FishingManager.Instance.CaughtFish(baitedFish);
            yield return new WaitForSeconds(0.5f);
            stateManager.ChangeState(new PlayerRidingState());
        }
    }
}