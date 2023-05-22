using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class RunningCupGameController : MonoBehaviour
{
    [SerializeField] private GameObject cup, startStand, smokeEffect, scoreBoard, pistol;
    public Vector3 cupSpawnCenter;
    public Vector3 cupSpawnSize;

    public int ballNumbers;

    public int pointsGoal;
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
        //SpawnCups();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(cupSpawnCenter, cupSpawnSize);
    }

    public void AddPoint()
    {
        if(timerRunning) points += 1;
    }

    void TimerComplete()
    {

        if (pointsGoal - points == 0) //vr player won
        {
            macroGameController.AddShots(0, 1, 1, 1);
        }
        else //vr player lost
        {
            macroGameController.AddShots(1, 0, 0, 0);
        }

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
            var spawnArea = cupSpawnCenter + new Vector3(Random.Range(-cupSpawnSize.x / 2, cupSpawnSize.x / 2), 0, Random.Range(-cupSpawnSize.z / 2, cupSpawnSize.z / 2));
            Instantiate(cup, spawnArea, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
        }
        
    }
}
