
using System;
using UnityEngine;

public class CarriedState : BallState
{
    private const string ROLLING_ANIMATION = "roll";
    private const string IDLE_ANIMATION = "idle";
    
    private const float DRIBBLE_FREQUENCY = 8f;
    private const float DRIBBLE_INTENSITY = 0.15f;
    
    private Vector3 offset;


    
    private Player carrierPlayer;
    private float dribbleTime;

    private float vx;
    
    public CarriedState(Ball ball, BallStateMachine machine, Collider2D playerDetectionArea, Transform contextCarrier) : base(ball, machine, playerDetectionArea, contextCarrier)
    {
        offset = new Vector3(.75f, 0f, 0f);
    }


    public override void Enter()
    {
        ball.Rigidbody.bodyType = RigidbodyType2D.Kinematic;
        ball.transform.SetParent(carrier);
        ball.transform.localPosition = Vector3.zero;
        ball.PlayAnimation(IDLE_ANIMATION);
        carrier.TryGetComponent(out carrierPlayer);
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
            ball.PlayAnimation(ROLLING_ANIMATION);
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
            vx = Mathf.Cos(dribbleTime * DRIBBLE_FREQUENCY) *  DRIBBLE_INTENSITY;
        }
    }

    private void BallOffsetAndFlip(bool leftSide)
    {
        ball.transform.position = leftSide ? carrier.position + new Vector3(vx + -offset.x,offset.y,offset.z) :
            carrier.position + new Vector3(vx + offset.x,offset.y,offset.z);
        ball.SpriteRenderer.flipX = leftSide;
    }

    public override void Exit()
    {
        ball.transform.SetParent(null);
    }
}
