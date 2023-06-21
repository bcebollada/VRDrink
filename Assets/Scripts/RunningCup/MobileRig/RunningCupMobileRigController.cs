using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class RunningCupMobileRigController : MonoBehaviour
{
    private Rigidbody rb;
    public FixedJoystick joystick;

    public float moveSpeed, jumpForce, rotSpeed;
    private Transform deadTransform;

    public GameObject cup, scoreBoard;

    private RunningCupGameController gameController;

    private int playerNumber;
    private int initialPoint; //to check if was hit;

    private MacroGameController macroGameController;

    public bool isOnGround;

    private bool wasHit;

    private Realtime.InstantiateOptions instantiateOptions = new Realtime.InstantiateOptions();
    private Realtime realtimeInstance;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        realtimeInstance = GameObject.FindGameObjectWithTag("Room").GetComponent<Realtime>();

        instantiateOptions.ownedByClient = true;
        instantiateOptions.useInstance = realtimeInstance;

        deadTransform = GameObject.FindGameObjectWithTag("Finish").transform;
        macroGameController = GameObject.FindGameObjectWithTag("MacroGameController").GetComponent<MacroGameController>();

        if (GameObject.FindGameObjectWithTag("PlayerNumber") != null)
        {
            playerNumber = GameObject.FindGameObjectWithTag("PlayerNumber").GetComponent<MobileRigPlayerNumber>().playerNumber;
        }
        else playerNumber = 1;
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
        if (rb.isKinematic && !wasHit) //turned kinnematic because was hit
        {
            print("hit");

            gameController.AddPoint(playerNumber);
            if (playerNumber == 1) macroGameController.AddShotsLocalGame(0, 1, 0, 0);
            else if (playerNumber == 2) macroGameController.AddShotsLocalGame(0, 0, 1, 0);
            else if (playerNumber == 3) macroGameController.AddShotsLocalGame(0, 0, 0, 1);

            wasHit = true;
        }
        else Debug.Log("no hit");
    }

    private void FixedUpdate()
    {
        // Store the current velocity in a variable
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = transform.forward * joystick.Vertical * moveSpeed;
        Vector3 velocityChange = targetVelocity - currentVelocity;
        rb.AddForce(velocityChange, ForceMode.Force);


        // Rotate the GameObject based on joystick input
        transform.Rotate(new Vector3(0, joystick.Horizontal * rotSpeed, 0));
    }

    public void Jump()
    {
        if (!isOnGround || rb.velocity.y > 0.1) return;
        rb.AddForce(transform.up * jumpForce);
        isOnGround = false;

        GetComponent<AudioSource>().Play();
    }

    public void Hit() //need to run this when ball hits the cup
    {
        rb.isKinematic = true;

        var smoke = Realtime.Instantiate("Thick Smoke Variant", transform.position, Quaternion.identity, instantiateOptions);
        StartCoroutine(DestroyRealtimeObject(smoke, 3));

        transform.position = deadTransform.position;
        transform.rotation = deadTransform.rotation;

        //gameController.AddPoint(playerNumber);

        cup.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Cup hitted");
        if (collision.gameObject.CompareTag("Ball"))
        {
            Debug.Log("Cup hitted by ball");

            //Hit();
        }

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle"))
        {
            if (rb.velocity.y < 0.01) isOnGround = true;
            else isOnGround = false;
        }
    }

    private IEnumerator DestroyRealtimeObject(GameObject objectToDestroy, float secondsToDestroy)
    {
        yield return new WaitForSeconds(secondsToDestroy);
        Realtime.Destroy(objectToDestroy);
    }

    public void ShowScoreboard()
    {
        scoreBoard.SetActive(true);
    }
}
