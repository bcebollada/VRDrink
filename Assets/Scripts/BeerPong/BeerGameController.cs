using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class BeerGameController : MonoBehaviour
{
    //Tables and ball Prefabs
    [SerializeField] private GameObject staticTable, movingTable, lastTable, currentTable, ballPrefab, ball, startStand, smokeEffect;
    [SerializeField] private Transform tableSpawner, ballSpawner;
    private int tableLevel;

    public string playerName;
    public int points;

    public TMP_Text pointText, timerText;

    public float timeLeft = 30.0f;  // Set the timer duration in seconds
    public bool timerRunning = false;

    private void Awake()
    {

    }

    void Update()
    {
        if (timerRunning)
        {
            timeLeft -= Time.deltaTime;  // Reduce the timer by the time that has passed since last frame
            timerText.text = Mathf.Round(timeLeft).ToString();
            if (timeLeft <= 0)
            {
                TimerComplete();  // Call the TimerComplete function when the time is up
                timerRunning = false;
            }
        }

        if (timeLeft < 15 && timeLeft > 5 && tableLevel == 0)
        {
            tableLevel = 1;
            Debug.Log("Static destroyed, moving spawned");
            var smoke = Instantiate(smokeEffect, currentTable.transform.position, Quaternion.identity);
            Destroy(smoke, 3);
            Destroy(currentTable); //removes first table
            currentTable = Instantiate(movingTable, tableSpawner.position, Quaternion.identity); //creates second table
        }
        else if (timeLeft < 5 && tableLevel == 1)
        {
            tableLevel = 2;
            Debug.Log("moving destroyed, last spawned");
            var smoke = Instantiate(smokeEffect, currentTable.transform.position, Quaternion.identity);
            Destroy(smoke, 3);
            Destroy(currentTable); //removes first table
            currentTable = Instantiate(lastTable, tableSpawner.position, Quaternion.identity); //creates third table
        }
    }

   

    void TimerComplete()
    {
        // Code to execute when the timer is complete
        timerText.text = "Finish!";
        Debug.Log("Timer complete!");
    }

    // Call this function to start the timer
    public void StartGame()
    {
        var smoke = Instantiate(smokeEffect, startStand.transform.position, Quaternion.identity);
        Destroy(smoke, 3);
        Destroy(startStand);
        timerRunning = true;
        SpawnBall();
        currentTable = Instantiate(staticTable, tableSpawner.position, Quaternion.identity);
        var smoke2 = Instantiate(smokeEffect, currentTable.transform.position, Quaternion.identity);
        Destroy(smoke2, 3);
    }

    public void MiniGameEnd()
    {

    }

    public void AddPoint(int pointsToAdd)
    {
        points += pointsToAdd;
        pointText.text = points.ToString();
    }

    public void SpawnBall()
    {
        if (ball != null) return;
        ball = Instantiate(ballPrefab, ballSpawner.position, Quaternion.identity);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("FlipCup_Scene");
    }
}
