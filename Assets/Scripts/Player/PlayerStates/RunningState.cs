
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
        

        if (player.InputProvider.GetActionPressedThisFrame() && player.Rigidbody.linearVelocity.sqrMagnitude > 0 && !player.HasBall)
        {
            machine.ChangeState(new TacklingState(player, machine));
        }
        if (player.InputProvider.GetActionPressedThisFrame() && player.HasBall)
        {
            machine.ChangeState(new PreppingShot(player, machine));
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
