using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputProvider : MonoBehaviour, IInputProvider
{
    [Header("Multiplayer Testing")] 
    [SerializeField] private int forceGamepadIndex = 0;
    [SerializeField] bool forceKeyboard = false;
    
    private PlayerInput playerInput;
    private PlayerControls controls;
    private Vector2 moveInput;
    
    
    bool actionHeld;
    bool actionReleasedThisFrame;
    bool actionPressedThisFrame;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        controls = new PlayerControls();

        ForceControllerDebug();
    }

    private void ForceControllerDebug()
    {
        if (forceKeyboard)
        {
            if (Keyboard.current != null)
            {
                controls.devices = new ReadOnlyArray<InputDevice>(new InputDevice[]{Keyboard.current});
            }
        }
        else if (forceGamepadIndex > 0)
        {
            int targetIndex = forceGamepadIndex - 1;
            if (Gamepad.all.Count > targetIndex)
            {
                Gamepad targetGamepad = Gamepad.all[targetIndex];
                controls.devices = new ReadOnlyArray<InputDevice>(new InputDevice[]{targetGamepad});
                Debug.Log(targetGamepad.name);
            }
            else
            {
                Debug.LogWarning($"[{gameObject.name}] No Gamepad found for index {forceGamepadIndex}]");
                controls.devices = playerInput.devices;
            }
        }
        else
        {
            controls.devices = playerInput.devices;
        }
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Move.performed += HandleMove;
        controls.Player.Move.canceled += HandleMove;
        controls.Player.Action.performed += HandleActionPressed;
        controls.Player.Action.canceled += HandleActionReleased;
    }

    private void OnDisable()
    {
        controls.Player.Move.performed -= HandleMove;
        controls.Player.Move.canceled -= HandleMove;
        controls.Player.Action.performed -= HandleActionPressed;
        controls.Player.Action.canceled -= HandleActionReleased;
        controls.Player.Disable();
    }

    private void LateUpdate()
    {
        actionReleasedThisFrame = false;
        actionPressedThisFrame = false;
    }

    void HandleMove(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>();
    void HandleActionPressed(InputAction.CallbackContext ctx)
    {
        actionHeld = true;
        actionPressedThisFrame = true;
    }

    void HandleActionReleased(InputAction.CallbackContext ctx)
    {
        actionHeld = false;
        actionReleasedThisFrame = true;
    }

    public Vector2 GetMoveInput() => moveInput;
    
    public bool GetActionPressedThisFrame()
    {
        return actionPressedThisFrame;
    }

    public bool GetActionHeld()
    {
        return actionHeld;
    }

    public bool GetActionReleasedThisFrame()
    {
        return actionReleasedThisFrame;
    }
}
