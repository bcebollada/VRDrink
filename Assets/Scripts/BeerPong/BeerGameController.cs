using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Normal.Realtime;


public class BeerGameController : MonoBehaviour
{
    //Tables and ball Prefabs
    [SerializeField] private GameObject staticTable, movingTable, lastTable, currentTable, ballPrefab, ball, startStand, smokeEffect, scoreBoard;
    [SerializeField] private Transform tableSpawner, ballSpawner;
    private int tableLevel;

    public string playerName;
    public int points;
    public int pointsGoal; //player will need to this amout of points

    public TMP_Text pointText, timerText, startCountDownText;

    public float timeLeft = 30.0f;  // Set the timer duration in seconds
    private float countDown = 5f;
    public bool timerRunning, gameStartCountdown;

    private MacroGameController macroGameController;

    public bool isDebugMode;

    private Realtime.InstantiateOptions instantiateOptions = new Realtime.InstantiateOptions();


    private void Awake()
    {
        instantiateOptions.ownedByClient = true;

        if (!isDebugMode) macroGameController = GameObject.Find("MacroGameController").GetComponent<MacroGameController>();
    }

    private void Start()
    {
        playerName = macroGameController.playerPlaying;
    }

    void Update()
    {
        if(gameStartCountdown)
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

        if (SystemInfo.deviceModel.Contains("Quest 2") || SystemInfo.deviceModel.Contains("Raider") || Application.platform == RuntimePlatform.WindowsEditor)
            UpdateTablesMainClient(); //if is in the headset, create tables

    }


    private void UpdateTablesMainClient()
    {
        if (timeLeft < 15 && timeLeft > 5 && tableLevel == 0)
        {
            tableLevel = 1;
            Debug.Log("Static destroyed, moving spawned");
            var smoke = Realtime.Instantiate("Thick Smoke Variant", currentTable.transform.position, Quaternion.identity, instantiateOptions);
            StartCoroutine(DestroyRealtimeObject(smoke, 3));

            Realtime.Destroy(currentTable); //removes first table
            currentTable = Realtime.Instantiate("TableCupsMoving", tableSpawner.position, Quaternion.identity, instantiateOptions); //creates second table
        }
        else if (timeLeft < 5 && tableLevel == 1)
        {
            tableLevel = 2;
            Debug.Log("moving destroyed, last spawned");
            var smoke = Realtime.Instantiate("Thick Smoke Variant", currentTable.transform.position, Quaternion.identity, instantiateOptions);
            StartCoroutine(DestroyRealtimeObject(smoke, 3));

            Realtime.Destroy(currentTable); //removes first table

            currentTable = Realtime.Instantiate("TableCupSpecial", tableSpawner.position, Quaternion.identity, instantiateOptions); //creates third table
        }
    }
   

    void TimerComplete()
    {
        /*if (playerName == "Player1") macroGameController.playerShots[0] += points;
        else if (playerName == "Player2") macroGameController.playerShots[1] += points;
        if (playerName == "Player3") macroGameController.playerShots[2] += points;
        if (playerName == "Player4") macroGameController.playerShots[3] += points;*/

        if(pointsGoal-points == 0) //vr player won
        {
            macroGameController.AddShots(0, 1, 1, 1);
        }
        else //vr player lost
        {
            macroGameController.AddShots(1, 0, 0, 0);
        }

        // Code to execute when the timer is complete
        timerText.text = "Finish!";
        Debug.Log("Timer complete!");

        var smoke = Realtime.Instantiate("Thick Smoke Variant", currentTable.transform.position, Quaternion.identity, instantiateOptions);
        StartCoroutine(DestroyRealtimeObject(smoke, 3));

        Realtime.Instantiate("ScoreBoard", currentTable.transform.position, Quaternion.Euler(0, -90f, 0), instantiateOptions);
        //Instantiate(scoreBoard, currentTable.transform.position, Quaternion.Euler(0 , -90f, 0));
        Realtime.Destroy(currentTable); //removes last table
    }

    // Call this function to start the timer
    public void StartGame()
    {
        gameStartCountdown = true;

        var smoke = Realtime.Instantiate("Thick Smoke Variant", startStand.transform.position, Quaternion.identity, instantiateOptions);
        StartCoroutine(DestroyRealtimeObject(smoke, 3));

        Destroy(startStand);

        if (SystemInfo.deviceModel.Contains("Quest 2") || SystemInfo.deviceModel.Contains("Raider") || Application.platform == RuntimePlatform.WindowsEditor)
        {
            SpawnBall();
            currentTable = Realtime.Instantiate("TableCupsStatic", tableSpawner.position, Quaternion.identity, instantiateOptions);
        }

        var smoke2 = Realtime.Instantiate("Thick Smoke Variant", currentTable.transform.position, Quaternion.identity, instantiateOptions);
        StartCoroutine(DestroyRealtimeObject(smoke2, 3));

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
        if (ball != null) return;

        ball = Realtime.Instantiate("Ball", ballSpawner.position, Quaternion.identity, instantiateOptions);
        //ball = Instantiate(ballPrefab, ballSpawner.position, Quaternion.identity);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("FlipCup_Scene");
    }

    private IEnumerator DestroyRealtimeObject(GameObject objectToDestroy, float secondsToDestroy)
    {
        yield return new WaitForSeconds(secondsToDestroy);
        Realtime.Destroy(objectToDestroy);
    }
}
