using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class RunningCupGameController : MonoBehaviour
{
    [SerializeField] private GameObject cup, startStand, smokeEffect, scoreBoard, pistol;
    public Vector3 ballSpawnCenter;
    public Vector3 ballSpawnSize;

    public int ballNumbers;

    public string playerName;
    public int points;
    public TMP_Text pointsText, startCountDownText, timerText;

    public float timeLeft = 60f;  // Set the timer duration in seconds
    private float countDown = 5f;
    public bool timerRunning, gameStartCountdown, isDebugMode;

    private MacroGameController macroGameController;

    private Vector3 center;

    public GameObject[] obstacles = new GameObject[14];


    private void Awake()
    {
        if(!isDebugMode) macroGameController = GameObject.Find("MacroGameController").GetComponent<MacroGameController>();
        center = startStand.transform.position;

    }

    private void Start()
    {
        playerName = macroGameController.playerPlaying;
    }


    // Update is called once per frame
    void Update()
    {
        pointsText.text = points.ToString();

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
    }

    public void GameStart()
    {
        Instantiate(pistol, startStand.transform.position, Quaternion.identity);
        gameStartCountdown = true;
        var smoke = Instantiate(smokeEffect, startStand.transform.position, Quaternion.identity);
        Destroy(smoke, 3);
        Destroy(startStand);
        SpawnCups();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(ballSpawnCenter, ballSpawnSize);
    }

    public void AddPoint()
    {
        if(timerRunning) points += 1;
    }

    void TimerComplete()
    {
        if (playerName == "Player1") macroGameController.playerShots[0] += points;
        else if (playerName == "Player2") macroGameController.playerShots[1] += points;
        else if (playerName == "Player3") macroGameController.playerShots[2] += points;
        else if (playerName == "Player4") macroGameController.playerShots[3] += points;
        timerRunning = false;

        // Code to execute when the timer is complete
        timerText.text = "Finish!";
        Debug.Log("Timer complete!");

        foreach(GameObject obstacle in obstacles)
        {
            var smokeObstacle = Instantiate(smokeEffect, obstacle.transform.position, Quaternion.identity);
            Destroy(smokeObstacle, 3);
            Destroy(obstacle);
        }

        var smoke = Instantiate(smokeEffect, center, Quaternion.identity);
        Destroy(smoke, 3);
        Instantiate(scoreBoard, center, Quaternion.Euler(0, -90f, 0));
    }

    private void SpawnCups()
    {
        //spawns randomly the cups
        for (int i = 0; i < ballNumbers; i++)
        {
            var spawnArea = ballSpawnCenter + new Vector3(Random.Range(-ballSpawnSize.x / 2, ballSpawnSize.x / 2), 0, Random.Range(-ballSpawnSize.z / 2, ballSpawnSize.z / 2));
            Instantiate(cup, spawnArea, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
        }
        
    }
}
