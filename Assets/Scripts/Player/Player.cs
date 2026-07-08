using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject playerIcon;
    [SerializeField] private Ball ball;
    [SerializeField] Collider2D ballDetectionArea;

    [Header("Settings")] 
    [SerializeField] private float speed;
    [Header("Tackle Settings")]
    [SerializeField] private float tackleDuration = 0.5f;
    [SerializeField] private float recoverDuration = 0.2f;
    [Header("Kicking Settings")]
    [SerializeField] private float kickPower = 1f;
    
    [Header("Passing Settings")]
    [SerializeField] private float passPower = 1f;
    [SerializeField] [Range(0,1)] private float closePassMultiplier = 0.8f;
    [SerializeField] private float maxPassSearchRadius = 10f;
    [SerializeField] private float maxPassLineDeviation = 1.5f;

    [Header("Header Settings")] 
    [SerializeField] private float jumpHeight = 0f;

    [SerializeField] private float gravity = 5f;

    private float height;
    private float heightVelocity;
    
    public float JumpHeight => jumpHeight;
    public float Height => height;
    

    public Collider2D BallDetectionArea => ballDetectionArea;

    
    private bool hasBall;
    private IInputProvider inputProvider = new  NullInputProvider();
    private PlayerStateMachine machine;
    private Rigidbody2D rb;
    
    public IInputProvider InputProvider => inputProvider;
    public float Speed => speed;
    public float TackleDuration => tackleDuration;
    public float RecoverDuration => recoverDuration;
    public Rigidbody2D Rigidbody => rb;
    public float Power => kickPower;
    public float PassPower => passPower;
    public float ClosePassMultiplier => closePassMultiplier;
    
    public bool Flipped => spriteRenderer.flipX;
    public bool HasBall => hasBall;
    public Ball Ball => ball;

    public void SetInputProvider(IInputProvider provider)
    {
        inputProvider = provider ?? new NullInputProvider();
    }

    public void ToggleIcon(bool toggle)
    {
        playerIcon?.SetActive(toggle);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        machine = new PlayerStateMachine();
        // inputProvider = GetComponent<IInputProvider>();
    }

    private void Start()
    {
        machine.Initialize(new RunningState(this, machine));
    }

    // Update is called once per frame
    void Update()
    {
        machine.Tick();
        ProcessGravity();
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
    public void SetHeight(float height) =>  this.height = height;
    public void SetHeightVelocity(float heightVelocity) =>  this.heightVelocity = heightVelocity;

    void ProcessGravity()
    {
        if (height > 0)
        {
            heightVelocity -= gravity * Time.deltaTime;
            height += heightVelocity;
            if (height <= 0)
            {
                height = 0;
            }

            spriteRenderer.transform.localPosition = Vector2.up * height;
        }
    }
}
