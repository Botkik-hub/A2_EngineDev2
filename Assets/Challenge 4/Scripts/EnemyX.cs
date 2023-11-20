using UnityEngine;

namespace Challenge_4.Scripts
{
    public class EnemyX : MonoBehaviour
    {
        public float speed;
        private Rigidbody enemyRb;
        private GameObject playerGoal;

        // Start is called before the first frame update
        private void Start()
        {
            enemyRb = GetComponent<Rigidbody>();
            playerGoal = GameObject.FindWithTag("PlayerGoal");
        }

        // Update is called once per frame
        private void Update()
        {
            // Set enemy direction towards player goal and move there
            Vector3 lookDirection = (playerGoal.transform.position - transform.position).normalized;
            enemyRb.AddForce(lookDirection * speed * Time.deltaTime);

        }

        private void OnCollisionEnter(Collision other)
        {
            // If enemy collides with either goal, destroy it
            if (other.gameObject.name == "Enemy Goal")
            {
                Destroy(gameObject);
            }
            else if (other.gameObject.name == "Player Goal")
            {
                Destroy(gameObject);
            }

        }

    }
}
