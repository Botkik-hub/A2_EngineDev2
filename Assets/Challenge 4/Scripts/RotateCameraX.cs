using UnityEngine;
using UnityEngine.InputSystem;

namespace Challenge_4.Scripts
{
    public class RotateCameraX : MonoBehaviour
    {
        private float speed = 200;
        public GameObject player;

        private PlayerInput playerInput;
        private float rotationInput;
        
        private void Start()
        {
            playerInput = GetComponent<PlayerInput>();
            playerInput.actions["RotateCamera"].performed += context => rotationInput = context.ReadValue<float>();
            playerInput.actions["RotateCamera"].canceled += context => rotationInput = 0.0f;
        }

        private void OnDestroy()
        {
            playerInput.actions["RotateCamera"].performed -= context => rotationInput = context.ReadValue<float>();
            playerInput.actions["RotateCamera"].canceled -= context => rotationInput = 0.0f;
        }

        // Update is called once per frame
        private void Update()
        {
            transform.Rotate(Vector3.up, rotationInput * speed * Time.deltaTime);
            transform.position = player.transform.position; // Move focal point with player
        }
    }
}