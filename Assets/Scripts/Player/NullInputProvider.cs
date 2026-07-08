
using UnityEngine;

public class NullInputProvider : IInputProvider
{
    public Vector2 GetMoveInput()
    {
        return Vector2.zero;
    }

    public bool GetActionPressedThisFrame()
    {
        return false;
    }

    public bool GetActionHeld()
    {
        return false;
    }

    public bool GetActionReleasedThisFrame()
    {
        return false;
    }

    public bool GetPassPressedThisFrame()
    {
        return false;
    }

    public bool GetPassHeld()
    {
        return false;
    }

    public bool GetPassReleasedThisFrame()
    {
        return false;
    }
}
