using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference action;

    private void Awake()
    {
        action.action.performed += TogglePause;
    }

    private void OnDestroy()
    {
        action.action.performed -= TogglePause;
    }

    private void OnEnable()
    {
        action.action.Enable();
    }

    private void OnDisable()
    {
        action.action.Disable();
    }

    public void Pause()
    {
        if (!PauseUtility.IsPaused)
            PauseUtility.TogglePause();
    }

    public void UnPause()
    {
        if (PauseUtility.IsPaused)
            PauseUtility.TogglePause();
    }

    private void TogglePause(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Pause Toggled");
        PauseUtility.TogglePause();
    }
}