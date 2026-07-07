using UnityEngine;

public abstract class BallState
{
    private const float GRAVITY = 10;
    
    protected readonly Ball ball;
    protected readonly BallStateMachine machine;
    protected readonly Collider2D playerDetectionArea;
    protected readonly Transform carrier;
    protected readonly BallSettings settings;
    
    public BallState(Ball ball, BallStateMachine machine, Collider2D playerDetectionArea, Transform contextCarrier)
    {
        this.ball = ball;
        this.machine = machine;
        this.playerDetectionArea = playerDetectionArea;
        this.carrier = contextCarrier;
        this.settings = ball.Settings;
        
    }


    public virtual void Enter()
    {
    }

    public abstract void Tick();

    public virtual void Exit()
    {
    }

    public void ProcessGravity(float bounciness = 0f)
    {
        if (ball.IsInAir || ball.HeightVelocity > 0)
        {
            ball.ApplyHeightGravity(GRAVITY * Time.deltaTime);
            
            if (!ball.IsInAir && ball.HeightVelocity < 0)
            {
                if (bounciness > 0f && ball.HeightVelocity < -1.5f)
                {
                    ball.InvertVelocity(bounciness);
                    ball.Rigidbody.linearVelocity *= bounciness;
                }
                else
                {
                    ball.ResetVerticalVelocity();
                }
            }
        }
    }

        
}
