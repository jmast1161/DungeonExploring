using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 2f;
    
    [SerializeField]
    private Rigidbody2D rigidBody;
    
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private AudioSource walkingAudioSource;

    private Vector2 screenBounds;
    private Vector2 moveDirection;
    private float objectWidth;
    private float objectHeight;
    public float hf = 0.0f;
    public float vf = 0.0f;

    private Vector2 velocity = new Vector2(0, 0);

    public event Action<Player> SpikeHit;
    public event Action<Player> DoorHit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2 - 1.5f;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2 - 1.5f;
        rigidBody.freezeRotation = true;
    }

    public void Initialize()
    {
        moveDirection = new Vector2(0, 0);
        transform.position = new Vector2(0, 0);
        walkingAudioSource.Stop();

    }

    private void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        rigidBody.angularVelocity = 0;
    }

    public void MovePlayer()
    {
        var moveX = Input.GetAxisRaw("Horizontal");
        var moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;

        if (moveDirection.x == 0 && moveDirection.y == 0)
        {
            walkingAudioSource.Stop();
        }
        else if (!walkingAudioSource.isPlaying)
        {
            walkingAudioSource.Play();
        }

        hf = moveX > 0.01f ? moveX : moveX < -0.01f ? 1 : 0;
        vf = moveY > 0.01f ? moveY : moveY < -0.01f ? 1 : 0;

        animator.SetBool("Left", moveX < -0.01f);
        animator.SetBool("Right", moveX > 0.01f);
        animator.SetFloat("Vertical", moveY);
        animator.SetFloat("Speed", vf);
    }

    private void StopAnimation()
    {
        animator.SetBool("Left", false);
        animator.SetBool("Right", false);
        animator.SetFloat("Vertical", 0);
        animator.SetFloat("Speed", 0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("collide");
        if (other.gameObject.tag == "Spikes")
        {
            StopAnimation();
            SpikeHit?.Invoke(this);
        }

        if (other.gameObject.tag == "Door")
        {
            Debug.Log("door");
            StopAnimation();
            DoorHit?.Invoke(this);
        }
    }
}