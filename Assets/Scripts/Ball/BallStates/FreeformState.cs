using UnityEngine;

public class FreeformState : BallState
{
    private static readonly Collider2D[] overlapResults = new Collider2D[4];
    private readonly ContactFilter2D filter;

    private float enterTime;
    Player carrierPlayer;
    
    public FreeformState(Ball ball, BallStateMachine machine, Collider2D playerDetectionArea, Transform carrier) : base(ball, machine,  playerDetectionArea,  carrier)
    {
        filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("Player"));
        filter.useLayerMask = true;
        filter.useTriggers = false;
    }

    public override void Enter()
    {
        ball.Rigidbody.bodyType = RigidbodyType2D.Dynamic;
        enterTime = Time.time;
        ball.PlayAnimation(Animations.IDLE_ANIMATION);
    }

    public override void Tick()
    {
        SetBallAnimation();
        CheckForPlayerPickup();
        CalculateBallPhysics();
        ProcessGravity(ball.Settings.bounciness);
    }

    private void CalculateBallPhysics()
    {
        var friction = ball.IsInAir ? ball.Settings.frictionAir : ball.Settings.frictionGround;
        ball.Rigidbody.linearVelocity = Vector2.MoveTowards(ball.Rigidbody.linearVelocity, Vector2.zero, friction * Time.deltaTime);
    }

    private void SetBallAnimation()
    {
        if (ball.Rigidbody.linearVelocity == Vector2.zero)
        {
            ball.PlayAnimation(Animations.IDLE_ANIMATION);
        }
        else if (ball.Rigidbody.linearVelocity.x > 0)
        {
            ball.PlayAnimation(Animations.ROLLING_ANIMATION);
        }
        else
        {
            ball.PlayAnimation(Animations.ROLLING_ANIMATION);
            ball.SpriteRenderer.flipX = true;
        }
    }

    private void CheckForPlayerPickup()
    {
        if (Time.time - enterTime < ball.Settings.recaptureGraceTime) return;

        int count = Physics2D.OverlapCollider(playerDetectionArea, filter, overlapResults);
        
        
        if (count == 0) return;
        
        Collider2D hit = overlapResults[0];
        Transform carrierTransform = hit.attachedRigidbody != null ? hit.attachedRigidbody.transform : hit.transform;

        
        carrierTransform.TryGetComponent(out Player carrierPlayer);
        carrierPlayer.SetBall(true);
        machine.ChangeState(new CarriedState(ball, machine, playerDetectionArea, carrierTransform));
    }

    public override bool CanAirInteract()
    {
        return true;
    }
}
