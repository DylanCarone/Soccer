using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] private Ball ball;

    [Header("Settings")] 
    [SerializeField] private float speed;
    [SerializeField] private float tackleDuration = 0.5f;
    [SerializeField] private float recoverDuration = 0.2f;
    [SerializeField] private float kickPower = 1f;


    private bool hasBall = false;
    private IInputProvider inputProvider;
    private PlayerStateMachine machine;
    private Rigidbody2D rb;
    
    public IInputProvider InputProvider => inputProvider;
    public float Speed => speed;
    public float TackleDuration => tackleDuration;
    public float RecoverDuration => recoverDuration;
    public Rigidbody2D Rigidbody => rb;
    public float Power => kickPower;
    
    public bool Flipped => spriteRenderer.flipX;
    public bool HasBall => hasBall;
    public Ball Ball => ball;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputProvider = GetComponent<IInputProvider>();
        machine = new PlayerStateMachine();
    }

    private void Start()
    {
        machine.Initialize(new RunningState(this, machine));
    }

    // Update is called once per frame
    void Update()
    {
        machine.Tick();
        
    }

    public void Move(Vector2 direction)
    {
        rb.linearVelocity = direction * speed;
        FlipPlayer(direction);
    }

    public void FlipPlayer(Vector2 direction)
    {
        if(direction.x > 0){
            spriteRenderer.flipX = false;
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    public void Stop()
    {
        rb.linearVelocity = Vector2.zero;
    }


    public void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }
    
    public void SetBall(bool value) => hasBall = value;

    public void OnAnimationFinishedTrigger()
    {
        machine.CurrentState.OnAnimationFinished();
    }
}
