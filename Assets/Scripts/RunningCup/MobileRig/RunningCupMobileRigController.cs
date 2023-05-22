using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningCupMobileRigController : MonoBehaviour
{
    private Rigidbody rb;
    public FixedJoystick joystick;

    public float moveSpeed, jumpForce, rotSpeed;
    private Transform deadTransform;

    public GameObject cup;

    private RunningCupGameController gameController;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        deadTransform = GameObject.Find("MobileRigDeadPos").transform;
    }

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<RunningCupGameController>();

        Vector3 spawnCenter = gameController.cupSpawnCenter;
        Vector3 spawnSize = gameController.cupSpawnSize;

        //put movbile rig in random position on scene
        transform.position = spawnCenter + new Vector3(Random.Range(-spawnSize.x / 2, spawnSize.x / 2), 0, Random.Range(-spawnSize.z / 2, spawnSize.z / 2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // Store the current velocity in a variable
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = transform.forward * joystick.Vertical * moveSpeed;
        Vector3 velocityChange = targetVelocity - currentVelocity;
        rb.AddForce(velocityChange, ForceMode.Acceleration);


        // Rotate the GameObject based on joystick input
        transform.Rotate(new Vector3(0, joystick.Horizontal * rotSpeed, 0));
    }

    public void Jump()
    {
        rb.AddForce(transform.up * jumpForce);
    }

    public void Hit() //need to run this when ball hits the cup
    {
        rb.isKinematic = true;
        transform.position = deadTransform.position;
        transform.rotation = deadTransform.rotation;

        cup.SetActive(false);

        gameController.AddPoint();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Cup hitted");
        if (collision.gameObject.CompareTag("Ball"))
        {
            Debug.Log("Cup hitted by ball");

            Hit();
        }
    }
}
