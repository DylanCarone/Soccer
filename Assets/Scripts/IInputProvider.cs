using UnityEngine;

public interface IInputProvider
{
    Vector2 GetMoveInput();
    bool GetActionPressedThisFrame();
    bool GetActionHeld();
    bool GetActionReleasedThisFrame();
}
