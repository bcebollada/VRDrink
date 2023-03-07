using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacroGameController : MonoBehaviour
{
    public int playerNumbers; //always will be 1 less than actual to be able to controll array, so if 2 players play, it will be 1

    public int[] playersPoints = new int[4]; //array used to controll players points

    public GameObject playerNumStand, playerSelectStartStand,smokeEffect,playerCamera;


    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
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
    }

    public void PlayersNum3()
    {
        playerNumbers = 2;
    }

    public void PlayersNum4()
    {
        playerNumbers = 3;
    }
}
