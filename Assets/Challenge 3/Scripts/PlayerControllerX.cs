using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;

    public PlayerInput playerInput;
    public InputAction flyUpAction;
    
    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip bounceSound;


    private bool _isFlyingUp;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        Physics.gravity *= gravityModifier;
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        playerInput = GetComponent<PlayerInput>();
        
        flyUpAction = playerInput.actions["FlyUp"];
        flyUpAction.performed += FlyUpStart;
        flyUpAction.canceled += FlyUpEnd;
        
        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);
    }

    private void OnDestroy()
    {
        flyUpAction.performed -= FlyUpStart;
        flyUpAction.canceled -= FlyUpEnd;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_isFlyingUp && !gameOver)
        {
            playerRb.AddForce(Vector3.up * floatForce * Time.deltaTime);
        }
        
        if (transform.position.y > 14.5f)
        {
            transform.position = new Vector3(transform.position.x, 14.5f, transform.position.z);
            if (playerRb.velocity.y > 0)
            {
                playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z);
            }
        }
    }

    private void FlyUpStart(InputAction.CallbackContext context)
    {
        _isFlyingUp = true;
    }

    private void FlyUpEnd(InputAction.CallbackContext context)
    {
        _isFlyingUp = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }
        
        else if (other.gameObject.CompareTag("Ground") && !gameOver)
        {
            playerAudio.PlayOneShot(bounceSound, 1.0f);
            playerRb.AddForce(Vector3.up * 10, ForceMode.Impulse);
        }
    }

}
