using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaitingBoard : MonoBehaviour
{
    private MacroGameController macroGameController;

    public TMP_Text player1Status, player2Status, player3Status, player4Status;

    public bool areAllPlayersConnected; //check bool to see if everyone connected already

    public GameObject startStand;

    private void Awake()
    {
        macroGameController = GameObject.Find("MacroGameController").GetComponent<MacroGameController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(macroGameController.playerNumbers == 1) //2 players playing
        {
            //deativate texts for others players
            player3Status.gameObject.transform.parent.gameObject.SetActive(false);
            player4Status.gameObject.transform.parent.gameObject.SetActive(false);
        }

        else if (macroGameController.playerNumbers == 2) //3 players playing
        {
            //deativate texts for others players
            player4Status.gameObject.transform.parent.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] mobileRigs = GameObject.FindGameObjectsWithTag("MobileRig");
        if(mobileRigs.Length >= 1 && !areAllPlayersConnected)
        {
            player2Status.text = "Connected";
            if (macroGameController.playerNumbers - 1 == 0) areAllPlayersConnected = true;
        }
        if (mobileRigs.Length >= 2 && !areAllPlayersConnected)
        {
            player3Status.text = "Connected";
            if (macroGameController.playerNumbers - 2 == 0) areAllPlayersConnected = true;
        }
        if (mobileRigs.Length == 3 && !areAllPlayersConnected)
        {
            player4Status.text = "Connected";
            if (macroGameController.playerNumbers - 3 == 0) areAllPlayersConnected = true;
        }

        if (areAllPlayersConnected) startStand.SetActive(true);
    }

    public void ButtonPress()
    {
        macroGameController.NextGame();
    }
}
