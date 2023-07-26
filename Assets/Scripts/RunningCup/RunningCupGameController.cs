using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Normal.Realtime;


public class RunningCupGameController : MonoBehaviour
{
    [SerializeField] private GameObject cup, startStand, smokeEffect, scoreBoard, pistol, instruction;
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

    private Realtime.InstantiateOptions instantiateOptions = new Realtime.InstantiateOptions();
    private Realtime realtimeInstance;

    private PointsManager pointsManager;
    public int[] initialPointsArray;


    private void Awake()
    {
        realtimeInstance = GameObject.FindGameObjectWithTag("Room").GetComponent<Realtime>();

        instantiateOptions.ownedByClient = true;
        instantiateOptions.useInstance = realtimeInstance;

        macroGameController = GameObject.FindGameObjectWithTag("MacroGameController").GetComponent<MacroGameController>();
        center = startStand.transform.position;
    }

    private void Start()
    {
        pointsManager = GameObject.FindGameObjectWithTag("PlayerPoints").GetComponent<PointsManager>();
        initialPointsArray = new int[4] { pointsManager.player1Points, pointsManager.player2Points, pointsManager.player3Points, pointsManager.player4Points };
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
        //remove instructions
        if(instruction != null) instruction.SetActive(false);

        GameObject[] mobileRigs = GameObject.FindGameObjectsWithTag("MobileRigGameController");
        Debug.Log("num" + mobileRigs.Length);
        foreach (var mobiles in mobileRigs)
        {
            var mobileController = mobiles.GetComponent<RunningCupMobileRigController>();
            mobileController.GameStart();
            mobileController.instructions.SetActive(false);
        }

        if (!macroGameController.isMobileRig) Realtime.Instantiate("Pistol", startStand.transform.position, Quaternion.identity, instantiateOptions);
        gameStartCountdown = true;
        var smoke = Instantiate(smokeEffect, startStand.transform.position, Quaternion.identity);
        Destroy(smoke, 3);
        Destroy(startStand, 0.5f);
        //SpawnCups();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(cupSpawnCenter, cupSpawnSize);
    }

    public void AddPoint(int playerNumberHit)
    {
        if (!timerRunning) return;

        pointsManager.AddPoints(playerNumberHit, 1);
            
        points += 1;

        if(macroGameController.pointsManager != null && macroGameController.isMobileRig)
        {
            macroGameController.pointsManager.AddPoints(playerNumberHit, 1); //gives shot to player hitted
        }

        if (points == GameObject.FindGameObjectsWithTag("MobileRigGameController").Length) TimerComplete(); //killed all mobile players
         
    }

    void TimerComplete()
    {
        int[] pointsToAdd = new int[4];
        if (pointsManager.player2Points != initialPointsArray[1]) pointsToAdd[1] = 1; //player 2 died
        if (pointsManager.player3Points != initialPointsArray[2]) pointsToAdd[2] = 1; //player 3 died
        if (pointsManager.player4Points != initialPointsArray[3]) pointsToAdd[3] = 1; //player 4 died

        if (pointsToAdd[1] + pointsToAdd[2] + pointsToAdd[3] != 3) pointsToAdd[0] = 1; //vr player lost
        if (pointsToAdd[1] + pointsToAdd[2] + pointsToAdd[3] == 0) pointsToAdd[0] = 2; //vr player didnt kill anyone, so extra penalty

        macroGameController.AddShotsLocalGame(pointsToAdd[0], pointsToAdd[1], pointsToAdd[2], pointsToAdd[3]);

        timerRunning = false;

        // Code to execute when the timer is complete
        timerText.text = "Finish!";
        Debug.Log("Timer complete!");

        var smoke = Instantiate(smokeEffect, center, Quaternion.identity);
        Destroy(smoke, 3);

        if (!macroGameController.isMobileRig)
        {
            Realtime.Instantiate("ScoreBoard", center, Quaternion.Euler(0, 180f, 0), instantiateOptions);
        }
        else //is mobile
        {
            GameObject[] mobileRigs = GameObject.FindGameObjectsWithTag("MobileRigGameController");

            foreach (var mobiles in mobileRigs)
            {
                mobiles.GetComponent<RunningCupMobileRigController>().ShowScoreboard();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        AddPoint(other.gameObject.GetComponent<RunningCupMobileRigController>().playerNumber);
    }
}
