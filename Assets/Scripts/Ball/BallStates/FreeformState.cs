using UnityEngine;

public class FreeformState : BallState
{
    private const string IDLE_ANIMATION = "idle";
    private const string ROLL_ANIMATION = "roll";
    private const float RECAPTURE_GRACE_TIME = 0.15f;

    private const float FRICTION_AIR = 3.5F;
    private const float FRICTION_GROUND = 10F;
    private const float BOUNCINESS = 0.8F;
    
    private static readonly Collider2D[] overlapResults = new Collider2D[4];
    private readonly ContactFilter2D filter;

    private float enterTime;
    Player carrierPlayer;
    
    public FreeformState(Ball ball, BallStateMachine machine, Collider2D playerDetectionArea, Transform carrier) : base(ball, machine,  playerDetectionArea,  carrier)
    {
        filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("Player"));
        filter.useTriggers = true;
    }

    public override void Enter()
    {
        ball.Rigidbody.bodyType = RigidbodyType2D.Dynamic;
        enterTime = Time.time;
        ball.PlayAnimation(IDLE_ANIMATION);
    }

    public override void Tick()
    {
        SetBallAnimation();
        CheckForPlayerPickup();
       

        var friction = ball.IsInAir ? FRICTION_AIR : FRICTION_GROUND;
        ball.Rigidbody.linearVelocity = Vector2.MoveTowards(ball.Rigidbody.linearVelocity, Vector2.zero, friction * Time.deltaTime); 
        ProcessGravity(BOUNCINESS);
    }

    private void SetBallAnimation()
    {
        if (ball.Rigidbody.linearVelocity == Vector2.zero)
        {
            ball.PlayAnimation(IDLE_ANIMATION);
        }
        else if (ball.Rigidbody.linearVelocity.x > 0)
        {
            ball.PlayAnimation(ROLL_ANIMATION);
        }
        else
        {
            ball.PlayAnimation(ROLL_ANIMATION);
            ball.SpriteRenderer.flipX = true;
        }
    }

    private void CheckForPlayerPickup()
    {
        if (Time.time - enterTime < RECAPTURE_GRACE_TIME) return;

        int count = Physics2D.OverlapCollider(playerDetectionArea, filter, overlapResults);
        
        
        if (count == 0) return;
        
        Collider2D hit = overlapResults[0];
        Transform carrierTransform = hit.attachedRigidbody != null ? hit.attachedRigidbody.transform : hit.transform;

        
        carrierTransform.TryGetComponent(out Player carrierPlayer);
        carrierPlayer.SetBall(true);
        machine.ChangeState(new CarriedState(ball, machine, playerDetectionArea, carrierTransform));
    }
}
