
using UnityEngine;

public class ShotState : BallState
{
    private const string ROLLING_ANIMATION = "roll";
    private const float SHOT_DURATION = 1f;

    private const float SHOT_SPRITE_SCALE = 0.7f;
    private const float SHOT_HEIGHT = 1f;

    private float initialSpeed;
    private float timeElapsed;
    
    private Transform ballSpriteTransform;
    private Vector3 ballOriginalScale;
    private Vector3 ballOriginalPosition;
    
    
    
    public ShotState(Ball ball, BallStateMachine machine, Collider2D playerDetectionArea, Transform contextCarrier) : base(ball, machine, playerDetectionArea, contextCarrier)
    {
        ballSpriteTransform = ball.SpriteRenderer.transform;
        ballOriginalScale = ballSpriteTransform.localScale;
        ballOriginalPosition = ballSpriteTransform.localPosition;
    }

    public override void Enter()
    {
        
        ball.PlayAnimation(ROLLING_ANIMATION);
        ball.SpriteRenderer.flipX = ball.Rigidbody.linearVelocity.x < 0;
        
        initialSpeed = ball.Rigidbody.linearVelocity.magnitude;
        timeElapsed = 0;
        
        //ballSpriteTransform.localScale = new Vector3(ballSpriteTransform.localScale.x, SHOT_SPRITE_SCALE, ballSpriteTransform.localScale.z);
        
        //float initialYOffset = (ballOriginalScale.y - SHOT_SPRITE_SCALE) * 0.5f;
        //ballSpriteTransform.localPosition = new Vector3(ballOriginalPosition.x, ballOriginalPosition.y - initialYOffset, ballOriginalPosition.z);

        ball.Trail.SetActive(true);

    }

    public override void Tick()
    {
        SquishBall();
        timeElapsed += Time.deltaTime;

        ProcessGravity();
        if (timeElapsed >=  SHOT_DURATION)
        {
            //ball.Rigidbody.linearVelocity = Vector2.zero;
            machine.ChangeState(new FreeformState(ball, machine, playerDetectionArea, null));
        }
    }

    private void SquishBall()
    {
        Vector2 currentVelocity = ball.Rigidbody.linearVelocity;
        float currentSpeed = currentVelocity.magnitude;
        
        //ball.Rigidbody.linearVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, ball.deceleration * Time.deltaTime);

        if (initialSpeed > 0.01f)
        {
            float speedRatio = Mathf.Clamp01(currentSpeed / initialSpeed);
            float easedRatio = Mathf.SmoothStep(0f, 1f, speedRatio);
            
            float targetYScale = Mathf.Lerp(ballOriginalScale.y, SHOT_SPRITE_SCALE, easedRatio);
            
            
            ballSpriteTransform.localScale = new Vector3(ballSpriteTransform.localScale.x, targetYScale, ballSpriteTransform.localScale.z);
            
            /*float currentYOffset = (ballOriginalScale.y - targetYScale);
            ballSpriteTransform.localPosition = new Vector3(
                ballOriginalPosition.x, 
                ballOriginalPosition.y +  SHOT_HEIGHT, 
                ballOriginalPosition.z
            );*/
        }
    }

    public override void Exit()
    {
        ballSpriteTransform.localScale = ballOriginalScale;
        //ballSpriteTransform.localPosition = ballOriginalPosition;
        ball.Trail.SetActive(false);
    }
}
