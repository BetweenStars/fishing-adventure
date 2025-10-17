using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [field: SerializeField] public InputActionReference movementAction { get; private set; }
    [field: SerializeField] public InputActionReference interactAction { get; private set; }
    [field: SerializeField] public InputActionReference escapeAction { get; private set; }
    [field: SerializeField] public InputActionReference inventoryAction { get; private set; }

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

    private void Start()
    {
        movementAction.action.Enable();
        interactAction.action.Enable();
        escapeAction.action.Enable();
        inventoryAction.action.Enable();
    }
}
