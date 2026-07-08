
using System;
using UnityEngine;

public class CarriedState : BallState
{
  
    private Player carrierPlayer;
    private float dribbleTime;

    private float vx;
    
    public CarriedState(Ball ball, BallStateMachine machine, Collider2D playerDetectionArea, Transform contextCarrier) : base(ball, machine, playerDetectionArea, contextCarrier)
    {
    }


    public override void Enter()
    {
        ball.Rigidbody.bodyType = RigidbodyType2D.Kinematic;
        ball.transform.SetParent(carrier);
        ball.transform.localPosition = Vector3.zero;
        ball.PlayAnimation(Animations.IDLE_ANIMATION);
        carrier.TryGetComponent(out carrierPlayer);
        
        ball.HandlePossessionChange(carrierPlayer);
    }

    public override void Tick()
    {
        bool leftSide = carrierPlayer != null && carrierPlayer.Flipped;
        dribbleTime += Time.deltaTime;
        
        
        BallAnimation();
        BallOffsetAndFlip(leftSide);
        ProcessGravity();
    }

    private void BallAnimation()
    {
        if (carrierPlayer.Rigidbody.linearVelocity.magnitude != 0)
        {
            ball.PlayAnimation(Animations.ROLLING_ANIMATION);
            DribbleBall();
        }
        else
        {
            ball.PauseAnimation();
        }
    }

    private void DribbleBall()
    {
        if (carrierPlayer.Rigidbody.linearVelocity.x != 0)
        {
            vx = Mathf.Cos(dribbleTime * ball.Settings.dribbleFrequency) *  ball.Settings.dribbleIntensity;
        }
    }

    private void BallOffsetAndFlip(bool leftSide)
    {
        var offset = ball.Settings.dribbleOffset;
        ball.transform.position = leftSide ? carrier.position + new Vector3(vx + -offset.x,offset.y,0) :
            carrier.position + new Vector3(vx + offset.x,offset.y,0);
        ball.SpriteRenderer.flipX = leftSide;
    }

    public override void Exit()
    {
        ball.transform.SetParent(null);
    }
}
