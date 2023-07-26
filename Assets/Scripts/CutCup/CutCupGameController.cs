using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using TMPro;


public class CutCupGameController : MonoBehaviour
{
    public TMP_Text pointText, timerText, startCountDownText;

    public int VRPoints, mobilePoints;
    public int pointsGoal; //player will need to this amout of points

    public float timeLeft = 30.0f;  // Set the timer duration in seconds
    private float countDown = 5f;
    public bool timerRunning, gameStartCountdown;
    private bool gameStarted;

    public GameObject startStand, grabVisualCue;
    public GameObject[] cannons;


    [SerializeField] private MacroGameController macroGameController;

    private Realtime.InstantiateOptions instantiateOptions = new Realtime.InstantiateOptions();
    private Realtime realtimeInstance;

    public GameStartCommunicator gameStartCommunicator;

    public Vector3 spawnSize;
    public Vector3 spawnCenter;

    public Transform laucherTransform;
    public float launchTimeDuration;

    public AudioSource audioSource;
    public enum soundNames { ninjaCut, goodAim }
    public AudioClip[] soundEffects;

    public CutTheCupPointsCommunicator pointsCommunicator;

    private void Awake()
    {
        realtimeInstance = GameObject.FindGameObjectWithTag("Room").GetComponent<Realtime>();

        //instantiateOptions.ownedByClient = true;
        instantiateOptions.useInstance = realtimeInstance;

        if (audioSource == null)
        {
            if (GetComponent<AudioSource>() != null)
            {
                audioSource = GetComponent<AudioSource>();
            }
            else Debug.LogWarning("No audio source on game controller");
        }
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
            instantiateOptions2.useInstance = realtimeInstance;
            Realtime.Instantiate("Katana", startStand.transform.position, Quaternion.identity, instantiateOptions2);
            Instantiate(grabVisualCue, startStand.transform.position, Quaternion.identity);

        }

        SetCannons(); //set cannons according to player numbers
    }

    private void SetCannons()
    {
        if(macroGameController.playerNumbers == 1)
        {
            cannons[1].SetActive(false);
            cannons[2].SetActive(false);
        }
        else if(macroGameController.playerNumbers == 2)
        {
            cannons[2].SetActive(false);
        }
    }

    void TimerComplete()
    {
        // Code to execute when the timer is complete
        timerText.text = "Finish!";
        Debug.Log("Timer complete!");

        if (VRPoints > mobilePoints) //vr player won
        {
            macroGameController.AddShotsLocalGame(0, 1, 1, 1);
        }
        else if ( mobilePoints > VRPoints) //vr player lost
        {
            macroGameController.AddShotsLocalGame(1, 0, 0, 0);
        }
        else //draw
        {
            macroGameController.AddShotsLocalGame(1, 1, 1, 1);
        }

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

    public void AddLocalVRPoint(int points)
    {
        //VRPoints += points;
        pointText.text = $"{VRPoints} /{pointsGoal}";
        pointsCommunicator.AddPoints(0, 1);

    }

    public void AddLocalMobilePoint(int points)
    {
        //mobilePoints += points;
        //pointText.text = $"{VRPoints} /{pointsGoal}";
        pointsCommunicator.AddPoints(1, 0);
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
        //SpawnObj("Bomb");
        LaunchObj("Bomb", Vector3.zero);
    }

    public void SpawnCup()
    {
        //SpawnObj("CutCup");
        LaunchObj("CutCup", Vector3.zero);
    }

    public void LaunchObj(string objToSpawn, Vector3 targetArea)
    {
        //var targetArea = spawnCenter + new Vector3(Random.Range(-spawnSize.x / 2, spawnSize.x / 2), 0, Random.Range(-spawnSize.z / 2, spawnSize.z / 2));

        var Vo = CalculateVelocity(targetArea, laucherTransform.position, launchTimeDuration);

        //GameObject obj = Instantiate(bola, gameObject.transform.position, Quaternion.identity);
        GameObject obj = Realtime.Instantiate(objToSpawn, laucherTransform.position, rotation: Quaternion.identity, instantiateOptions);

        obj.GetComponent<Rigidbody>().velocity = Vo;
    }

    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        //define the distance x and y first
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

        //create a float the represent our distance
        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;
        return result;
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

    public void PlaySound(soundNames soundName)
    {
        audioSource.PlayOneShot(soundEffects[(int)soundName]);
    }
}
