
using UnityEngine;

public class PreppingShot : PlayerState
{
    
    const float DURATION_MAX_BONUS = 1f;
    private const float EASE_REWARD_FACTOR = 0.3f;
    private float timeStartShot;
    Vector2 shotDirection =  Vector2.zero;
    
    public PreppingShot(Player player, PlayerStateMachine machine) : base(player, machine)
    {
    }

    public override void Enter()
    {
        player.PlayAnimation(Animations.PREP_ANIMATION);
        player.Rigidbody.linearVelocity = Vector2.zero;
        
        timeStartShot = Time.time;
        shotDirection = Vector2.zero;
    }

    public override void Tick()
    {
        Vector2 currentInput = player.InputProvider.GetMoveInput();
        player.FlipPlayer(currentInput);
        if (currentInput != Vector2.zero)
        {
            shotDirection = currentInput.normalized; 
        }
        
        
        var durationPress = Time.time - timeStartShot;
        
        if (!player.InputProvider.GetActionHeld() || durationPress >= DURATION_MAX_BONUS)
        {
            ExecuteShot(durationPress);
        }
        else
        {
            float bonus = CalculateBonus(durationPress);
            float shotPower = player.Power * (1 + bonus);
            Vector2 finalDirection = shotDirection;
            
        }
    }

    private void ExecuteShot(float finalDuration)
    {
        Vector2 lookDirection = player.Flipped ? Vector2.left : Vector2.right;
        float finalBonus = CalculateBonus(finalDuration);
        float finalPower = player.Power * (1 + finalBonus);
        Vector2 finalDirection = shotDirection == Vector2.zero ? lookDirection : shotDirection;

        if (Vector2.Dot(finalDirection, lookDirection) < 0)
        {
            // use this to prevent the player from shooting behind -- just disable the look direction line
            //finalDirection = new Vector2(0, Mathf.Sign(finalDirection.y));
        }
        
        
        
        machine.ChangeState(new ShootingState(player, machine, finalPower, finalDirection));
    }
    private float CalculateBonus(float duration)
    {
        float easeTime = Mathf.Clamp01(duration / DURATION_MAX_BONUS);
        return Mathf.SmoothStep(0f, EASE_REWARD_FACTOR, easeTime);
    }
}
