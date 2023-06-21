using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Normal.Realtime;


public class BeerGameController : MonoBehaviour
{
    //Tables and ball Prefabs
    [SerializeField] private GameObject staticTable, movingTable, lastTable, currentTable, ballPrefab, ball, startStand, smokeEffect, scoreBoard, pingPongTable;
    [SerializeField] private Transform tableSpawner, ballSpawner;
    [SerializeField] private Transform[] interceptorsMesh = new Transform[3];

    private int tableLevel;

    public string playerName;
    public int points;
    public int pointsGoal; //player will need to this amout of points

    public TMP_Text pointText, timerText, startCountDownText;

    public float timeLeft = 30.0f;  // Set the timer duration in seconds
    private float countDown = 5f;
    public bool timerRunning, gameStartCountdown;

    [SerializeField] private MacroGameController macroGameController;

    public bool isDebugMode;

    private Realtime.InstantiateOptions instantiateOptions = new Realtime.InstantiateOptions();
    private Realtime realtimeInstance;



    private void Awake()
    {
        realtimeInstance = GameObject.FindGameObjectWithTag("Room").GetComponent<Realtime>();

        instantiateOptions.ownedByClient = true;
        instantiateOptions.useInstance = realtimeInstance;

        if (!isDebugMode) macroGameController = GameObject.FindGameObjectWithTag("MacroGameController").GetComponent<MacroGameController>();
    }

    private void Start()
    {
        playerName = macroGameController.playerPlaying;

        foreach (var interceptor in interceptorsMesh)
        {
            interceptor.gameObject.GetComponent<MeshRenderer>().enabled = false;
            //interceptor.gameObject.SetActive(false);
        }

        //SetInterceptors();

    }

    void Update()
    {
        if (gameStartCountdown)
        {
            countDown -= Time.deltaTime; //reduce start countdown in seconds
            startCountDownText.text = Mathf.Round(countDown).ToString();
            if (countDown <= 0)
            {
                timerRunning = true;
                startCountDownText.text = "";
            }
        }

        if (timerRunning)
        {
            gameStartCountdown = false;
            timeLeft -= Time.deltaTime;  // Reduce the timer by the time that has passed since last frame
            timerText.text = Mathf.Round(timeLeft).ToString();
            if (timeLeft <= 0)
            {
                TimerComplete();  // Call the TimerComplete function when the time is up
                timerRunning = false;
            }
        }

        if (!macroGameController.isMobileRig)
            UpdateTablesMainClient(); //if is in the headset, create tables

        if (ball == null && timerRunning) SpawnBall();

    }


    private void UpdateTablesMainClient()
    {
        if (timeLeft < 15 && timeLeft > 5 && tableLevel == 0)
        {
            tableLevel = 1;
            Debug.Log("Static destroyed, moving spawned");
            var smoke = Realtime.Instantiate("Thick Smoke Variant", currentTable.transform.position, currentTable.transform.rotation, instantiateOptions);
            StartCoroutine(DestroyRealtimeObject(smoke, 3));

            Realtime.Destroy(currentTable); //removes first table
            currentTable = Realtime.Instantiate("TableCupsMoving", tableSpawner.position, tableSpawner.transform.rotation, instantiateOptions); //creates second table
        }
        else if (timeLeft < 5 && tableLevel == 1)
        {
            tableLevel = 2;
            Debug.Log("moving destroyed, last spawned");
            var smoke = Realtime.Instantiate("Thick Smoke Variant", currentTable.transform.position, currentTable.transform.rotation, instantiateOptions);
            StartCoroutine(DestroyRealtimeObject(smoke, 3));

            Realtime.Destroy(currentTable); //removes first table

            currentTable = Realtime.Instantiate("TableCupSpecial", tableSpawner.position, tableSpawner.rotation, instantiateOptions); //creates third table
        }
    }
   

    void TimerComplete()
    {
        interceptorsMesh[0].gameObject.SetActive(false);
        interceptorsMesh[1].gameObject.SetActive(false);
        interceptorsMesh[2].gameObject.SetActive(false);
        pingPongTable.SetActive(false);

        if (pointsGoal-points <= 0) //vr player won
        {
            macroGameController.AddShotsLocalGame(0, 1, 1, 1);
        }
        else //vr player lost
        {
            macroGameController.AddShotsLocalGame(1, 0, 0, 0);
        }

        // Code to execute when the timer is complete
        timerText.text = "Finish!";
        Debug.Log("Timer complete!");

        if (!macroGameController.isMobileRig) //if is not mobile
        {
            var smoke = Realtime.Instantiate("Thick Smoke Variant", currentTable.transform.position, currentTable.transform.rotation, instantiateOptions);
            StartCoroutine(DestroyRealtimeObject(smoke, 3));

            Realtime.Instantiate("ScoreBoard", currentTable.transform.position, Quaternion.Euler(0, 90f, 0), instantiateOptions);
            //Instantiate(scoreBoard, currentTable.transform.position, Quaternion.Euler(0 , -90f, 0));
            Realtime.Destroy(currentTable); //removes last table
        }
        else //is mobile
        {
            GameObject[] mobileRigs = GameObject.FindGameObjectsWithTag("MobileRigGameController");

            foreach (var mobiles in mobileRigs)
            {
                mobiles.GetComponent<MobileRigController>().ShowScoreboard();
                mobiles.transform.position += new Vector3(0, 5, 0);
            }
        }


    }

