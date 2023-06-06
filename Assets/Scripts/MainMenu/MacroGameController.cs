using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Normal.Realtime;
using TMPro;


public class MacroGameController : MonoBehaviour
{
    public int playerNumbers; //always will be 1 less than actual to be able to controll array, so if 2 players play, it will be 1

    public string playerPlaying;

    public int[] playerLocalShots = new int[4]; //array used to controll players points in scpecific mini game
    public int[] playerOverallShots = new int[4]; //array used to controll players points in macrogame

    public GameObject playerSelectStartStand,smokeEffect,playerCamera, waitPlayersBoard;

    private GameObject playerNumStand;

    public int miniGamesPlayed; //int to know which game we are 0 = none 1 = beerpong 2 = flip cup 3 = roullete 

    public bool isMainMacroController;

    private Realtime.InstantiateOptions instantiateOptions = new Realtime.InstantiateOptions();

    public bool isMobileRig;

    public PointsManager pointsManager;


    private void Awake()
    {
        instantiateOptions.ownedByClient = true;

        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");

        if(GameObject.FindGameObjectWithTag("PlayerPoints") != null)
        {
            pointsManager = GameObject.FindGameObjectWithTag("PlayerPoints").GetComponent<PointsManager>();
        }

        if (GameObject.FindGameObjectWithTag("PlayerNumber") != null)
        {
            playerNumbers = GameObject.FindGameObjectWithTag("PlayerNumber").GetComponent<MobileRigPlayerNumber>().numberOfPlayers;
        }

    }

    private void Start()
    {
        if ((SystemInfo.deviceModel.Contains("Oculus Quest") || SystemInfo.deviceModel.Contains("Raider") || Application.platform == RuntimePlatform.WindowsEditor) && !isMobileRig) //isnt mobile rig
            isMobileRig = false;
        else isMobileRig = true;

    }

    void Update()
    {
        UpdatePlayerShots();
    }

    private void UpdatePlayerShots()
    {

    }


    public void PlayersSelect()
    {
        var spawnPosition = playerSelectStartStand.transform.position;
        Destroy(playerSelectStartStand);
        //var smoke = Instantiate(smokeEffect, spawnPosition, Quaternion.identity);
        //Destroy(smoke, 3);
        if(!isMobileRig) playerNumStand = Realtime.Instantiate("Players Number Stands", spawnPosition, Quaternion.identity, instantiateOptions);
    }

    public void PlayersNum2()
    {
        playerNumbers = 1;
        GameObject.FindGameObjectWithTag("PlayerNumber").GetComponent<MobileRigPlayerNumber>().numberOfPlayers = 1;

        playerLocalShots[0] = 0; //sets the array for the number of players
        playerLocalShots[1] = 0;
        playerLocalShots[2] = -1; //negative for players not used
        playerLocalShots[3] = -1;

        GameObject[] cups = GameObject.FindGameObjectsWithTag("Cup");
        foreach (var cup in cups)
        {
            Realtime.Destroy(cup);
        }

        PlayerConnecWait();
    }

    public void PlayersNum3()
    {
        playerNumbers = 2;
        GameObject.FindGameObjectWithTag("PlayerNumber").GetComponent<MobileRigPlayerNumber>().numberOfPlayers = 2;

        playerLocalShots[0] = 0; //sets the array for the number of players
        playerLocalShots[1] = 0;
        playerLocalShots[2] = 0; //negative for players not used
        playerLocalShots[3] = -1;

        GameObject[] cups = GameObject.FindGameObjectsWithTag("Cup");
        foreach (var cup in cups)
        {
            Realtime.Destroy(cup);
        }

        PlayerConnecWait();
    }

    public void PlayersNum4()
    {
        playerNumbers = 3;
        GameObject.FindGameObjectWithTag("PlayerNumber").GetComponent<MobileRigPlayerNumber>().numberOfPlayers = 3;

        playerLocalShots[0] = 0; //sets the array for the number of players
        playerLocalShots[1] = 0;
        playerLocalShots[2] = 0; 
        playerLocalShots[3] = 0;

        GameObject[] cups = GameObject.FindGameObjectsWithTag("Cup");
        foreach (var cup in cups)
        {
            Realtime.Destroy(cup);
        }

        PlayerConnecWait();
    }

