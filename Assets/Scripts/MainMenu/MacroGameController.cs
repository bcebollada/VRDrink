using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MacroGameController : MonoBehaviour
{
    public int playerNumbers; //always will be 1 less than actual to be able to controll array, so if 2 players play, it will be 1

    public string playerPlaying;

    public int[] playersPoints = new int[4]; //array used to controll players points

    public GameObject playerNumStand, playerSelectStartStand,smokeEffect,playerCamera;

    public int miniGamesPlayed; //int to know which game we are 0 = none 1 = beerpong 2 = flip cup 3 = roullete

    public bool isMainMacroController;


    private void Awake()
    {
        Debug.Log("awake");

        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");

        
    }

    private void Start()
    {

    }

    void Update()
    {
        
    }

    public void PlayersSelect()
    {
        var spawnPosition = playerSelectStartStand.transform.position;
        Destroy(playerSelectStartStand);
        var smoke = Instantiate(smokeEffect, spawnPosition, Quaternion.identity);
        Destroy(smoke, 3);
        Instantiate(playerNumStand, spawnPosition, Quaternion.identity);
    }

    public void PlayersNum2()
    {
        playerNumbers = 1;

        playersPoints[0] = 0; //sets the array for the number of players
        playersPoints[1] = 0;
        playersPoints[2] = -1; //negative for players not used
        playersPoints[3] = -1;

        NextGame();
    }

    public void PlayersNum3()
    {
        playerNumbers = 2;

        playersPoints[0] = 0; //sets the array for the number of players
        playersPoints[1] = 0;
        playersPoints[2] = 0; //negative for players not used
        playersPoints[3] = -1;

        NextGame();
    }

    public void PlayersNum4()
    {
        playerNumbers = 3;

        playersPoints[0] = 0; //sets the array for the number of players
        playersPoints[1] = 0;
        playersPoints[2] = 0; 
        playersPoints[3] = 0;

        NextGame();
    }


    public void NextGame() //checks how many players are planning depending on who is playing, restarts all the games
    {
        Debug.Log("Next Game");
        if (miniGamesPlayed == 0)
        {
            Debug.Log("Loading BeerPong");
            SceneManager.LoadScene("BeerPong_Scene");
            miniGamesPlayed = 1;
        }

        else if (miniGamesPlayed == 1)
        {
            Debug.Log("Loading RunningCup");
            SceneManager.LoadScene("RunningCup_Scene");
            miniGamesPlayed = 2;
        }

        else if (miniGamesPlayed == 2)
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
            DontDestroyOnLoad(this.gameObject);
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
