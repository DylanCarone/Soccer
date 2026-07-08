using UnityEngine;

public class TacklingState : PlayerState
{
    private float tackleStart = 0;
    private float groundFriction = 5f;
    
    public TacklingState(Player player, PlayerStateMachine machine) : base(player, machine)
    {
    }

    public override void Enter()
    {
        tackleStart = Time.time;
        player.PlayAnimation(Animations.TACKLE_ANIMATION);
        //player.Stop();
    }

    public override void Tick()
    {  
        if (Time.time - tackleStart > player.TackleDuration)
        {
            machine.ChangeState(new RecoveringState(player, machine));
        }
        else
        {
            player.Rigidbody.linearVelocity = Vector2.MoveTowards(
                player.Rigidbody.linearVelocity, 
                Vector2.zero, 
                groundFriction * Time.deltaTime );
        }
        
    }
    
    
}
