
using Unity.VisualScripting;
using UnityEngine;

public class RunningState : PlayerState
{

    
    public RunningState(Player player, PlayerStateMachine machine) : base(player, machine)
    {}

    public override void Enter()
    {
    }

    public override void Tick()
    {
        MovePlayer();

        if (player.HasBall)
        {
            if (player.InputProvider.GetActionPressedThisFrame())
            {
                machine.ChangeState(new PreppingShot(player, machine));
            }
            if (player.InputProvider.GetPassPressedThisFrame())
            {
                machine.ChangeState(new PassingState(player, machine));
            }
        }
        else
        {
            if (player.Rigidbody.linearVelocity == Vector2.zero)
            {
                if (player.InputProvider.GetActionPressedThisFrame() && player.Ball.Machine.CurrentState.CanAirInteract())
                {
                    machine.ChangeState(new HeaderState(player, machine, player.BallDetectionArea));
                } 
            }
            if (player.InputProvider.GetActionPressedThisFrame() && player.Rigidbody.linearVelocity.sqrMagnitude > 0)
            {
                machine.ChangeState(new TacklingState(player, machine));
            } 
        }




       
    }

    private void MovePlayer()
    {
        Vector2 direction = player.InputProvider.GetMoveInput().normalized;

        if (direction.sqrMagnitude > 0)
        {
            player.Move(direction);
            player.PlayAnimation(Animations.RUN_ANIMATION);
        }
        else
        {
            player.Stop();
            player.PlayAnimation(Animations.IDLE_ANIMATION);
        }
    }

    public override void Exit()
    {
    }
}
