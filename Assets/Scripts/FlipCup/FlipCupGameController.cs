using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlipCupGameController : MonoBehaviour
{
    [SerializeField] private GameObject startStand;

    public string playerName;
    public int points;

    public TMP_Text pointText, timerText;

    public float timeLeft = 30.0f;  // Set the timer duration in seconds
    public bool timerRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
        Destroy(startStand);
        timerRunning = true;
    }

    public void AddPoint(int pointsToAdd)
    {
        points += pointsToAdd;
        pointText.text = points.ToString();
    }
}
