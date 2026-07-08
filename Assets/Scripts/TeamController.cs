using System;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    [SerializeField] private Ball ball;
    [SerializeField] private Player[] teamPlayers;
    [SerializeField] private PlayerInputProvider humanInput;

    private Player controlledPlayer;

    private void Awake()
    {
        ball.OnPossesionChanged += HandlePossessionChanged;
        PassingState.OnPlayerPass += HandlePossessionChanged;
    }

    private void OnDisable()
    {
        ball.OnPossesionChanged -= HandlePossessionChanged;
        PassingState.OnPlayerPass -= HandlePossessionChanged;
    }

    private void Start()
    {
        if (teamPlayers.Length > 0)
        {
            SwitchControlTo(teamPlayers[0]);
        }
    }

    private void Update()
    {
        if (controlledPlayer.InputProvider.GetPassPressedThisFrame() && !controlledPlayer.HasBall)
        {
            bool teammateHasBall = Array.Exists(teamPlayers, p => p.HasBall);
            if (!teammateHasBall)
            {
                SwitchControlToClosestToBall();
            }
        }
    }

    private void SwitchControlToClosestToBall()
    {
        Player closest = null;
        float closestDistance = float.MaxValue;

        foreach (var player in teamPlayers)
        {
            float dist = Vector3.Distance(player.transform.position, ball.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closest = player;
            }
        }

        if (closest != null)
        {
            SwitchControlTo(closest);
        }
    }

    private void HandlePossessionChanged(Player newCarrier)
    {
        if (newCarrier != null && Array.IndexOf(teamPlayers, newCarrier) >= 0)
        {
            SwitchControlTo(newCarrier);
        }
    }
    
    

    private void SwitchControlTo(Player newPlayer)
    {
        controlledPlayer?.SetInputProvider(null);
        controlledPlayer?.ToggleIcon(false);
        controlledPlayer = newPlayer;
        controlledPlayer?.ToggleIcon(true);
        controlledPlayer?.SetInputProvider(humanInput);
    }

}
