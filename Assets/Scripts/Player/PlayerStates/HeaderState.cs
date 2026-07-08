using UnityEngine;

public class HeaderState : PlayerState
{
    private float heightStart = 1;
    private float heightVelocity = 0.1f;
    
    private ContactFilter2D ballFilter;
    private Collider2D[] overlapResults = new Collider2D[5];
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public HeaderState(Player player, PlayerStateMachine machine, Collider2D ballDetectionArea) : base(player, machine)
    {
        this.ballDetectionArea = ballDetectionArea;
        
        ballFilter = new ContactFilter2D();
        ballFilter.SetLayerMask(LayerMask.GetMask("Ball"));
        ballFilter.useLayerMask = true;
        ballFilter.useTriggers = true;
    }

    public override void Enter()
    {
        player.PlayAnimation(Animations.HEADER_ANIMATION);
        player.SetHeight(player.JumpHeight);
        player.SetHeightVelocity(player.JumpHeight);
    }

    public override void Tick()
    {
        CheckForBall();
        if (player.Height <= 0)
        {
            machine.ChangeState(new RecoveringState(player, machine));
        }
    }
    
    
    private void CheckForBall()
    {
        if (ballDetectionArea == null) return;
        
        int count = Physics2D.OverlapCollider(ballDetectionArea, ballFilter, overlapResults);

        if (count == 0) return;
        
        Collider2D ballCollider = overlapResults[0];
        Ball ball = ballCollider.GetComponentInParent<Ball>();
        Debug.Log($"found {ballCollider.gameObject.name}!");
        if (ball != null)
        {
            if (ball.Machine.CurrentState.CanAirInteract())
            {
                Debug.Log("Should hit ball here!");
                Vector2 lookDirection = player.Flipped ? Vector2.left : Vector2.right;
                ball.Shoot(lookDirection,player.Power);
            }
        }
    }
}
