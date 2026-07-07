using UnityEngine;

[CreateAssetMenu(fileName = "BallSettings", menuName = "Scriptable Objects/BallSettings")]
public class BallSettings : ScriptableObject
{
    [Header("General Settings")]
    public float gravity = 10;
    
    [Header("Carried Ball Settings")]
    public float dribbleFrequency = 8f;
    public float dribbleIntensity = 0.15f;
    public Vector2 dribbleOffset = new Vector2(0.75f, 0f);
    
    [Header("Freeform Ball Settings")]
    public float recaptureGraceTime = 0.15f;
    public float frictionAir = 3.5F;
    public float frictionGround = 10F;
    public float bounciness = 0.8F;
    
    [Header("Shot Ball Settings")]
    public float shotDuration = 1f;
    public float shotSpriteScale = 0.7f;
    public float shotHeight = 1f;
}
