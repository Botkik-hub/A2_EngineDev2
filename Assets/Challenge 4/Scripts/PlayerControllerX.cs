using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Challenge_4.Scripts
{
    public class PlayerControllerX : MonoBehaviour
    {
        private Rigidbody playerRb;
        private float speed = 500;
        private GameObject focalPoint;

        public bool hasPowerup;
        public GameObject powerupIndicator;
        public int powerUpDuration = 5;

        public float turboBoost = 10f;
        
        public ParticleSystem boostParticle;
        
        private float normalStrength = 10; // how hard to hit enemy without powerup
        private float powerupStrength = 25; // how hard to hit enemy with powerup

        private PlayerInput playerInput;
        private InputAction truboAction;
        
        private float forwardInput;
        
        
        void Start()
        {
            playerRb = GetComponent<Rigidbody>();
            focalPoint = GameObject.Find("Focal Point");
            playerInput = GetComponent<PlayerInput>();
            truboAction = playerInput.actions["TurboBoost"];
            
            
            playerInput.actions["MoveForward"].performed += context => forwardInput = context.ReadValue<float>();
            playerInput.actions["MoveForward"].canceled += context => forwardInput = 0.0f;
            truboAction.performed += TurboBoost;
        }

        private void OnDestroy()
        {
            playerInput.actions["MoveForward"].performed -= context => forwardInput = context.ReadValue<float>();
            playerInput.actions["MoveForward"].canceled -= context => forwardInput = 0.0f;
            truboAction.performed -= TurboBoost;
        }
        
        void Update()
        {
            // Add force to player in direction of the focal point (and camera)
            playerRb.AddForce(focalPoint.transform.forward * forwardInput * speed * Time.deltaTime);

            // Set powerup indicator position to beneath player
            powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);
        }
        
        private void TurboBoost(InputAction.CallbackContext obj)
        {
            playerRb.AddForce(focalPoint.transform.forward * turboBoost, ForceMode.Impulse);
            boostParticle.Play();
        }

        // If Player collides with powerup, activate powerup
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Powerup"))
            {
                Destroy(other.gameObject);
                hasPowerup = true;
                powerupIndicator.SetActive(true);
                StartCoroutine(PowerupCooldown());
            }
        }

        // Coroutine to count down powerup duration
        IEnumerator PowerupCooldown()
        {
            yield return new WaitForSeconds(powerUpDuration);
            hasPowerup = false;
            powerupIndicator.SetActive(false);
        }

        // If Player collides with enemy
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
                Vector3 awayFromPlayer =  other.gameObject.transform.position - transform.position;

                if (hasPowerup) // if have powerup hit enemy with powerup force
                {
                    enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
                }
                else // if no powerup, hit enemy with normal strength 
                {
                    enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
                }
            }
        }
    }
}
