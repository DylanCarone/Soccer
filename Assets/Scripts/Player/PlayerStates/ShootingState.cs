using UnityEngine;
public class ShootingState : PlayerState
{
    private const string KICK_ANIMATION = "kick";

    private float shotPower;
    Vector2 shotDirection;
    public ShootingState(Player player, PlayerStateMachine machine, float power, Vector2 shotDirection) : base(player, machine)
    {
        shotPower = power;
        this.shotDirection = shotDirection;
    }

    public override void Enter()
    {
        player.PlayAnimation(KICK_ANIMATION);
    }

    public override void Tick()
    {
        
    }

    public override void OnAnimationFinished()
    {
        ShootBall();
        Debug.Log($"Power: {shotPower} -- Direction: {shotDirection}");
        machine.ChangeState(new RunningState(player, machine));
        
    }

    void ShootBall()
    {
        Debug.Log("Shoot!");
        player.Ball.Shoot(shotDirection, shotPower);
        player.SetBall(false);
    }
}
