using UnityEngine;

public abstract class PlayerState
{
    protected readonly Player player;
    protected readonly PlayerStateMachine machine;
    protected  Collider2D ballDetectionArea;
    
    public PlayerState(Player player, PlayerStateMachine machine, Collider2D ballDetectionArea = null)
    {
        this.player = player;
        this.machine = machine;
        this.ballDetectionArea = ballDetectionArea;
    }


    public virtual void Enter()
    {
    }

    public abstract void Tick();

    public virtual void Exit()
    {
    }
    
    public virtual void OnAnimationFinished(){}

}
