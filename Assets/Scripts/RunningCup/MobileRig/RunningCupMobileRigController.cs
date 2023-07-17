using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using TMPro;

public class RunningCupMobileRigController : MonoBehaviour
{
    public Rigidbody rb;
    public FixedJoystick joystick;

    public float moveSpeed, jumpForce, rotSpeed;
    private Transform deadTransform;

    public GameObject cup, scoreBoard, instructions;
    public Camera cam;

    private RunningCupGameController gameController;

    public int playerNumber, initialPoint;

    public MacroGameController macroGameController;

    public bool isOnGround;

    private bool wasHit;

    private Realtime.InstantiateOptions instantiateOptions = new Realtime.InstantiateOptions();
    private Realtime realtimeInstance;

    public TMP_Text debug1, debug2, debug3;

    private PointsManager pointsManager;




    private void Awake()
    {
        realtimeInstance = GameObject.FindGameObjectWithTag("Room").GetComponent<Realtime>();

        instantiateOptions.ownedByClient = true;
        instantiateOptions.useInstance = realtimeInstance;

        deadTransform = GameObject.FindGameObjectWithTag("Finish").transform;
        macroGameController = GameObject.FindGameObjectWithTag("MacroGameController").GetComponent<MacroGameController>();


        if (GameObject.FindGameObjectWithTag("PlayerPoints") != null)
        {
            pointsManager = GameObject.FindGameObjectWithTag("PlayerPoints").GetComponent<PointsManager>();
        }

        if (GameObject.FindGameObjectWithTag("PlayerNumber") != null)
        {
            playerNumber = GameObject.FindGameObjectWithTag("PlayerNumber").GetComponent<MobileRigPlayerNumber>().playerNumber;
        }
        else playerNumber = 2;
    }

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<RunningCupGameController>();

        Vector3 spawnCenter = gameController.cupSpawnCenter;
        Vector3 spawnSize = gameController.cupSpawnSize;

        //put movbile rig in random position on scene
        transform.position = spawnCenter + new Vector3(Random.Range(-spawnSize.x / 2, spawnSize.x / 2), 0, Random.Range(-spawnSize.z / 2, spawnSize.z / 2));

        if(playerNumber == 2) initialPoint = pointsManager.player2Points;
        else if (playerNumber == 3) initialPoint = pointsManager.player3Points;
        else if (playerNumber == 4) initialPoint = pointsManager.player4Points;

    }

    // Update is called once per frame
    void Update()
    {

        debug2.text = macroGameController.pointsManager.player2Points.ToString();
        debug3.text = playerNumber.ToString();
        /*if(playerNumber == 2)
        {
            if(macroGameController.pointsManager.player2Points != initialPoint)
            {
                rb.isKinematic = true;

                var smoke = Realtime.Instantiate("Thick Smoke Variant", transform.position, Quaternion.identity, instantiateOptions);
                StartCoroutine(DestroyRealtimeObject(smoke, 3));

                transform.position = deadTransform.position;
                transform.rotation = deadTransform.rotation;

                gameController.AddPoint(playerNumber);

                cup.SetActive(false);

                initialPoint = macroGameController.pointsManager.player2Points;
            }
        }
        else if (playerNumber == 3)
        {
            if (macroGameController.pointsManager.player3Points != initialPoint)
            {
                rb.isKinematic = true;

                var smoke = Realtime.Instantiate("Thick Smoke Variant", transform.position, Quaternion.identity, instantiateOptions);
                StartCoroutine(DestroyRealtimeObject(smoke, 3));

                transform.position = deadTransform.position;
                transform.rotation = deadTransform.rotation;

                gameController.AddPoint(playerNumber);

                cup.SetActive(false);

                initialPoint = macroGameController.pointsManager.player3Points;
            }
        }
        else if (playerNumber == 4)
        {
            if (macroGameController.pointsManager.player4Points != initialPoint)
            {
                rb.isKinematic = true;

                var smoke = Realtime.Instantiate("Thick Smoke Variant", transform.position, Quaternion.identity, instantiateOptions);
                StartCoroutine(DestroyRealtimeObject(smoke, 3));

                transform.position = deadTransform.position;
                transform.rotation = deadTransform.rotation;

                gameController.AddPoint(playerNumber);

                cup.SetActive(false);

                initialPoint = macroGameController.pointsManager.player2Points;
            }
        }*/
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
        debug1.text = "hit1";

        rb.constraints = RigidbodyConstraints.FreezeAll;
        debug1.text = "hit2";

        transform.position = deadTransform.position;
        transform.rotation = deadTransform.rotation;

        //gameController.AddPoint(playerNumber);

        cup.SetActive(false);
        debug1.text = "hit3";

        ShowScoreboard();

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Cup hitted");
        if (collision.gameObject.CompareTag("Ball"))
        {
            Debug.Log("Cup hitted by ball");

            Hit();

            if (macroGameController.isMobileRig)
            {
                //Hit();

            }
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
        cam.cullingMask = ~cam.cullingMask;
        cam.cullingMask = (1 << 5); // Set the culling mask to block everything except the target layer

        rb.constraints = RigidbodyConstraints.FreezeAll;

        transform.position = deadTransform.position;
        transform.rotation = deadTransform.rotation;
    }

    public void GameStart()
    {
        instructions.SetActive(false);
        cam.cullingMask = int.MaxValue; //show everything
    }
}
