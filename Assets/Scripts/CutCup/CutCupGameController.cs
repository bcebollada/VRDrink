using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using TMPro;


public class CutCupGameController : MonoBehaviour
{
    public int points;
    public int pointsGoal; //player will need to this amout of points

    public TMP_Text pointText, timerText, startCountDownText;

    public float timeLeft = 30.0f;  // Set the timer duration in seconds
    private float countDown = 5f;
    public bool timerRunning, gameStartCountdown;
    private bool gameStarted;

    public GameObject startStand, grabVisualCue;


    [SerializeField] private MacroGameController macroGameController;

    private Realtime.InstantiateOptions instantiateOptions = new Realtime.InstantiateOptions();
    private Realtime realtimeInstance;

    public GameStartCommunicator gameStartCommunicator;

    public Vector3 spawnSize;
    public Vector3 spawnCenter;

    private void Awake()
    {
        realtimeInstance = GameObject.FindGameObjectWithTag("Room").GetComponent<Realtime>();

        //instantiateOptions.ownedByClient = true;
        instantiateOptions.useInstance = realtimeInstance;

        //set position
        Transform spawnTransform = GameObject.FindGameObjectWithTag("Respawn").transform;
        transform.parent.position = spawnTransform.position;
        transform.parent.rotation = spawnTransform.rotation;
    }

    private void Update()
    {
        if (!gameStarted)
        {
            if (gameStartCommunicator.gameStarted)
            {
                gameStarted = true;
                StartGame();
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

    public void StartGame()
    {
        //remove instructions
        GameObject[] instructions = GameObject.FindGameObjectsWithTag("Instructions");
        foreach (var instruction in instructions)
        {
            instruction.SetActive(false);
        }

        var smoke = Realtime.Instantiate("Thick Smoke Variant", startStand.transform.position, Quaternion.identity, instantiateOptions);
        StartCoroutine(DestroyRealtimeObject(smoke, 3));

        startStand.SetActive(false);

        gameStartCountdown = true;

        if (!macroGameController.isMobileRig)
        {
            Realtime.InstantiateOptions instantiateOptions2 = new Realtime.InstantiateOptions();
            instantiateOptions2.ownedByClient = true;
            Realtime.Instantiate("Katana", startStand.transform.position, Quaternion.identity, instantiateOptions2);
            Instantiate(grabVisualCue, startStand.transform.position, Quaternion.identity);

        }
    }

    void TimerComplete()
    {
        // Code to execute when the timer is complete
        timerText.text = "Finish!";
        Debug.Log("Timer complete!");

        if (!macroGameController.isMobileRig) //if is not mobile
        {
            var smoke = Realtime.Instantiate("Thick Smoke Variant", startStand.transform.position, startStand.transform.rotation, instantiateOptions);
            StartCoroutine(DestroyRealtimeObject(smoke, 3));

            Realtime.Instantiate("ScoreBoard", startStand.transform.position, Quaternion.Euler(0, 90f, 0), instantiateOptions);
        }
        else //is mobile
        {
            GameObject[] mobileRigs = GameObject.FindGameObjectsWithTag("MobileRig");

            foreach (var mobiles in mobileRigs)
            {
                mobiles.GetComponent<CutCupMobileController>().ShowScoreboard();
                mobiles.transform.position += new Vector3(0, 5, 0);
            }
        }
    }

    public void GameOver()
    {
        TimerComplete();
    }


    public void StartGameCupTurn()
    {
        Debug.Log("game start");
        gameStartCommunicator.SetBool(true);
    }

    private void OnDrawGizmos()
    {
        Color gizColor = Color.red;
        gizColor.a = 0.3f;
        Gizmos.color = gizColor;

        Gizmos.DrawCube(spawnCenter, spawnSize);
    }

    public void DebugSpawn()
    {
        SpawnObj("Bomb");
    }

    public void SpawnBomb()
    {
        SpawnObj("Bomb");
    }

    public void SpawnCup()
    {
        SpawnObj("CutCup");
    }

    public void SpawnObj(string objName)
    {
        Vector3 randomPoint = new Vector3(
                Random.Range(spawnCenter.x - spawnSize.x / 2, spawnCenter.x + spawnSize.x / 2),
                Random.Range(spawnCenter.y - spawnSize.y / 2, spawnCenter.y + spawnSize.y / 2),
                Random.Range(spawnCenter.z - spawnSize.z / 2, spawnCenter.z + spawnSize.z / 2)
            );

        Realtime.Instantiate(objName, randomPoint, Quaternion.identity, instantiateOptions);
    }

    private IEnumerator DestroyRealtimeObject(GameObject objectToDestroy, float secondsToDestroy)
    {
        yield return new WaitForSeconds(secondsToDestroy);
        Realtime.Destroy(objectToDestroy);
    }
}
