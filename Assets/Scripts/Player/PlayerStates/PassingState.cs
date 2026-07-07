
using UnityEngine;

public class PassingState : PlayerState
{

    private const float MAX_HOLD_TIME = 0.1f;

    private float holdStartTime;
    private Vector2 aimDirection;
    
    public PassingState(Player player, PlayerStateMachine machine) : base(player, machine)
    {
    }

    public override void Enter()
    {
        player.Rigidbody.linearVelocity = Vector2.zero;
        holdStartTime = Time.time;
        aimDirection = player.Flipped ? Vector2.left : Vector2.right;
        player.PlayAnimation(Animations.KICK_ANIMATION);
    }

    public override void Tick()
    {
        Vector2 currentInput = player.InputProvider.GetMoveInput();

        if (currentInput != Vector2.zero)
        {
            aimDirection = currentInput.normalized;
            player.FlipPlayer(currentInput);
        }
        
        float holdDuration = Time.time - holdStartTime;

        if (!player.InputProvider.GetPassHeld() || holdDuration >= MAX_HOLD_TIME)
        {
            ExecutePass();
        }
        
        
    }


    void ExecutePass()
    {
        Player target = PassTargetFinder.FindClosestPlayer(player.transform.position, aimDirection, 10f, 20f, player);
        
        Vector2 direction = target != null ? ((Vector2)target.transform.position - (Vector2)player.transform.position).normalized : aimDirection; 
        
        Debug.Log(target);
        float distance = target != null ? Vector2.Distance(player.transform.position, target.transform.position) : 0;
        player.Ball.Pass(direction, player.PassPower, distance);
        player.SetBall(false);
        machine.ChangeState(new RunningState(player, machine));

    }
}
