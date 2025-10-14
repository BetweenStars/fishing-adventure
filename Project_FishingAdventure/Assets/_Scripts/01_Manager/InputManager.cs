using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [SerializeField]
    private InputActionReference _movementAction;
    public InputActionReference movementAction => _movementAction;
    [SerializeField]
    private InputActionReference _interactAction;
    public InputActionReference interactAction => _interactAction;
    [SerializeField]
    InputActionReference _escapeAction;
    public InputActionReference escapeAction => _escapeAction;

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
    }
}
