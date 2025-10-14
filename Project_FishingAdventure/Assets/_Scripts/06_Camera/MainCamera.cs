using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Vector3 offset = new Vector3(0f, 0f, 8f);
    [SerializeField]
    private float smoothSpeed = 5.0f;

    private void LateUpdate()
    {
        if (PlayerManager.player == null) return;

        Vector3 destinationPosition = PlayerManager.player.transform.position + offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, destinationPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
    }
}
