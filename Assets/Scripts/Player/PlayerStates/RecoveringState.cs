
using UnityEngine;

public class RecoveringState : PlayerState
{

    private float recoverTimer;
    
    public RecoveringState(Player player, PlayerStateMachine machine) : base(player, machine)
    {
    }

    public override void Enter()
    {
        player.Stop();
        player.PlayAnimation(Animations.RECOVER_ANIMATION);
        recoverTimer = Time.time;
        
    }

    public override void Tick()
    {
        if (Time.time - recoverTimer > player.RecoverDuration)
        {
            machine.ChangeState(new RunningState(player, machine));
        }
    }
}