    // Call this function to start the timer
    public void StartGame()
    {
        SetInterceptors();

        //remove instructions
        GameObject[] instructions = GameObject.FindGameObjectsWithTag("Instructions");
        foreach (var instruction in instructions)
        {
            instruction.SetActive(false);
        }

        gameStartCountdown = true;  

        var smoke = Realtime.Instantiate("Thick Smoke Variant", startStand.transform.position, Quaternion.identity, instantiateOptions);
        StartCoroutine(DestroyRealtimeObject(smoke, 3));

        Destroy(startStand);

        if (!macroGameController.isMobileRig)
        {
            SpawnBall();
            currentTable = Realtime.Instantiate("TableCupsStatic", tableSpawner.position, tableSpawner.rotation, instantiateOptions);
        }

        var smoke2 = Realtime.Instantiate("Thick Smoke Variant", currentTable.transform.position, Quaternion.identity, instantiateOptions);
        StartCoroutine(DestroyRealtimeObject(smoke2, 3));

    }

    public void SetInterceptors()
    {

        if(macroGameController.playerNumbers == 1)
        {
            interceptorsMesh[0].gameObject.GetComponent<MeshRenderer>().enabled = true;

            //interceptorsMesh[1].transform.position += new Vector3(0, 200, 0); //add hight so it doesnt show. normcore doesnt let us destroy scene objects
            //interceptorsMesh[2].transform.position += new Vector3(0, 200, 0);
        }

        if (macroGameController.playerNumbers == 2) //2 mobile players
        {
            interceptorsMesh[0].gameObject.GetComponent<MeshRenderer>().enabled = true;
            interceptorsMesh[1].gameObject.GetComponent<MeshRenderer>().enabled = true;


            //interceptorsMesh[2].transform.position += new Vector3(0, 200, 0);

            interceptorsMesh[0].localScale = new Vector3(interceptorsMesh[0].localScale.x / 2, interceptorsMesh[0].localScale.y, interceptorsMesh[0].localScale.z);
            interceptorsMesh[0].localPosition += new Vector3(0.25f, 0, 0);

            interceptorsMesh[1].localScale = new Vector3(interceptorsMesh[1].localScale.x / 2, interceptorsMesh[1].localScale.y, interceptorsMesh[1].localScale.z);
            interceptorsMesh[1].localPosition -= new Vector3(0.25f, 0, 0);

        }
        else if (macroGameController.playerNumbers == 3) //3 mobile players
        {
            interceptorsMesh[0].gameObject.GetComponent<MeshRenderer>().enabled = true;
            interceptorsMesh[1].gameObject.GetComponent<MeshRenderer>().enabled = true;
            interceptorsMesh[2].gameObject.GetComponent<MeshRenderer>().enabled = true;


            interceptorsMesh[0].localScale = new Vector3(interceptorsMesh[0].localScale.x / 3, interceptorsMesh[0].localScale.y, interceptorsMesh[0].localScale.z);
            interceptorsMesh[0].localPosition += new Vector3(0.33f, 0, 0);

            interceptorsMesh[1].localScale = new Vector3(interceptorsMesh[1].localScale.x / 3, interceptorsMesh[1].localScale.y, interceptorsMesh[1].localScale.z);

            interceptorsMesh[2].localScale = new Vector3(interceptorsMesh[2].localScale.x / 3, interceptorsMesh[2].localScale.y, interceptorsMesh[2].localScale.z);
            interceptorsMesh[2].localPosition -= new Vector3(0.33f, 0, 0);
        }
    }

    public void MiniGameEnd()
    {
    }

    public void AddPoint(int pointsToAdd)
    {
        points += pointsToAdd;
        pointText.text = $"{points} /{pointsGoal}";
    }

    public void SpawnBall()
    {
        //if (ball != null) return;
        if (macroGameController.isMobileRig) return;

        ball = Realtime.Instantiate("Ball", ballSpawner.position, Quaternion.identity, instantiateOptions);
        //ball = Instantiate(ballPrefab, ballSpawner.position, Quaternion.identity);
    }

    private IEnumerator DestroyRealtimeObject(GameObject objectToDestroy, float secondsToDestroy)
    {
        yield return new WaitForSeconds(secondsToDestroy);
        Realtime.Destroy(objectToDestroy);
    }
}