    public void AddShotsLocalGame(int player1Shot, int player2Shot, int player3Shot, int player4Shot)
    {
        playerLocalShots[0] = player1Shot;
        playerLocalShots[1] = player2Shot;
        if(playerNumbers >= 2) playerLocalShots[2] = player3Shot;
        if(playerNumbers == 3) playerLocalShots[3] = player4Shot;
    }

    public void AddShotsOverall(int pointsParameter, int player)
    {

    }

    public void PlayerConnecWait() //waits for all player to connect in order to continue
    {
        if (isMobileRig) return; //just vr needs to instantiate
        Realtime.Instantiate("WaitPlayersBoard", playerNumStand.transform.position, playerNumStand.transform.rotation, instantiateOptions);
        Realtime.Destroy(playerNumStand);
    }

    public void NextGame() //checks how many players are planning depending on who is playing, restarts all the games
    {
        Debug.Log("Next Game");

        if (SceneManager.GetActiveScene().name == "MainMenu_Scene" || SceneManager.GetActiveScene().name == "MainMenuMobileRig_Scene")
        {
            Debug.Log("Loading BeerPong");

            if (!isMobileRig) //isnt mobile rig
            {
                SceneManager.LoadScene("BeerPong_Scene");
            }

            else
            {
                SceneManager.LoadScene("BeerPong_MobileRigScene"); //is mobile rig
            }
   
            miniGamesPlayed = 1;
        }

        else if (SceneManager.GetActiveScene().name == "BeerPong_Scene" || SceneManager.GetActiveScene().name == "BeerPong_MobileRigScene")
        {
            Debug.Log("Loading RunningCup");

            if (!isMobileRig) //isnt mobile rig
            {
                SceneManager.LoadScene("RunningCup_Scene");
            }

            else
            {
                SceneManager.LoadScene("RunningCupMobileRig_Scene"); //is mobile rig
            }

            miniGamesPlayed = 2;
        }

        else if (SceneManager.GetActiveScene().name == "RunningCup_Scene" || SceneManager.GetActiveScene().name == "RunningCupMobileRig_Scene")
        {
            if (playerNumbers == 1 && playerPlaying != "Player2")
            {
                SceneManager.LoadScene("BeerPong_Scene");
                ChangePlayer();
                miniGamesPlayed = 1;
            }

            else if (playerNumbers == 2 && playerPlaying != "Player3")
            {
                SceneManager.LoadScene("BeerPong_Scene");
                ChangePlayer();
                miniGamesPlayed = 1;
            }

            else if (playerNumbers == 3 && playerPlaying != "Player4")
            {
                SceneManager.LoadScene("BeerPong_Scene");
                ChangePlayer();
                miniGamesPlayed = 1;
            }
        }
    }

    public void ChangePlayer()
    {
        //checks which player is playing then it changes to the next one
        if (playerPlaying == "Player1")
        {
            Debug.Log("Change to player 2");
            playerPlaying = "Player2";
        }
        else if (playerPlaying == "Player2")
        {
            if (playerNumbers > 1)
            {
                Debug.Log("Change to player 3");
                playerPlaying = "Player3";
            }

            else EndGame();
        }

        else if (playerPlaying == "Player3")
        {
            if (playerNumbers > 2)
            {
                Debug.Log("Change to player 4");
                playerPlaying = "Player4";
            }
            else EndGame();
        }
        else if (playerPlaying == "Player4") EndGame();
    }

    public void EndGame()
    {
        Debug.Log("Game Ended");
    }

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);

        if (isMainMacroController)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("MacroGameController");

            foreach (GameObject obj in objs)
            {
                if (obj.GetComponent<MacroGameController>().miniGamesPlayed < miniGamesPlayed && obj != this.gameObject) Destroy(obj); ; //checks which macro has made more games, destroys the lower one
            }
            //DontDestroyOnLoad(this.gameObject);
            playerCamera = GameObject.FindGameObjectWithTag("MainCamera");

        }
    }

    // called when the game is terminated
    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
