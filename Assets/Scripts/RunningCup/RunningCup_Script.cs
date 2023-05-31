using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningCup_Script : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float detectionDistance = 1f;
    public float directionChangeDelay;
    private Rigidbody rb;
    private float timeSinceDirectionChange = 0f;
    public bool isJumping = false;
    public float jumpScale;
    public float gravity = -9.8f;
    public Transform rayCastOrigin;
    public float time;

    private RunningCupGameController gameController;

    private void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<RunningCupGameController>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Get the current velocity of the object
        Vector3 currentVelocity = rb.velocity;

        // Calculate the desired velocity of the object based on the moveSpeed
        Vector3 targetVelocity = transform.forward * moveSpeed;

        // Add a force to the Rigidbody to move the object towards the targetVelocity
        Vector3 velocityChange = targetVelocity - currentVelocity;
        if(!isJumping) rb.AddForce(velocityChange, ForceMode.Acceleration);
        //if (rb.velocity.y == 0) isJumping = false;

        // Rotate the creature if it's time to change direction
        timeSinceDirectionChange += Time.deltaTime;
        if (timeSinceDirectionChange >= directionChangeDelay)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, Random.Range(-180f, 180f), 0f);
            StartCoroutine(RotateCreatureSmoothly(targetRotation, 1.0f));
            timeSinceDirectionChange = 0f;
        }

        /// Cast the ray from the middle of the object
        RaycastHit hitInfo;
        Vector3 raycastStartPos = rayCastOrigin.position;
        if (Physics.Raycast(raycastStartPos, transform.forward, out hitInfo, detectionDistance))
        {
            Debug.DrawLine(raycastStartPos, hitInfo.point, Color.red);
            if (hitInfo.collider.tag == "Obstacle" && !isJumping)
            {
                isJumping = true;
                GameObject targetObject = hitInfo.collider.gameObject;
                JumpOnObject2(targetObject);
            }
            if (hitInfo.collider.tag == "Wall" && timeSinceDirectionChange >= directionChangeDelay)
            {
                Quaternion targetRotation = Quaternion.Euler(0f, Random.Range(120, 250f), 0f);
                StartCoroutine(RotateCreatureSmoothly(targetRotation, 1.0f));
                timeSinceDirectionChange = 0f;
            }
        }

        // Apply gravity to the object
        //rb.AddForce(Physics.gravity, ForceMode.Acceleration);
    }

    public void JumpOnObject(GameObject targetObject)
    {
        // Calculate the jump force needed to land on top of the object
        float height = targetObject.transform.position.y + (targetObject.GetComponent<Collider>().bounds.size.y/2) - transform.position.y;
        print(targetObject.GetComponent<Collider>().bounds.size.y);
        float jumpForce = Mathf.Sqrt(2 * height * -gravity) * rb.mass;
        float jumpTime = jumpForce / -rb.mass / gravity;
        float horizontalVelocity = detectionDistance / jumpTime;

        // Apply the calculated force to jump on top of the object
        Vector3 jumpDirection = (targetObject.GetComponent<Collider>().bounds.size - transform.position).normalized;
        jumpDirection.y = Mathf.Sqrt(height * -2 * gravity)*jumpScale;
        rb.velocity = jumpDirection * horizontalVelocity;
    }

    public void JumpOnObject2(GameObject targetObject)
    {
        Debug.Log("Jump");
        // Calculate the jump force needed to land on top of the object
        float height = targetObject.transform.position.y + (targetObject.GetComponent<Collider>().bounds.size.y / 2) - transform.position.y;
        float verticalVel = height - ((gravity * (time*time)) / 2);
        float horizontalVel = detectionDistance / time;

        Debug.Log("height" + height + " vVel" + verticalVel + " hvel" + horizontalVel);

        rb.velocity = Vector3.zero;
        rb.AddForce((transform.forward * horizontalVel) + (transform.up * verticalVel), ForceMode.Impulse);

    }


    IEnumerator RotateCreatureSmoothly(Quaternion targetRotation, float duration)
    {
        Quaternion initialRotation = transform.rotation;
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle")) isJumping = false;
        else if (collision.gameObject.CompareTag("Ball"))
        {
            //gameController.AddPoint();
            Destroy(this.gameObject);
        }
        else if(collision.gameObject.CompareTag("Wall"))
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y+180, 0);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle")) isJumping = true;
    }

}




