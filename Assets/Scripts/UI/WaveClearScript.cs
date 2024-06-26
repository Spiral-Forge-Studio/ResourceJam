using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveClearScript : MonoBehaviour
{
    public float initialSpeed = 10f;          // Initial speed of the drop
    public float stopPositionY = 0f;          // Y position where the object stops in the middle
    public float stopDuration = 1f;           // Duration for which the object stops in the middle
    public float acceleration = 2f;           // Acceleration for the second drop
    public float bottomY = -10f;              // Y position where the object goes out of view
    public float resetDelay = 1f;             // Delay before resetting to the top
    public float slowDownDistance = 2f;       // Distance before the stop position to start slowing down

    private Rigidbody2D rb;
    private Vector2 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        StartCoroutine(DropRoutine());
    }

    private IEnumerator DropRoutine()
    {
        while (true)
        {
            // Initial drop
            rb.velocity = new Vector2(0, -initialSpeed);

            bool breakLoop = false;
            // Slow down before reaching the stop position
            while (transform.position.y > stopPositionY && breakLoop == false)
            {
                float distanceToStop = transform.position.y - stopPositionY;
                if (distanceToStop <= slowDownDistance)
                {
                    float slowFactor = distanceToStop / slowDownDistance;
                    rb.velocity = new Vector2(0, -initialSpeed * slowFactor);
                    
                }
                Debug.Log(rb.velocity.magnitude);
                if (rb.velocity.magnitude < 0.01f) breakLoop = true;

                yield return null;
            }

            // Slow down to a stop
            rb.velocity = Vector2.zero;

            // Wait for the stop duration
            yield return new WaitForSeconds(stopDuration);

            // Accelerate down again
            while (transform.position.y > bottomY)
            {
                rb.velocity += new Vector2(0, -acceleration * Time.fixedDeltaTime);
                yield return null;
            }

            // Hide and reset the position after going out of view
            rb.velocity = Vector2.zero;
            transform.position = startPosition;

            // Wait before starting the drop again
            yield return new WaitForSeconds(resetDelay);

            Destroy(gameObject);
        }
    }
}
