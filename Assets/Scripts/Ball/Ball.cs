using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D playerDetectionArea;
    [SerializeField] private GameObject trail;
    [SerializeField] private BallSettings settings;

    public BallSettings Settings => settings;
    
    private float height;
    private float heightVelocity;
    public float HeightVelocity => heightVelocity;
    
    
    private Transform playerTransform = null;

    
    private Rigidbody2D rb;
    public Rigidbody2D Rigidbody => rb;
    
    private BallStateMachine machine;
    
    public Transform PlayerTransform => playerTransform;
    public SpriteRenderer SpriteRenderer => spriteRenderer;
    public GameObject Trail => trail;
    
    public bool IsInAir => spriteRenderer.transform.localPosition.y > 0.01f;
    
   

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        machine = new BallStateMachine();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        machine.Initialize(new FreeformState(this, machine, playerDetectionArea, playerTransform));
    }

    // Update is called once per frame
    void Update()
    {
        machine.Tick();
    }

    public void PlayAnimation(string animationName)
    {
        animator.speed = 1;
        animator.Play(animationName);
    }

    public void PauseAnimation()
    {
        animator.speed = 0;    
    }

    public void Shoot(Vector2 shotVelocity, float power)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = shotVelocity * power;
        LaunchIntoAir(power * 0.6f);
        machine.ChangeState(new ShotState(this, machine,playerDetectionArea, playerTransform));
    }

    public void ApplyHeightGravity(float velocity)
    {
        heightVelocity -= velocity;
        height += heightVelocity * Time.deltaTime;

        if (height < 0f)
        {
            height = 0f;
        }
        
        height = height < 0f ? 0f : height;
        
        spriteRenderer.transform.localPosition = new Vector3(0f, height, 0f);

    }

    public void InvertVelocity(float bounciness)
    {
        heightVelocity = -heightVelocity * bounciness;
        height = 0.05f;
        spriteRenderer.transform.localPosition = new Vector3(0f, height, 0f);
    }

    public void ResetVerticalVelocity()
    {
        heightVelocity = 0f;
        height = 0f;
        spriteRenderer.transform.localPosition = new Vector3(0f, 0f, 0f);
    }
    public void LaunchIntoAir(float verticalForce)
    {
        heightVelocity = verticalForce;
        height = 0.05f;
    }
    
}
