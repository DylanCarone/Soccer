using System;
using UnityEngine;

public class AnimationBridge : MonoBehaviour
{
    private Player player;


    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }


    public void OnAnimationFinishedTrigger()
    {
        if (player != null)
        {
            player.OnAnimationFinishedTrigger();
        }
    }
}
