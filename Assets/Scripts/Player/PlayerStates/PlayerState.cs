public abstract class PlayerState
{
    protected readonly Player player;
    protected readonly PlayerStateMachine machine;
    
    public PlayerState(Player player, PlayerStateMachine machine)
    {
        this.player = player;
        this.machine = machine;
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
