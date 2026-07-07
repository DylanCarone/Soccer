public class BallStateMachine
{
    public BallState CurrentState { get; private set; }


    public void Initialize(BallState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(BallState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
    
    public void Tick() => CurrentState?.Tick();
    
}